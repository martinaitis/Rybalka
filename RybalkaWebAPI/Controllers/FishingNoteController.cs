using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RybalkaWebAPI.Data;
using RybalkaWebAPI.Models.Dto.FishingNote;
using RybalkaWebAPI.Models.Entity;
using RybalkaWebAPI.Services.WeatherForecast;

namespace RybalkaWebAPI.Controllers
{
    [Route("api/note")]
    [ApiController]
    public class FishingNoteController : ControllerBase
    {
        private const string GET_ROUTE_NAME = "GetNotes";

        private readonly ILogger<FishingNoteController> _logger;
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        private readonly WeatherForecastService _weatherForecastService;

        public FishingNoteController(
            ILogger<FishingNoteController> logger,
            ApplicationDbContext db,
            IMapper mapper,
            WeatherForecastService weatherForecastService)
        {
            _logger = logger;
            _db = db;
            _mapper = mapper;
            _weatherForecastService = weatherForecastService;
        }

        /// <remarks>
        /// Use default parameters to get all fishing notes.
        /// For filtering use only one parameter, parameters can not be combined.
        /// </remarks>
        [HttpGet(Name = GET_ROUTE_NAME)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<FishingNoteDto>> GetNotes(int id = 0, string user = "")
        {
            if (id != 0 && !user.IsNullOrEmpty())
            {
                return BadRequest("Use one filter parameter");
            }
            else if (id != 0)
            {
                return GetNoteById(id);
            }
            else if (!user.IsNullOrEmpty())
            {
                return GetNotesByUser(user);
            }
            else
            {
                return GetAllNotes();
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<FishingNoteDto>> PostNote([FromBody] FishingNoteDto noteDto)
        {
            if (noteDto == null)
            {
                var message = $"Request body does not contains {nameof(FishingNoteDto)}";
                _logger.LogWarning($"Action: {nameof(PostNote)} Message: {message}");
                return BadRequest(message);
            }
            else if (DateTime.Compare(noteDto.StartTime!, DateTime.Now.AddDays(-7)) < 0)
            {
                var message = $"Fishing date should not be older than 7 days";
                _logger.LogWarning($"Action: {nameof(PostNote)} " + $"Message: {message}");
                return BadRequest(message);
            }
            else
            {
                var forecast = await _weatherForecastService.GetHourWeatherForecast(
                    noteDto.Coordinates!.Latitude,
                    noteDto.Coordinates.Longitude,
                    noteDto.StartTime);
                if (forecast == null || forecast.Condition == null)
                {
                    _logger.LogWarning($"Action: {nameof(PostNote)} Message: {nameof(forecast)} == null");
                    return await CompleteFishingNotePost(noteDto);
                }

                noteDto.Temp = forecast.Temp;
                noteDto.WindKph = forecast.WindKph;
                noteDto.WindDir = forecast.WindDir;
                noteDto.CloudPct = forecast.CloudPct;
                noteDto.ConditionText = forecast.Condition.Text;

                return await CompleteFishingNotePost(noteDto);
            }
        }
        private async Task<ActionResult<FishingNoteDto>> CompleteFishingNotePost(FishingNoteDto noteDto)
        {
            FishingNote note = _mapper.Map<FishingNote>(noteDto);
            _db.FishingNotes.Add(note);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetNotes), new { id = note.Id }, noteDto);
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteNote(int id)
        {
            var note = _db.FishingNotes.FirstOrDefault(n => n.Id == id);
            if (note == null)
            {
                var message = $"Fishing note with id:{id} does not exist in DB";
                _logger.LogWarning($"Action: {nameof(DeleteNote)} Message: {message}");
                return NotFound(message);
            }
            else
            {
                _db.FishingNotes.Remove(note);
                await _db.SaveChangesAsync();
                return NoContent();
            }
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateNote(int id, [FromBody]FishingNoteDto noteDto)
        {
            if (noteDto == null)
            {
                var message = $"Request body does not contains {nameof(FishingNoteDto)}";
                _logger.LogWarning($"Action: {nameof(UpdateNote)} Message: {message}");
                return BadRequest(message);
            }

            var note = _db.FishingNotes.FirstOrDefault(n => n.Id == id);
            if (note == null)
            {
                var message = $"Fishing note with id:{id} does not exist in DB";
                _logger.LogWarning($"Action: {nameof(UpdateNote)} Message: {message}");
                return NotFound(message);
            }

            _mapper.Map(noteDto, note);
            _db.Update(note);
            await _db.SaveChangesAsync();

            return NoContent();
        }

        private ActionResult<IEnumerable<FishingNoteDto>> GetAllNotes()
        {
            var notes = _db.FishingNotes.AsNoTracking();
            if (notes.Any())
            {
                return Ok(_mapper.Map<IEnumerable<FishingNoteDto>>(notes));
            }

            _logger.LogWarning($"Action: {nameof(GetAllNotes)} Message: {nameof(notes)} table is empty");
            return NotFound();
        }

        private ActionResult<IEnumerable<FishingNoteDto>> GetNoteById(int id)
        {
            var note = _db.FishingNotes.AsNoTracking().FirstOrDefault(n => n.Id == id);
            if (note == null)
            {
                var message = $"Fishing note with id:{id} does not exist in DB";
                _logger.LogWarning($"Action: {nameof(GetNoteById)} Message: {message}");
                return NotFound(message);
            }

            return Ok(_mapper.Map<FishingNoteDto>(note));
        }

        private ActionResult<IEnumerable<FishingNoteDto>> GetNotesByUser(string user)
        {
            var notes = _db.FishingNotes.AsNoTracking().Where(n => n.User == user);
            if (notes.Any())
            {
                return Ok(_mapper.Map<IEnumerable<FishingNoteDto>>(notes));
            }

            _logger.LogWarning($"Action: {nameof(GetNotesByUser)} Message: notes by user:{user} does not exist in DB");
            return NoContent();
        }
    }
}
