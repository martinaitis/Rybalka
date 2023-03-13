using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rybalka.Core.Dto.FishingNote;
using Rybalka.Core.Interfaces.Services;
using RybalkaWebAPI.Attributes.Action;

namespace RybalkaWebAPI.Controllers
{
    [ServiceFilter(typeof(LogAttribute))]
    [Route("api/note")]
    [ApiController]
    public class FishingNoteController : ControllerBase
    {
        private const string GET_ROUTE_NAME = "GetNotes";
        private const int MAX_IMAGE_SIZE_BYTE = 10 * 1024 * 1024; //10MB

        private readonly IFishingNoteService _fishingNoteService;

        public FishingNoteController(IFishingNoteService fishingNoteService)
        {
            _fishingNoteService = fishingNoteService;
        }

        /// <remarks>
        /// Use default parameters to get all fishing notes.
        /// For filtering use only one parameter, parameters can not be combined.
        /// </remarks>
        [HttpGet(Name = GET_ROUTE_NAME), Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<FishingNoteResponse>>> GetNotes(
            CancellationToken ct,
            int id = 0,
            int userId = 0)
        {
            if (id != 0 && userId != 0)
            {
                return BadRequest("Use one filter parameter");
            }
            else if (id != 0)
            {
                return await GetNoteById(id, ct);
            }
            else if (userId != 0)
            {
                return await GetNotesByUserId(userId, ct);
            }
            else
            {
                return await GetAllNotes(ct);
            }
        }

        [HttpPost, Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<FishingNoteResponse>> PostNote(
            [FromForm] FishingNoteRequest noteRequest,
            CancellationToken ct)
        {
            if (noteRequest == null)
            {
                return BadRequest($"Request body does not contains {nameof(FishingNoteResponse)}");
            }

            else
            {
                var noteResponse = await _fishingNoteService.CreateFishingNote(noteRequest, ct);
                return CreatedAtAction(nameof(GetNotes), new { id = noteResponse.Id }, noteResponse);
            }
        }

        [HttpDelete, Authorize(Roles = "User")]
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

        [HttpPut, Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateNote(
            [FromBody] FishingNoteRequest noteRequest,
            int id,
            CancellationToken ct)
        {
            if (noteRequest == null)
            {
                return BadRequest($"Request body does not contains {nameof(FishingNoteResponse)}");
            }

            if (await _fishingNoteService.UpdateFishingNote(noteRequest, id, ct))
            {
                return NoContent();
            }

            return NotFound($"Fishing note with id:{id} does not exist in DB");
        }

        private async Task<ActionResult<List<FishingNoteResponse>>> GetAllNotes(CancellationToken ct)
        {
            var notes = await _fishingNoteService.GetAllFishingNotes(ct);
            if (notes.Any())
            {
                return Ok(notes.OrderByDescending(n => n.StartTime));
            }

            return NoContent();
        }

        private async Task<ActionResult<List<FishingNoteResponse>>> GetNoteById(int id, CancellationToken ct)
        {
            var note = await _fishingNoteService.GetFishingNoteById(id, ct);
            if (note == null)
            {
                return NotFound($"Fishing note with id:{id} does not exist in DB");
            }

            return Ok(note);
        }

        private async Task<ActionResult<List<FishingNoteResponse>>> GetNotesByUserId(
            int userId,
            CancellationToken ct)
        {
            var notes = await _fishingNoteService.GetFishingNotesByUserId(userId, ct);
            if (notes.Any())
            {
                return Ok(notes.OrderByDescending(n => n.StartTime));
            }

            return NoContent();
        }
    }
}
