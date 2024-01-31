using ArtworkService.Models.Dtos;

namespace ArtworkService.Services.IServices
{
    public interface IUser
    {
        Task<UserDto> GetUserById(string Id);
    }
}
