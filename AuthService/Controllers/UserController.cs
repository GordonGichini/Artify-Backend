
using AuthService.Models.Dtos;
using AuthService.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUser _userService;
        private readonly ResponseDto _response;
        private readonly IConfiguration _configuration;

        public UserController(IUser user, IConfiguration configuration)
        {
            _userService = user;
            _configuration = configuration;
            _response = new ResponseDto();
        }
        [HttpPost("register")]
        public async Task<ActionResult<ResponseDto>> RegisterUser(RegisterUserDto registerUserDto)
        {
            var res = await _userService.RegisterUser(registerUserDto);
            if(string.IsNullOrWhiteSpace(res))
            {
                //success
                _response.Result = "User Registered successfully";
                return Created("", _response);
            }
            _response.ErrorMessage = res;
            _response.IsSuccess = false;
            return BadRequest(_response);
        }
        [HttpPost("login")]
        public async Task<ActionResult<ResponseDto>> loginUser(LoginRequestDto loginRequestDto)
        {
            var res = await _userService.loginUser(loginRequestDto);

            if (res.User!=null)
            {
                _response.Result = res;
                return Created("", _response);
            }

            _response.ErrorMessage = "Invalid Credentials";
            _response.IsSuccess = false;
            return BadRequest(_response);

        }

        [HttpPost("AssignRole")]
        public async Task<ActionResult<ResponseDto>> AssignRole(RegisterUserDto registerUserDto)
        {
            var res = await _userService.AssignUserRoles(registerUserDto.Email, registerUserDto.Role);
            if (res)
            {
                //success
                _response.Result = res;
                return Ok(_response);
            }
            _response.ErrorMessage = "Error Occurred ";
            _response.Result = res;
            _response.IsSuccess = false;
            return BadRequest(_response);
        }
    }
}
