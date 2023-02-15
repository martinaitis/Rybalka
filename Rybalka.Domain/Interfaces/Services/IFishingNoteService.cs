using Rybalka.Domain.Dto.FishingNote;

namespace Rybalka.Domain.Interfaces.Services
{
    public interface IFishingNoteService
    {
        List<FishingNoteDto> GetAllFishingNotes();
        FishingNoteDto? GetFishingNoteById(int id);
        List<FishingNoteDto> GetFishingNotesByUser(string user);
    }
}
