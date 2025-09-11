using Microsoft.AspNetCore.Mvc;
using ServiceLibraryProject;
using ClassLibraryProject.Entities;
using System.Diagnostics;
using WebAPI.Dtos;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImageStorageController : ControllerBase
    {
        private readonly BuildingImageService _buildingImageService;
        private readonly ILogger<ImageStorageController> _logger;

        public ImageStorageController(
            BuildingImageService buildingImageService,
            ILogger<ImageStorageController> logger)
        {
            _buildingImageService = buildingImageService;
            _logger = logger;
        }

        /// <summary>
        /// Sube una imagen y la almacena en la base de datos
        /// </summary>
        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadImage([FromForm] ImageUploadDto uploadDto)
        {
            var stopwatch = Stopwatch.StartNew();
            var requestId = HttpContext.Items["RequestId"]?.ToString() ?? Guid.NewGuid().ToString();

            _logger.LogInformation("Iniciando upload de imagen - BuildingId: {BuildingId}, FileName: {FileName}, Size: {FileSize}MB, RequestId: {RequestId}",
                uploadDto.BuildingId, uploadDto.FileName ?? uploadDto.File?.FileName ?? "unknown", (uploadDto.File?.Length ?? 0) / 1024.0 / 1024.0, requestId);

            try
            {
                // Validaciones de entrada
                if (uploadDto.File == null || uploadDto.File.Length == 0)
                {
                    _logger.LogWarning("Upload fallido - Archivo vacío o nulo. RequestId: {RequestId}", requestId);
                    return BadRequest("Archivo no válido");
                }

                if (uploadDto.File.Length > 5_242_880) // 5MB
                {
                    _logger.LogWarning("Upload fallido - Archivo demasiado grande: {FileSize}MB. RequestId: {RequestId}", 
                        uploadDto.File.Length / 1024.0 / 1024.0, requestId);
                    return BadRequest("El archivo excede el tamaño máximo permitido (5MB)");
                }

                // Validar tipo de archivo
                var allowedTypes = new[] { "image/jpeg", "image/jpg", "image/png", "image/webp", "image/gif" };
                if (!allowedTypes.Contains(uploadDto.File.ContentType.ToLower()))
                {
                    _logger.LogWarning("Upload fallido - Tipo de archivo no permitido: {ContentType}. RequestId: {RequestId}", 
                        uploadDto.File.ContentType, requestId);
                    return BadRequest($"Tipo de archivo no permitido: {uploadDto.File.ContentType}");
                }

                _logger.LogDebug("Validaciones pasadas - Procesando archivo. RequestId: {RequestId}", requestId);

                // Convertir archivo a bytes
                using var memoryStream = new MemoryStream();
                await uploadDto.File.CopyToAsync(memoryStream);
                var imageData = memoryStream.ToArray();

                // Crear registro de imagen
                var buildingImageId = Guid.NewGuid().ToString();
                var buildingImage = new BuildingImage
                {
                    BuildingImageId = buildingImageId,
                    BuildingId = uploadDto.BuildingId,
                    FileName = uploadDto.FileName ?? uploadDto.File.FileName,
                    Url = $"/api/ImageStorage/download/{buildingImageId}",
                    AltText = uploadDto.AltText ?? "Imagen del edificio",
                    IsCoverImage = false,
                    ImageData = imageData,
                    Building = null! // EF lo manejará
                };

                _logger.LogDebug("Guardando imagen en base de datos - BuildingImageId: {BuildingImageId}. RequestId: {RequestId}", 
                    buildingImageId, requestId);

                await _buildingImageService.AddAsync(buildingImage);

                stopwatch.Stop();

                var response = new
                {
                    Message = "Imagen subida correctamente",
                    BuildingImageId = buildingImage.BuildingImageId,
                    Size = imageData.Length,
                    DownloadUrl = $"/api/ImageStorage/download/{buildingImage.BuildingImageId}"
                };

                _logger.LogInformation("Upload completado exitosamente - BuildingImageId: {BuildingImageId}, Size: {Size}bytes, Duración: {ElapsedMs}ms, RequestId: {RequestId}",
                    buildingImage.BuildingImageId, imageData.Length, stopwatch.ElapsedMilliseconds, requestId);

                return Ok(response);
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                _logger.LogError(ex, "Error durante upload de imagen - BuildingId: {BuildingId}, FileName: {FileName}, Duración: {ElapsedMs}ms, RequestId: {RequestId}",
                    uploadDto.BuildingId, uploadDto.FileName ?? uploadDto.File?.FileName ?? "unknown", stopwatch.ElapsedMilliseconds, requestId);
                
                throw; // Dejar que el middleware global de excepciones lo maneje
            }
        }

        /// <summary>
        /// Descarga una imagen por su ID
        /// </summary>
        [HttpGet("download/{buildingImageId}")]
        public async Task<IActionResult> DownloadImage(string buildingImageId)
        {
            try
            {
                var image = await _buildingImageService.GetByIdAsync(buildingImageId);
                
                if (image?.ImageData == null)
                    return NotFound("Imagen no encontrada");

                var contentType = GetContentType(image.FileName);
                return File(image.ImageData, contentType, image.FileName);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Lista las imágenes de un edificio
        /// </summary>
        [HttpGet("building/{buildingId}")]
        public async Task<IActionResult> GetBuildingImages(string buildingId)
        {
            try
            {
                var images = await _buildingImageService.GetByBuildingIdAsync(buildingId);
                
                var result = images.Select(img => new
                {
                    BuildingImageId = img.BuildingImageId,
                    FileName = img.FileName,
                    AltText = img.AltText,
                    IsCoverImage = img.IsCoverImage,
                    HasImageData = img.ImageData != null && img.ImageData.Length > 0,
                    Size = img.ImageData?.Length ?? 0,
                    DownloadUrl = $"/api/ImageStorage/download/{img.BuildingImageId}"
                });

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Actualiza el texto alternativo de una imagen
        /// </summary>
        [HttpPatch("update-alt/{buildingImageId}")]
        public async Task<IActionResult> UpdateAltText(string buildingImageId, [FromBody] UpdateAltTextRequest request)
        {
            try
            {
                var image = await _buildingImageService.GetByIdAsync(buildingImageId);
                if (image == null)
                    return NotFound("Imagen no encontrada");

                image.AltText = request.AltText;
                await _buildingImageService.UpdateAsync(image);

                return Ok(new { Message = "Texto alternativo actualizado correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Elimina una imagen
        /// </summary>
        [HttpDelete("delete/{buildingImageId}")]
        public async Task<IActionResult> DeleteImage(string buildingImageId)
        {
            try
            {
                var image = await _buildingImageService.GetByIdAsync(buildingImageId);
                if (image == null)
                    return NotFound("Imagen no encontrada");

                await _buildingImageService.DeleteAsync(buildingImageId);

                return Ok(new { Message = "Imagen eliminada correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Establece una imagen como portada (y quita la portada de las demás)
        /// </summary>
        [HttpPatch("set-cover/{buildingImageId}")]
        public async Task<IActionResult> SetAsCoverImage(string buildingImageId)
        {
            try
            {
                var image = await _buildingImageService.GetByIdAsync(buildingImageId);
                if (image == null)
                    return NotFound("Imagen no encontrada");

                // Obtener todas las imágenes del edificio
                var buildingImages = await _buildingImageService.GetByBuildingIdAsync(image.BuildingId);
                
                // Quitar portada de todas las imágenes
                foreach (var img in buildingImages)
                {
                    img.IsCoverImage = false;
                    await _buildingImageService.UpdateAsync(img);
                }

                // Establecer esta imagen como portada
                image.IsCoverImage = true;
                await _buildingImageService.UpdateAsync(image);

                return Ok(new { Message = "Imagen establecida como portada correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        private string GetContentType(string fileName)
        {
            var extension = Path.GetExtension(fileName).ToLowerInvariant();
            return extension switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".bmp" => "image/bmp",
                ".webp" => "image/webp",
                _ => "application/octet-stream"
            };
        }
    }

    public class UpdateAltTextRequest
    {
        public string AltText { get; set; } = string.Empty;
    }
}
