using Rybalka.Core.Entities;

namespace Rybalka.Core.Interfaces.Repositories
{
    public interface IFishingNoteRepository
    {
        Task<List<FishingNote>> GetFishingNotesReadOnly();
        Task<FishingNote?> GetFishingNoteByIdReadOnly(int id);
        Task<FishingNote?> GetFishingNoteById(int id);
        Task<List<FishingNote>> GetFishingNotesByUserReadOnly(string user);
        Task CreateFishingNote(FishingNote note);
        Task DeleteFishingNote(FishingNote note);
        Task UpdateFishingNote(FishingNote note);
    }
}
