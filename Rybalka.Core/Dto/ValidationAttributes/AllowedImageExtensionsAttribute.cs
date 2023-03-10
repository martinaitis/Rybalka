using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Rybalka.Core.Dto.ValidationAttributes
{
    public class AllowedImageExtensionsAttribute : ValidationAttribute
    {
        private readonly string[] _extensions;
        public AllowedImageExtensionsAttribute(string[] extensions)
        {
            _extensions = extensions;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value != null && value is IFormFile file)
            {
                var extension = Path.GetExtension(file.FileName);
                if (!_extensions.Contains(extension.ToLower()))
                {
                    return new ValidationResult("This image extension is not allowed.");
                }
            }

            return ValidationResult.Success;
        }
    }
}
