namespace ArtworkService.Models
{
    public class Artwork
    {
        public Guid Id { get; set; }
        public Guid SellerId { get; set; }
        public string ArtworkName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public DateTime StartTime { get; set; } = DateTime.Now;
        public DateTime StopTime { get; set; }
        public int StartingPrice { get; set; }
        public int HighestBid { get; set; }
        public string Category { get; set; } = string.Empty;
        public string Status { get; set; } = "Open";
    }
}
