using Rybalka.Application.Dto.FishingNote;
using Rybalka.Domain.Interfaces.Repositories;
using Rybalka.Domain.Interfaces.Services;

namespace Rybalka.Application.Services
{
    public sealed class FishingNoteService : IFishingNoteService
    {
        private readonly IMapper _mapper;
        private readonly IFishingNoteRepository _fishingNoteRepository;
        public FishingNoteService(
            IMapper mapper,
            IFishingNoteRepository fishingNoteRepository)
        {
            _fishingNoteRepository= fishingNoteRepository;
            _mapper = mapper;
        }

        public List<FishingNoteDto> GetAllFishingNotes()
        {
            var notes = _fishingNoteRepository.GetFishingNotes().ToList();

            return _mapper.Map<List<FishingNoteDto>>(notes);
        }

        public FishingNoteDto? GetFishingNoteById(int id)
        {
            var note = _fishingNoteRepository.GetFishingNotes().FirstOrDefault(n => n.Id == id);
            
            return _mapper.Map<FishingNoteDto>(note) ?? null;
        }

        public List<FishingNoteDto> GetFishingNotesByUser(string user)
        {
            var notes = _fishingNoteRepository.GetFishingNotes().Where(n => n.User == user).ToList();
            
            return _mapper.Map<List<FishingNoteDto>>(notes);
        }
    }
}
