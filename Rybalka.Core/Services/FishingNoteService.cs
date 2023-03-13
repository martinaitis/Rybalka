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

        public async Task<List<FishingNoteResponse>> GetAllFishingNotes(CancellationToken ct)
        {
            var notes = await _fishingNoteRepository.GetFishingNotesReadOnly(ct);

            return _mapper.Map<List<FishingNoteResponse>>(notes);
        }

        public async Task<FishingNoteResponse?> GetFishingNoteById(int id, CancellationToken ct)
        {
            var note = await _fishingNoteRepository.GetFishingNoteByIdReadOnly(id, ct);

            return _mapper.Map<FishingNoteResponse>(note) ?? null;
        }

        public async Task<List<FishingNoteResponse>> GetFishingNotesByUserId(int userId, CancellationToken ct)
        {
            var notes = await _fishingNoteRepository.GetFishingNotesByUserIdReadOnly(userId, ct);

            return _mapper.Map<List<FishingNoteResponse>>(notes);
        }

        public async Task<FishingNoteResponse> CreateFishingNote(
            FishingNoteRequest noteRequest,
            CancellationToken ct)
        {
            var forecastTime = noteRequest.StartTime.AddHours(
                    (noteRequest.EndTime - noteRequest.StartTime).TotalHours / 2);
            var forecast = await _weatherForecastClient.GetHourWeatherForecast(
                noteRequest.Coordinates.Latitude,
                noteRequest.Coordinates.Longitude,
                forecastTime,
                ct);

            if (forecast != null)
            {
                noteRequest.Temp = forecast.Temp;
                if (forecast.WindKph != null)
                {
                    noteRequest.WindMps = decimal.Divide((decimal)forecast.WindKph, (decimal)3.6);
                }
                
                noteRequest.WindDir = forecast.WindDir;
                noteRequest.CloudPct = forecast.CloudPct;
                noteRequest.ConditionText = forecast.Condition?.Text;
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

        public async Task<bool> UpdateFishingNote(
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
