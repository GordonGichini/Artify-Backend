namespace ArtworkService.Models.Dtos
{
    public class AddArtworkDto
    {
        public Guid SellerId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public DateTime StartTime { get; set; } = DateTime.Now;
        public DateTime StopTime { get; set; }
        public double StartingPrice { get; set; }
        public string Category { get; set; } = string.Empty;
    }
}
