using WebAPI.Dtos.BuildingImage;

namespace WebAPI.Dtos.Building
{
    public class BuildingCreatorDto : BuildingBaseDto 
    {
        // Hereda todos los campos de BuildingBaseDto
        
        /// <summary>
        /// Lista de imágenes del edificio que se crearán junto con el edificio
        /// </summary>
        public List<BuildingImageCreatorDto>? Images { get; set; } = new List<BuildingImageCreatorDto>();
    }
}
