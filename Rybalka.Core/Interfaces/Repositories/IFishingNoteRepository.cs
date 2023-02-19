using Rybalka.Core.Entities;

namespace Rybalka.Core.Interfaces.Repositories
{
    public interface IFishingNoteRepository
    {
        Task<List<FishingNote>> GetFishingNotesReadOnly(CancellationToken ct);
        Task<FishingNote?> GetFishingNoteByIdReadOnly(int id, CancellationToken ct);
        Task<FishingNote?> GetFishingNoteById(int id, CancellationToken ct);
        Task<List<FishingNote>> GetFishingNotesByUserReadOnly(string user, CancellationToken ct);
        Task CreateFishingNote(FishingNote note, CancellationToken ct);
        Task DeleteFishingNote(FishingNote note, CancellationToken ct);
        Task UpdateFishingNote(FishingNote note, CancellationToken ct);
    }
}
