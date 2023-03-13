using Rybalka.Core.Dto.FishingNote;

namespace Rybalka.Core.Interfaces.Services
{
    public interface IFishingNoteService
    {
        Task<List<FishingNoteResponse>> GetAllFishingNotes(CancellationToken ct);
        Task<FishingNoteResponse?> GetFishingNoteById(int id, CancellationToken ct);
        Task<List<FishingNoteResponse>> GetFishingNotesByUserId(int userId, CancellationToken ct);
        Task<FishingNoteResponse> CreateFishingNote(FishingNoteRequest noteRequest, CancellationToken ct);
        Task<bool> DeleteFishingNote(int id, CancellationToken ct);
        Task<bool> UpdateFishingNote(FishingNoteRequest noteRequest, int id, CancellationToken ct);
    }
}
