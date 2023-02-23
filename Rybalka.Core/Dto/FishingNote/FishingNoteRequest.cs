using Microsoft.AspNetCore.Http;
using Rybalka.Core.Dto.ValidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace Rybalka.Core.Dto.FishingNote
{
    public sealed class FishingNoteRequest
    {
        [MaxLength(15)]
        [MinLength(4)]
        public required string User { get; set; }

        public string? Title { get; set; }

        public required DateTime StartTime { get; set; }

        public required DateTime EndTime { get; set; }

        public required CoordinatesDto Coordinates { get; set; }

        public required string WaterBody { get; set; }

        public required int FishingMethod { get; set; }

        public required int FishCount { get; set; }
        public string? Bait { get; set; }
        public string? Description { get; set; }
        public decimal? Temp { get; set; }
        public decimal? WindKph { get; set; }
        public string? WindDir { get; set; }
        public decimal? CloudPct { get; set; }
        public string? ConditionText { get; set; }

        [DataType(DataType.Upload)]
        [MaxFileSize(10 * 1024 * 1024)] //10MB
        [AllowedImageExtensions(new string[] { ".jpg", ".jpeg", ".png", ".tiff" })]
        public IFormFile? Image { get; set; }
    }
}
