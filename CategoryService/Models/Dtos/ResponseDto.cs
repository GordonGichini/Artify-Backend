namespace Categoryservice.Models.Dtos
{
    public class ResponseDto
    {
        public string ErrorMessage { get; set; } = string.Empty;
        public bool IsSuccess { get; set; } = true;
        public object Result { get; set; } = default!;
    }
}