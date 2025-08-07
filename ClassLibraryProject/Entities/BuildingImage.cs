namespace ClassLibraryProject.Entities
{
    public class BuildingImage
    {
        public required string BuildingImageId { get; set; }
        public required string BuildingId { get; set; }
        public required string FileName { get; set; }
        public required string FilePath { get; set; }
        public required string AltText { get; set; }
        public bool IsCoverImage { get; set; }
        public required virtual Building Building { get; set; }

        public BuildingImage() { }
    }
}
