using AutoMapper;
using Categoryservice.Models;
using Categoryservice.Models.Dtos;
using Categoryservice.Service.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Categoryservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategory _categoryServices;
        private readonly IMapper _mapper;
        private readonly ResponseDto _responseDto;
        public CategoryController(IMapper mapper, ICategory category)
        {
            _mapper = mapper;
            _responseDto = new ResponseDto();
            _categoryServices = category;
            
        }
        [HttpGet]
        public async Task<ActionResult<ResponseDto>> GetAll()
        {
            var art= await _categoryServices.GetAll();
            _responseDto.Result = art;
            return Ok(_responseDto);
        }
        [HttpPost]
        public async Task<ActionResult<ResponseDto>>AddCategory(CategoryDto category)
        {
            var art = _mapper.Map<Category>(category);
            var res = await _categoryServices.CreateCategory(art);
            _responseDto.Result = art;
            return Ok(_responseDto);
           
        }
        [HttpGet("{Id}")]
        public async Task<ActionResult<ResponseDto>>GetCategory(Guid Id)
        {
            var art = await _categoryServices.GetCategoryById(Id);
            if(art == null)
            {
                return NotFound(_responseDto);
            }
            _responseDto.Result=art;
            return Ok(_responseDto);
        }
        [HttpDelete("{Id}")]
        public async Task<string> DeleteCategory(Guid Id)
        {
            var art = await _categoryServices.GetCategoryById(Id);
            if (art == null)
            {
                return "Category not found";
            }
            var res =await _categoryServices.DeleteCategory(art);
            return "Category deleted successfully";
         }
        [HttpPut("{Id}")]
        public async Task<ActionResult<ResponseDto>>UpdateCategory(Guid Id, CategoryDto categoryDto)
        {
            var art = await _categoryServices.GetCategoryById(Id);
            if( art == null)
            {
                return NotFound(_responseDto);
            }
            var cat = _mapper.Map<Category>(categoryDto);
            var res = await _categoryServices.UpdateCategory(cat);
            _responseDto.ErrorMessage = "Category Updated";
            return Ok(_responseDto);


        }
    }
    
}