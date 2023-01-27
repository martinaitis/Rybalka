using System.ComponentModel.DataAnnotations;

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
        public double Latitude { get; set; }

        [Required]
        public double Longitude { get; set; }

        [Required]
        public string? WaterBody { get; set; }

        [Required]
        public string? FishingMethod { get; set; }

        [Required]
        public string? Bait { get; set; }

        [Required]
        public string? Note { get; set; }
    }
}
