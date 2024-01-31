using AutoMapper;
using ArtworkService.Models;
using ArtworkService.Models.Dtos;

namespace ArtworkService.Profiles
{
    public class ArtworkProfiles : Profile
    {
        public ArtworkProfiles()
        {
            CreateMap<AddArtworkDto, Artwork>().ReverseMap();

        }
    }
}
