using AutoMapper;
using Categoryservice.Models;
using Categoryservice.Models.Dtos;

namespace Categoryservice.Profiles
{
    public class CategoryProfiles:Profile
    {
        public CategoryProfiles()
        {
            CreateMap<CategoryDto, Category>().ReverseMap();
        }
    }
}