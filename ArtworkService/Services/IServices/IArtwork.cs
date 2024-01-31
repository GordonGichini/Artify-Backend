using ArtworkService.Models;


namespace ArtworkService.Services.IServices
{
    public interface IArtwork
    {
        Task<List<Artwork>> GetAllArtworks();
        Task<Artwork> GetArtwork(Guid Id);
        Task<List<Artwork>> GetMyArtworks(Guid userId);
        Task<string> AddArtwork(Artwork artwork);
        Task<string> UpdateArtwork();
        Task<string> DeleteArtwork(Artwork artwork);
    }
}
