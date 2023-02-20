using Rybalka.Core.Dto.FishingNote;

namespace Rybalka.Core.Interfaces.Services
{
    public interface IFishingNoteService
    {
        Task<List<FishingNoteDto>> GetAllFishingNotes(CancellationToken ct);
        Task<List<FishingNoteResponse>> GetAllFishingNotesV2(CancellationToken ct);
        Task<FishingNoteDto?> GetFishingNoteById(int id, CancellationToken ct);
        Task<FishingNoteResponse?> GetFishingNoteByIdV2(int id, CancellationToken ct);
        Task<List<FishingNoteDto>> GetFishingNotesByUser(string user, CancellationToken ct);
        Task<List<FishingNoteResponse>> GetFishingNotesByUserV2(string user, CancellationToken ct);
        Task<FishingNoteDto> CreateFishingNote(FishingNoteDto noteDto, CancellationToken ct);
        Task<FishingNoteResponse> CreateFishingNoteV2(FishingNoteRequest noteRequest, CancellationToken ct);
        Task<bool> DeleteFishingNote(int id, CancellationToken ct);
        Task<bool> UpdateFishingNote(FishingNoteDto noteDto, CancellationToken ct);
        Task<bool> UpdateFishingNoteV2(FishingNoteRequest noteRequest, int id, CancellationToken ct);
    }
}
