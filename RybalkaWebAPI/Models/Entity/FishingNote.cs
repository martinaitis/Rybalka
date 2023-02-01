using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static RybalkaWebAPI.Models.AppEnums;

namespace RybalkaWebAPI.Models.Entity
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
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public required string WaterBody { get; set; }
        public FishingMethods? FishingMethod { get; set; }
        public string? Bait { get; set; }
        public decimal FishCount { get; set; }
        public string? Note { get; set; }
        public decimal? Temp { get; set; }
        public decimal? WindKph { get; set; }
        public string? WindDir { get; set; }
        public decimal? CloudPct { get; set; }
        public string? ConditionText { get; set; }
    }
}
