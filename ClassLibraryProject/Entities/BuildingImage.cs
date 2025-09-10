using System;

namespace ClassLibraryProject.Entities
{
    public class BuildingImage
    {
        public required string BuildingImageId { get; set; }
        public required string BuildingId { get; set; }
        public required string FileName { get; set; }
        public required string Url { get; set; }
        public required string AltText { get; set; }
        public bool IsCoverImage { get; set; }
        
        // Columnas para FILESTREAM
        public Guid RowGuid { get; set; } = Guid.NewGuid();
        public byte[]? ImageData { get; set; }
        
        public required virtual Building Building { get; set; }

        public BuildingImage() { }
    }
}
