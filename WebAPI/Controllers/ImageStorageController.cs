using Microsoft.AspNetCore.Mvc;
using ServiceLibraryProject;
using ClassLibraryProject.Entities;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImageStorageController : ControllerBase
    {
        private readonly BuildingImageService _buildingImageService;

        public ImageStorageController(BuildingImageService buildingImageService)
        {
            _buildingImageService = buildingImageService;
        }

        /// <summary>
        /// Sube una imagen y la almacena en la base de datos
        /// </summary>
        [HttpPost("upload")]
        public async Task<IActionResult> UploadImage([FromForm] string buildingId, [FromForm] string fileName, [FromForm] string altText, [FromForm] IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                    return BadRequest("Archivo no válido");

                // Convertir archivo a bytes
                using var memoryStream = new MemoryStream();
                await file.CopyToAsync(memoryStream);
                var imageData = memoryStream.ToArray();

                // Crear registro de imagen
                var buildingImage = new BuildingImage
                {
                    BuildingImageId = Guid.NewGuid().ToString(),
                    BuildingId = buildingId,
                    FileName = fileName ?? file.FileName,
                    Url = $"/api/ImageStorage/download/{Guid.NewGuid()}",
                    AltText = altText ?? "Imagen del edificio",
                    IsCoverImage = false,
                    ImageData = imageData,
                    Building = null! // EF lo manejará
                };

                await _buildingImageService.AddAsync(buildingImage);

                return Ok(new
                {
                    Message = "Imagen subida correctamente",
                    BuildingImageId = buildingImage.BuildingImageId,
                    Size = imageData.Length,
                    DownloadUrl = $"/api/ImageStorage/download/{buildingImage.BuildingImageId}"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
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
}
