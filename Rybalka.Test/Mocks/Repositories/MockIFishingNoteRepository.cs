using Moq;
using Rybalka.Core.Interfaces.Repositories;
using Rybalka.Test.Mocks.Database;

namespace Rybalka.Test.Mocks.Repositories
{
    public sealed class MockIFishingNoteRepository
    {
        public static Mock<IFishingNoteRepository> GetMock()
        {
            var mock = new Mock<IFishingNoteRepository>();
            
            mock.Setup(m => m.GetFishingNotesReadOnly(CancellationToken.None))
                .ReturnsAsync(() => MockFishingNoteTable.fishingNotes);

            return mock;
        }
    }
}
