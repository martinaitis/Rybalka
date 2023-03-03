using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Rybalka.Core.Dto.FishingNote;
using Rybalka.Core.Interfaces.Services;
using RybalkaWebAPI.Attributes.Action;

namespace RybalkaWebAPI.Controllers
{
    [ServiceFilter(typeof(LogAttribute))]
    [Route("api/note")]
    [ApiController]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
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
        [HttpGet(Name = GET_ROUTE_NAME), MapToApiVersion("1.0")]
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

        /// <remarks>
        /// Use default parameters to get all fishing notes.
        /// For filtering use only one parameter, parameters can not be combined.
        /// </remarks>
        [HttpGet(Name = GET_ROUTE_NAME), MapToApiVersion("2.0"), Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<FishingNoteResponse>>> GetNotesV2(
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
                return await GetNoteByIdV2(id, ct);
            }
            else if (!user.IsNullOrEmpty())
            {
                return await GetNotesByUserV2(user, ct);
            }
            else
            {
                return await GetAllNotesV2(ct);
            }
        }

        [HttpPost, MapToApiVersion("1.0")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<FishingNoteDto>> PostNote(
            [FromBody] FishingNoteDto note,
            CancellationToken ct)
        {
            if (note == null)
            {
                return BadRequest($"Request body does not contains {nameof(FishingNoteDto)}");
            }
            else
            {
                var createdNote = await _fishingNoteService.CreateFishingNote(note, ct);
                return CreatedAtAction(nameof(GetNotes), new { id = createdNote.Id }, createdNote);
            }
        }

        [HttpPost, MapToApiVersion("2.0"), Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<FishingNoteResponse>> PostNoteV2(
            [FromForm] FishingNoteRequest noteRequest,
            CancellationToken ct)
        {
            if (noteRequest == null)
            {
                return BadRequest($"Request body does not contains {nameof(FishingNoteDto)}");
            }

            else
            {
                var noteResponse = await _fishingNoteService.CreateFishingNoteV2(noteRequest, ct);
                return CreatedAtAction(nameof(GetNotes), new { id = noteResponse.Id }, noteResponse);
            }
        }

        [HttpDelete, MapToApiVersion("1.0")]
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

        [HttpDelete, MapToApiVersion("2.0"), Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteNoteV2(int id, CancellationToken ct)
        {
            if (await _fishingNoteService.DeleteFishingNote(id, ct))
            {
                return NoContent();
            }

            return NotFound($"Fishing note with id:{id} does not exist in DB");
        }

        [HttpPut, MapToApiVersion("1.0")]
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

        [HttpPut, MapToApiVersion("2.0"), Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateNoteV2(
            [FromBody] FishingNoteRequest noteRequest,
            int id,
            CancellationToken ct)
        {
            if (noteRequest == null)
            {
                return BadRequest($"Request body does not contains {nameof(FishingNoteDto)}");
            }

            if (await _fishingNoteService.UpdateFishingNoteV2(noteRequest, id, ct))
            {
                return NoContent();
            }

            return NotFound($"Fishing note with id:{id} does not exist in DB");
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

        private async Task<ActionResult<List<FishingNoteResponse>>> GetAllNotesV2(CancellationToken ct)
        {
            var notes = await _fishingNoteService.GetAllFishingNotesV2(ct);
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

        private async Task<ActionResult<List<FishingNoteResponse>>> GetNoteByIdV2(int id, CancellationToken ct)
        {
            var note = await _fishingNoteService.GetFishingNoteByIdV2(id, ct);
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

        private async Task<ActionResult<List<FishingNoteResponse>>> GetNotesByUserV2(
            string user,
            CancellationToken ct)
        {
            var notes = await _fishingNoteService.GetFishingNotesByUserV2(user, ct);
            if (notes.Any())
            {
                return Ok(notes.OrderByDescending(n => n.StartTime));
            }

            return NoContent();
        }
    }
}
