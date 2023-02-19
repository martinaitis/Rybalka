using System.ComponentModel.DataAnnotations;

namespace Rybalka.Core.Dto.Auth
{
    public sealed class RegisterDto
    {
        public required string UserName { get; set; }

        [DataType(DataType.Password)]
        public required string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Password and confirmation password did not match")]
        public required string ConfirmPassword { get; set; }
    }
}
