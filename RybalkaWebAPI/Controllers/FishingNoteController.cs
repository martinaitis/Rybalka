using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Rybalka.Core.Dto.FishingNote;
using Rybalka.Core.Interfaces.Services;
using RybalkaWebAPI.Attributes;

namespace RybalkaWebAPI.Controllers
{
    [ServiceFilter(typeof(LogAttribute))]
    [Route("api/note")]
    [ApiController]
    public class FishingNoteController : ControllerBase
    {
        private const string GET_ROUTE_NAME = "GetNotes";

        private readonly IFishingNoteService _fishingNoteService;

        public FishingNoteController(IFishingNoteService fishingNoteService)
        {
            _fishingNoteService = fishingNoteService;
        }

        /// <remarks>
        /// Use default parameters to get all fishing notes.
        /// For filtering use only one parameter, parameters can not be combined.
        /// </remarks>
        [HttpGet(Name = GET_ROUTE_NAME)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<FishingNoteDto>>> GetNotes(
            CancellationToken ct,
            int id = 0,
            string user = "")
        {
            if (id != 0 && !user.IsNullOrEmpty())
            {
                return BadRequest("Use one filter parameter");
            }
            else if (id != 0)
            {
                return await GetNoteById(id, ct);
            }
            else if (!user.IsNullOrEmpty())
            {
                return await GetNotesByUser(user, ct);
            }
            else
            {
                return await GetAllNotes(ct);
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<FishingNoteDto>> PostNote(
            [FromBody] FishingNoteDto noteDto,
            CancellationToken ct)
        {
            if (noteDto == null)
            {
                return BadRequest($"Request body does not contains {nameof(FishingNoteDto)}");
            }
            else
            {
                var createdNoteDto = await _fishingNoteService.CreateFishingNote(noteDto, ct);
                return CreatedAtAction(nameof(GetNotes), new { id = createdNoteDto.Id }, createdNoteDto);
            }
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteNote(int id, CancellationToken ct)
        {
            if (await _fishingNoteService.DeleteFishingNote(id, ct))
            {
                return NoContent();
            }

            return NotFound($"Fishing note with id:{id} does not exist in DB");
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateNote([FromBody] FishingNoteDto noteDto, CancellationToken ct)
        {
            if (noteDto == null)
            {
                return BadRequest($"Request body does not contains {nameof(FishingNoteDto)}");
            }

            if (await _fishingNoteService.UpdateFishingNote(noteDto, ct))
            {
                return NoContent();
            }

            return NotFound($"Fishing note with id:{noteDto.Id} does not exist in DB");
        }

        private async Task<ActionResult<List<FishingNoteDto>>> GetAllNotes(CancellationToken ct)
        {
            var notes = await _fishingNoteService.GetAllFishingNotes(ct);
            if (notes.Any())
            {
                return Ok(notes.OrderByDescending(n => n.StartTime));
            }

            return NoContent();
        }

        private async Task<ActionResult<List<FishingNoteDto>>> GetNoteById(int id, CancellationToken ct)
        {
            var note = await _fishingNoteService.GetFishingNoteById(id, ct);
            if (note == null)
            {
                return NotFound($"Fishing note with id:{id} does not exist in DB");
            }

            return Ok(note);
        }

        private async Task<ActionResult<List<FishingNoteDto>>> GetNotesByUser(string user, CancellationToken ct)
        {
            var notes = await _fishingNoteService.GetFishingNotesByUser(user, ct);
            if (notes.Any())
            {
                return Ok(notes.OrderByDescending(n => n.StartTime));
            }

            return NoContent();
        }
    }
}
