using WebAPI.Dtos.BuildingImage;

namespace WebAPI.Dtos.Building
{
    /// <summary>
    /// DTO para crear un edificio junto con sus imágenes
    /// </summary>
    public class BuildingWithImagesCreatorDto : BuildingCreatorDto
    {
        /// <summary>
        /// Lista de imágenes en base64 que se subirán junto con el edificio
        /// </summary>
        public List<ImageFileDto>? ImageFiles { get; set; } = new List<ImageFileDto>();
    }

    /// <summary>
    /// Representa un archivo de imagen con sus datos
    /// </summary>
    public class ImageFileDto
    {
        /// <summary>
        /// Nombre del archivo
        /// </summary>
        public string FileName { get; set; } = string.Empty;

        /// <summary>
        /// Contenido del archivo en base64
        /// </summary>
        public string FileContent { get; set; } = string.Empty;

        /// <summary>
        /// Tipo de contenido (image/jpeg, image/png, etc.)
        /// </summary>
        public string ContentType { get; set; } = string.Empty;

        /// <summary>
        /// Texto alternativo para la imagen
        /// </summary>
        public string AltText { get; set; } = string.Empty;

        /// <summary>
        /// Si es la imagen de portada
        /// </summary>
        public bool IsCoverImage { get; set; }
    }
}
