using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ArtworkService.Models;
using ArtworkService.Models.Dtos;
using ArtworkService.Services.IServices;

namespace ArtworkService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtworkController : ControllerBase
    {
        private readonly IArtwork _artworkService;
        private readonly IMapper _mapper;
        private readonly ResponseDto _response;
        private readonly IUser _userService;

        public ArtworkController(IUser userService IArtwork artwork, IBid bidServices
            IMapper mapper)
        {
            _userService = userService;
            _artworkService = artwork;
            _bidServices = bidServices;
            _mapper = mapper;
            _response = new ResponseDto();
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Seller")]
        public async Task<ActionResult<ResponseDto>> AddArtwork(AddArtworkDto artwork)
        {
            var UserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (UserId == null)
            {
                _response.Errormessage = "Please login to add artwork";
                return Unauthorized(_response);
            }

            var art = _mapper.Map<Artwork>(newArtwork);
            art.SellerId = Guid.Parse(UserId);
            var res = await _artworkService.AddNewArtwork(art);
            _response.Result = res;
            return Created("", _response);

        }
        [HttpGet]
        public async Task<ActionResult<ResponseDto>> GetAllArtworks()
        {
            var arts = await _artworkService.GetAllArtworks();
            _responseDto.Result = arts;
            return Ok(_responseDto);

        }
        [HttpGet("myArtworks")]
        public async Task<ActionResult<ResponseDto>> GetMyArtworks()
        {
            var UserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (UserId == null)
            {
                _responseDto.Errormessage = "Please login to view your art";
                return Unauthorized(_responseDto);
            }
            var artworks = await _artworkService.GetMyArtworks(Guid.Parse(UserId));
            _response.Result = arts;
            return Ok(_response);
        }

        [HttpPut("{Id}")]
        [Authorize(Roles = "Seller")]
        public async Task<ActionResult<ResponseDto>> UpdateArtwork(Guid Id, AddArtworkDto Uartwork)
        {
            var artwork = await _artworkService.GetArtwork(Id);

            if (artwork == null)
            {
                _responseDto.Result = "Artwork Not found";
                _responseDto.IsSuccess = false;
                return NotFound(_responseDto);
            }
            _mapper.Map(Uartwork, artwork);
            var res = await _artworkService.UpdateArtwork();
            _responseDto.Result = res;
            return Ok(_responseDto);
        }



        [HttpDelete("{Id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ResponseDto>> DeleteArtwork (Guid Id)
        {
            var artwork = await _artworkService.GetArtwork(Id);

            if (artwork == null)
            {
                _responseDto.Result = "Artwork Not found";
                _responseDto.IsSuccess = false;
                return NotFound(_responseDto);
            }
            var res = await _artworkService.DeleteArtwork(artwork);
            _responseDto.Result = res;
            return Ok(_responseDto);

        }
    } 
}
