namespace Rybalka.Core.Dto
{
    public sealed class ServiceResponseDto
    {
        public bool IsSuccess { get; set; } = true;
        public Dictionary<string, string> Result { get; set; } = new Dictionary<string, string>();
        public string ErrorMessage { get; set; } = string.Empty;
        public int StatusCode { get; set; } = 200;
    }
}
