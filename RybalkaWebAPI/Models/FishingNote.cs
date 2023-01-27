using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RybalkaWebAPI.Models
{
    public class FishingNote
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public required string User { get; set; }
        public DateTime? FishingDate { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public required string WaterBody { get; set; }
        public string? FishingMethod { get; set; }
        public string? Bait { get; set; }
        public string? Note { get; set; }
    }
}
