using System.ComponentModel.DataAnnotations;

namespace Rybalka.Core.Dto.Auth
{
    public sealed class LoginDto
    {
        public required string UserName { get; set; }

        [DataType(DataType.Password)]
        public required string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
