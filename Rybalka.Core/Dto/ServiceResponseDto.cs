namespace Rybalka.Core.Dto
{
    public sealed class ServiceResponseDto
    {
        public bool IsSuccess { get; set; } = true;
        public string ErrorMessage { get; set; } = string.Empty;
        public int StatusCode { get; set; } = 200;
    }
}
