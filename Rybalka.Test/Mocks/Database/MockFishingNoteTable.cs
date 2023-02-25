using Rybalka.Core.Entities;
using static Rybalka.Core.AppEnums;

namespace Rybalka.Test.Mocks.Database
{
    public static class MockFishingNoteTable
    {
        public static readonly List<FishingNote> fishingNotes = new()
        {
            new FishingNote
            {
                Id = 1,
                User = "Karolis",
                Title = "Pavadinimas",
                StartTime = new DateTime(2023, 2, 20, 10, 0, 0),
                EndTime = new DateTime(2023, 2, 20, 12, 0, 0),
                Latitude = 24,
                Longitude = 52,
                WaterBody = "Neris",
                FishingMethod = FishingMethod.Spinning,
                Bait = "Vobleris",
                FishCount = 0,
                Description = "Aukštyn upe.",
                Temp = -2,
                WindMps = 3,
                WindDir = "N",
                CloudPct = 10,
                ConditionText = "Sunny",
                ImageFileName = "https://sheikasop-001-site1.atempurl.com/images/test.jpg"
            },
            new FishingNote
            {
                Id = 2,
                User = "Paulius",
                Title = "Pavadinimas2",
                StartTime = new DateTime(2023, 1, 20, 10, 0, 0),
                EndTime = new DateTime(2023, 1, 20, 12, 0, 0),
                Latitude = 24,
                Longitude = 52,
                WaterBody = "Merkys",
                FishingMethod = FishingMethod.Bottom,
                Bait = "Sliekas",
                FishCount = 1,
                Description = "Daug alaus",
                Temp = 5,
                WindMps = 6,
                WindDir = "S",
                CloudPct = 80,
                ConditionText = "Cloudy",
                ImageFileName = "https://sheikasop-001-site1.atempurl.com/images/test2.jpg"
            }
        };
    }
}
