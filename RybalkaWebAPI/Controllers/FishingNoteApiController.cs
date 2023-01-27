using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RybalkaWebAPI.Data;
using RybalkaWebAPI.Models;
using RybalkaWebAPI.Models.Dto;

namespace RybalkaWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FishingNoteApiController : ControllerBase
    {
        private readonly ILogger<FishingNoteApiController> _logger;
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public FishingNoteApiController(
            ILogger<FishingNoteApiController> logger,
            ApplicationDbContext db,
            IMapper mapper)
        {
            _logger = logger;
            _db = db;
            _mapper = mapper;
        }

        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<FishingNoteDto>> GetAllNotes()
        {
            var notes = _db.FishingNotes.AsNoTracking();
            if (notes.Any())
            {
                return Ok(_mapper.Map<IEnumerable<FishingNoteDto>>(notes));
            }

            _logger.LogWarning($"Action: {nameof(GetAllNotes)} Message: Notes table is empty");
            return NotFound();
        }

        [HttpGet("id", Name = nameof(GetNoteById))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<FishingNoteDto>> GetNoteById(int id)
        {
            var note = _db.FishingNotes.AsNoTracking().FirstOrDefault(n => n.Id == id);
            if (note == null)
            {
                _logger.LogWarning($"Action: {nameof(GetNoteById)} Message: note with id:{id} does not exist in DB");
                return NotFound();
            }

            return Ok(_mapper.Map<FishingNoteDto>(note));
        }

        [HttpGet("user")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public ActionResult<IEnumerable<FishingNoteDto>> GetNotesByUser(string user)
        {
            var notes = _db.FishingNotes.AsNoTracking().Where(n => n.User == user);
            if (notes.Any())
            {
                return Ok(_mapper.Map<IEnumerable<FishingNoteDto>>(notes));
            }

            _logger.LogWarning($"Action: {nameof(GetNotesByUser)} Message: notes by user:{user} does not exist in DB");
            return NoContent();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<FishingNoteDto>> PostNote([FromBody] FishingNoteDto noteDto)
        {
            if (noteDto == null)
            {
                _logger.LogWarning($"Action: {nameof(PostNote)} Message: {nameof(noteDto)} == null");
                return BadRequest(noteDto);
            }
            else if (DateTime.Compare((DateTime)noteDto.FishingDate!, DateTime.Now.AddDays(-7)) < 0)
            {
                _logger.LogWarning($"Action: {nameof(PostNote)} " +
                    $"Message: Too late to create a note for fishind date - {noteDto.FishingDate}");
                return BadRequest("Fishing date should not be older than 7 days.");
            }
            else
            {
                FishingNote note = _mapper.Map<FishingNote>(noteDto);

                _db.FishingNotes.Add(note);
                await _db.SaveChangesAsync();

                return CreatedAtRoute(nameof(GetNoteById), new { id = note.Id });
            }
        }

        [HttpDelete("id")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteNote(int id)
        {
            var note = _db.FishingNotes.FirstOrDefault(n => n.Id == id);
            if (note == null)
            {
                _logger.LogWarning($"Action: {nameof(DeleteNote)} Message: note with id:{id} does not exist in DB");
                return NotFound();
            }
            else
            {
                _db.FishingNotes.Remove(note);
                await _db.SaveChangesAsync();
                return NoContent();
            }
        }

        [HttpPut("id")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateNote(int id, [FromBody]FishingNoteDto noteDto)
        {
            if (noteDto == null)
            {
                _logger.LogWarning($"Action: {nameof(UpdateNote)} Message: {nameof(noteDto)} == null");
                return BadRequest(noteDto);
            }

            var note = _db.FishingNotes.FirstOrDefault(n => n.Id == id);
            if (note == null)
            {
                _logger.LogWarning($"Action: {nameof(UpdateNote)} Message: note with id:{id} does not exist in DB");
                return NotFound();
            }

            _mapper.Map(noteDto, note);
            _db.Update(note);
            await _db.SaveChangesAsync();

            return NoContent();
        }
    }
}
