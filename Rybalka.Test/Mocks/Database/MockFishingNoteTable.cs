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
            }
        };
    }
}
