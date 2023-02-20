using AutoMapper;
using Microsoft.AspNetCore.Http;
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

        public async Task<List<FishingNoteDto>> GetAllFishingNotes(CancellationToken ct)
        {
            var notes = await _fishingNoteRepository.GetFishingNotesReadOnly(ct);

            return _mapper.Map<List<FishingNoteDto>>(notes);
        }

        public async Task<List<FishingNoteResponse>> GetAllFishingNotesV2(CancellationToken ct)
        {
            var notes = await _fishingNoteRepository.GetFishingNotesReadOnly(ct);

            return _mapper.Map<List<FishingNoteResponse>>(notes);
        }

        public async Task<FishingNoteDto?> GetFishingNoteById(int id, CancellationToken ct)
        {
            var note = await _fishingNoteRepository.GetFishingNoteByIdReadOnly(id, ct);

            return _mapper.Map<FishingNoteDto>(note) ?? null;
        }

        public async Task<FishingNoteResponse?> GetFishingNoteByIdV2(int id, CancellationToken ct)
        {
            var note = await _fishingNoteRepository.GetFishingNoteByIdReadOnly(id, ct);

            return _mapper.Map<FishingNoteResponse>(note) ?? null;
        }

        public async Task<List<FishingNoteDto>> GetFishingNotesByUser(string user, CancellationToken ct)
        {
            var notes = await _fishingNoteRepository.GetFishingNotesByUserReadOnly(user, ct);

            return _mapper.Map<List<FishingNoteDto>>(notes);
        }

        public async Task<List<FishingNoteResponse>> GetFishingNotesByUserV2(string user, CancellationToken ct)
        {
            var notes = await _fishingNoteRepository.GetFishingNotesByUserReadOnly(user, ct);

            return _mapper.Map<List<FishingNoteResponse>>(notes);
        }

        public async Task<FishingNoteDto> CreateFishingNote(FishingNoteDto noteDto, CancellationToken ct)
        {
            if (noteDto.Coordinates != null
                && noteDto.Coordinates.Latitude != null
                && noteDto.Coordinates.Longitude != null)
            {
                var forecast = await _weatherForecastClient.GetHourWeatherForecast(
                (double)noteDto.Coordinates.Latitude,
                (double)noteDto.Coordinates.Longitude,
                noteDto.StartTime,
                ct);

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

            await _fishingNoteRepository.CreateFishingNote(note, ct);

            return noteDto;
        }

        public async Task<FishingNoteResponse> CreateFishingNoteV2(
            FishingNoteRequest noteRequest,
            CancellationToken ct)
        {
            if (noteRequest.Coordinates != null
                && noteRequest.Coordinates.Latitude != null
                && noteRequest.Coordinates.Longitude != null)
            {
                var forecast = await _weatherForecastClient.GetHourWeatherForecast(
                (double)noteRequest.Coordinates.Latitude,
                (double)noteRequest.Coordinates.Longitude,
                noteRequest.StartTime,
                ct);

                if (forecast != null)
                {
                    noteRequest.Temp = forecast.Temp;
                    noteRequest.WindKph = forecast.WindKph;
                    noteRequest.WindDir = forecast.WindDir;
                    noteRequest.CloudPct = forecast.CloudPct;
                    noteRequest.ConditionText = forecast.Condition?.Text;
                }
            }

            var note = _mapper.Map<FishingNote>(noteRequest);
            note.ImageFileName = await UploadImage(noteRequest.Image, ct);

            await _fishingNoteRepository.CreateFishingNote(note, ct);

            return _mapper.Map<FishingNoteResponse>(note);
        }

        private static async Task<string?> UploadImage(IFormFile? image, CancellationToken ct)
        {
            string? uniqueImageName = null;
            if (image != null)
            {
                uniqueImageName = Guid.NewGuid().ToString() + "_" + image.FileName;
                await image.CopyToAsync(
                    new FileStream(
                        Path.Combine(
                            Directory.GetCurrentDirectory(),
                            "wwwroot/images/",
                            uniqueImageName),
                        FileMode.Create),
                    ct);
            }

            return uniqueImageName;
        }

        public async Task<bool> DeleteFishingNote(int id, CancellationToken ct)
        {
            var note = await _fishingNoteRepository.GetFishingNoteById(id, ct);
            if (note == null)
            {
                return false;
            }
            
            await _fishingNoteRepository.DeleteFishingNote(note, ct);

            return true;
        }

        public async Task<bool> UpdateFishingNote(FishingNoteDto noteDto, CancellationToken ct)
        {
            var note = await _fishingNoteRepository.GetFishingNoteById(noteDto.Id, ct);
            if (note == null)
            {
                return false;
            }

            _mapper.Map(noteDto, note);
            await _fishingNoteRepository.UpdateFishingNote(note, ct);

            return true;
        }

        public async Task<bool> UpdateFishingNoteV2(
            FishingNoteRequest noteRequest,
            int id,
            CancellationToken ct)
        {
            var note = await _fishingNoteRepository.GetFishingNoteById(id, ct);
            if (note == null)
            {
                return false;
            }

            _mapper.Map(noteRequest, note);
            await _fishingNoteRepository.UpdateFishingNote(note, ct);

            return true;
        }
    }
}
