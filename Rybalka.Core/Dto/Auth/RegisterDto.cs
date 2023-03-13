using System.ComponentModel.DataAnnotations;

namespace Rybalka.Core.Dto.Auth
{
    public sealed class RegisterDto
    {
        [StringLength(30, MinimumLength = 3)]
        public required string Username { get; set; }

        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 6)]
        public required string Password { get; set; }
    }
}
