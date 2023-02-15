using Rybalka.Domain.Entities;

namespace Rybalka.Domain.Interfaces.Repositories
{
    public interface IFishingNoteRepository
    {
        IQueryable<FishingNote> GetFishingNotes();
    }
}
