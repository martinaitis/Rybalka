namespace Rybalka.Core.Dto.FishingNote
{
    public sealed class FishingNoteResponse
    {
        public int Id { get; set; }
        public string? User { get; set; }
        public string? Title { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public CoordinatesDto? Coordinates { get; set; }
        public string? WaterBody { get; set; }
        public int? FishingMethod { get; set; }
        public int? FishCount { get; set; }
        public string? Bait { get; set; }
        public string? Description { get; set; }
        public decimal? Temp { get; set; }
        public decimal? WindKph { get; set; }
        public string? WindDir { get; set; }
        public decimal? CloudPct { get; set; }
        public string? ConditionText { get; set; }
        public string? ImageFileName { get; set; }
    }
}
