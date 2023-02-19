using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Rybalka.Core.AppEnums;

namespace Rybalka.Core.Entities
{
    public class FishingNote
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public required string User { get; set; }
        public string? Title { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public required string WaterBody { get; set; }
        public FishingMethod? FishingMethod { get; set; }
        public string? Bait { get; set; }
        public int? FishCount { get; set; }
        public string? Description { get; set; }

        [Precision(6, 2)]
        public decimal? Temp { get; set; }

        [Precision(6, 2)]
        public decimal? WindKph { get; set; }
        public string? WindDir { get; set; }

        [Precision(6, 2)]
        public decimal? CloudPct { get; set; }
        public string? ConditionText { get; set; }
    }
}
