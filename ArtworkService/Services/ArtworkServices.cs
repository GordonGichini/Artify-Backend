using Microsoft.EntityFrameworkCore;
using ArtworkService.Data;
using ArtworkService.Models;
using ArtworkService.Models.Dtos;
using ArtworkService.Services.IServices;

namespace ArtworkService.Services
{
    public class ArtworkServices : IArtwork
    {
        private readonly ApplicationDbContext _context;
        public ArtworkServices(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<string> AddArtwork(Artwork artwork)
        {
            _context.Artwork.Add(artwork);
            await _context.SaveChangesAsync();
            return "Artwork Added Successfully";
        }

        public async Task<string> DeleteArtwork(Artwork artwork)
        {
            _context.Artwork.Remove(artwork);
            await _context.SaveChangesAsync();
            return "Artwork Deleted Successfully!";
        }

       public async Task<List<Artwork>> GetAllArtworks()
       {
        return await _context.Artworks.ToListAsync();
       }

        public async Task<Artwork> GetArtwork(Guid Id)
        {
            return await _context.Artworks.Where(b => b.Id == Id).FirstOrDefaultAsync();
        }

        public async Task<List<Artwork>> GetMyArtworks(Guid userId)
        {
            return await_context.Artworks.Where(x => x.SellerId == userId).ToListAsync();
        }

        public async  Task<string> UpdateArtwork()
        {
            await _context.SaveChangesAsync();
            return "Artwork Updated Successfully"
        }
    }
}
