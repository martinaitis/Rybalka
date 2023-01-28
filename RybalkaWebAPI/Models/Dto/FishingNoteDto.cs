using System.ComponentModel.DataAnnotations;
using static RybalkaWebAPI.Models.AppEnums;

namespace RybalkaWebAPI.Models.Dto
{
    public class FishingNoteDto
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(15)]
        [MinLength(4)]
        public string? User { get; set; }

        [Required]
        public DateTime? FishingDate { get; set; }

        [Required]
        public DateTime? StartTime { get; set; }

        [Required]
        public DateTime? EndTime { get; set; }

        [Required]
        public CoordinatesDto? Coordinates { get; set; }

        [Required]
        public string? WaterBody { get; set; }

        [Required]
        public FishingMethods? FishingMethod { get; set; }

        public string? Bait { get; set; }

        public string? Note { get; set; }
    }
}
