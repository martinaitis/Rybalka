using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Rybalka.Core.Dto.FishingNote;
using Rybalka.Core.Interfaces.Services;
using Rybalka.Infrastructure.Data;
using RybalkaWebAPI.Attributes;

namespace RybalkaWebAPI.Controllers
{
    [ServiceFilter(typeof(LogAttribute))]
    [Route("api/note")]
    [ApiController]
    public class FishingNoteController : ControllerBase
    {
        private const string GET_ROUTE_NAME = "GetNotes";

        private readonly ILogger<FishingNoteController> _logger;
        private readonly ApplicationDbContext _db;
        private readonly IFishingNoteService _fishingNoteService;

        public FishingNoteController(
            ILogger<FishingNoteController> logger,
            ApplicationDbContext db,
            IFishingNoteService fishingNoteService)
        {
            _logger = logger;
            _db = db;
            _fishingNoteService = fishingNoteService;
        }

        /// <remarks>
        /// Use default parameters to get all fishing notes.
        /// For filtering use only one parameter, parameters can not be combined.
        /// </remarks>
        [HttpGet(Name = GET_ROUTE_NAME)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<FishingNoteDto>>> GetNotes(int id = 0, string user = "")
        {
            if (id != 0 && !user.IsNullOrEmpty())
            {
                return BadRequest("Use one filter parameter");
            }
            else if (id != 0)
            {
                return await GetNoteById(id);
            }
            else if (!user.IsNullOrEmpty())
            {
                return await GetNotesByUser(user);
            }
            else
            {
                return await GetAllNotes();
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<FishingNoteDto>> PostNote([FromBody] FishingNoteDto noteDto)
        {
            if (noteDto == null)
            {
                return BadRequest($"Request body does not contains {nameof(FishingNoteDto)}");
            }
            else
            {
                var createdNoteDto = await _fishingNoteService.CreateFishingNote(noteDto);
                return CreatedAtAction(nameof(GetNotes), new { id = createdNoteDto.Id }, createdNoteDto);
            }
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteNote(int id)
        {
            if (await _fishingNoteService.DeleteFishingNote(id))
            {
                return NoContent();
            }

            return NotFound($"Fishing note with id:{id} does not exist in DB");
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateNote([FromBody] FishingNoteDto noteDto)
        {
            if (noteDto == null)
            {
                return BadRequest($"Request body does not contains {nameof(FishingNoteDto)}");
            }

            if (await _fishingNoteService.UpdateFishingNote(noteDto))
            {
                return NoContent();
            }

            return NotFound($"Fishing note with id:{noteDto.Id} does not exist in DB");
        }

        private async Task<ActionResult<List<FishingNoteDto>>> GetAllNotes()
        {
            var notes = await _fishingNoteService.GetAllFishingNotes();
            if (notes.Any())
            {
                return Ok(notes.OrderByDescending(n => n.StartTime));
            }

            return NoContent();
        }

        private async Task<ActionResult<List<FishingNoteDto>>> GetNoteById(int id)
        {
            var note = await _fishingNoteService.GetFishingNoteById(id);
            if (note == null)
            {
                return NotFound($"Fishing note with id:{id} does not exist in DB");
            }

            return Ok(note);
        }

        private async Task<ActionResult<List<FishingNoteDto>>> GetNotesByUser(string user)
        {
            var notes = await _fishingNoteService.GetFishingNotesByUser(user);
            if (notes.Any())
            {
                return Ok(notes.OrderByDescending(n => n.StartTime));
            }

            return NoContent();
        }
    }
}
