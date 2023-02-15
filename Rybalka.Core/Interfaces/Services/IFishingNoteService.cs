using Rybalka.Core.Dto.FishingNote;

namespace Rybalka.Core.Interfaces.Services
{
    public interface IFishingNoteService
    {
        Task<List<FishingNoteDto>> GetAllFishingNotes();
        Task<FishingNoteDto?> GetFishingNoteById(int id);
        Task<List<FishingNoteDto>> GetFishingNotesByUser(string user);
        Task<FishingNoteDto> CreateFishingNote(FishingNoteDto noteDto);
        Task<bool> DeleteFishingNote(int id);
        Task<bool> UpdateFishingNote(FishingNoteDto noteDto);
    }
}
