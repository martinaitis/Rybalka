using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Rybalka.Core.Dto.FishingNote
{
    public sealed class FishingNoteRequest
    {
        [Required]
        [MaxLength(15)]
        [MinLength(4)]
        public string? User { get; set; }

        public string? Title { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        [Required]
        public CoordinatesDto? Coordinates { get; set; }

        [Required]
        public string? WaterBody { get; set; }

        [Required]
        public int? FishingMethod { get; set; }

        [Required]
        public int? FishCount { get; set; }
        public string? Bait { get; set; }
        public string? Description { get; set; }
        public decimal? Temp { get; set; }
        public decimal? WindKph { get; set; }
        public string? WindDir { get; set; }
        public decimal? CloudPct { get; set; }
        public string? ConditionText { get; set; }
        public IFormFile? Image { get; set; }
    }
}
