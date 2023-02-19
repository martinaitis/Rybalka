namespace Rybalka.Core.Dto
{
    public sealed class ServiceResponseDto
    {
        public bool IsSuccess { get; set; }

        public string? ErrorMessage { get; set; }
    }
}
