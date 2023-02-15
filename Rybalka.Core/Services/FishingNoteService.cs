using AutoMapper;
using Rybalka.Core.Dto.FishingNote;
using Rybalka.Core.Entities;
using Rybalka.Core.Interfaces.Clients;
using Rybalka.Core.Interfaces.Repositories;
using Rybalka.Core.Interfaces.Services;

namespace Rybalka.Core.Services
{
    public sealed class FishingNoteService : IFishingNoteService
    {
        private readonly IMapper _mapper;
        private readonly IFishingNoteRepository _fishingNoteRepository;
        private readonly IWeatherForecastClient _weatherForecastClient;
        public FishingNoteService(
            IMapper mapper,
            IFishingNoteRepository fishingNoteRepository,
            IWeatherForecastClient weatherForecastClient)
        {
            _fishingNoteRepository = fishingNoteRepository;
            _mapper = mapper;
            _weatherForecastClient = weatherForecastClient;
        }

        public async Task<List<FishingNoteDto>> GetAllFishingNotes()
        {
            var notes = await _fishingNoteRepository.GetFishingNotesReadOnly();

            return _mapper.Map<List<FishingNoteDto>>(notes);
        }

        public async Task<FishingNoteDto?> GetFishingNoteById(int id)
        {
            var note = await _fishingNoteRepository.GetFishingNoteByIdReadOnly(id);

            return _mapper.Map<FishingNoteDto>(note) ?? null;
        }

        public async Task<List<FishingNoteDto>> GetFishingNotesByUser(string user)
        {
            var notes = await _fishingNoteRepository.GetFishingNotesByUserReadOnly(user);

            return _mapper.Map<List<FishingNoteDto>>(notes);
        }

        public async Task<FishingNoteDto> CreateFishingNote(FishingNoteDto noteDto)
        {
            if (noteDto.Coordinates != null
                && noteDto.Coordinates.Latitude != null
                && noteDto.Coordinates.Longitude != null)
            {
                var forecast = await _weatherForecastClient.GetHourWeatherForecast(
                (double)noteDto.Coordinates.Latitude,
                (double)noteDto.Coordinates.Longitude,
                noteDto.StartTime);

                if (forecast != null)
                {
                    noteDto.Temp = forecast.Temp;
                    noteDto.WindKph = forecast.WindKph;
                    noteDto.WindDir = forecast.WindDir;
                    noteDto.CloudPct = forecast.CloudPct;
                    noteDto.ConditionText = forecast.Condition?.Text;
                }
            }

            var note = _mapper.Map<FishingNote>(noteDto);
            await _fishingNoteRepository.CreateFishingNote(note);

            return noteDto;
        }

        public async Task<bool> DeleteFishingNote(int id)
        {
            var note = await _fishingNoteRepository.GetFishingNoteById(id);
            if (note == null)
            {
                return false;
            }
            
            await _fishingNoteRepository.DeleteFishingNote(note);

            return true;
        }

        public async Task<bool> UpdateFishingNote(FishingNoteDto noteDto)
        {
            var note = await _fishingNoteRepository.GetFishingNoteById(noteDto.Id);
            if (note == null)
            {
                return false;
            }

            _mapper.Map(noteDto, note);
            await _fishingNoteRepository.UpdateFishingNote(note);

            return true;
        }
    }
}
