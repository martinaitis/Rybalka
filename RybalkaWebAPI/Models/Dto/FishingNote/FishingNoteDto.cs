using System.ComponentModel.DataAnnotations;
using static RybalkaWebAPI.Models.AppEnums;

namespace RybalkaWebAPI.Models.Dto.FishingNote
{
    public class FishingNoteDto
    {
        public int Id { get; set; }

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
        public FishingMethods? FishingMethod { get; set; }

        [Required]
        public decimal? FishCount { get; set; }
        public string? Bait { get; set; }
        public string? Note { get; set; }
        public decimal? Temp { get; set; }
        public decimal? WindKph { get; set; }
        public string? WindDir { get; set; }
        public decimal? CloudPct { get; set; }
        public string? ConditionText { get; set; }
    }
}
