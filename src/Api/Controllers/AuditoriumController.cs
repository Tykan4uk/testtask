using Api.Extensions;
using Application.Common;
using Application.Dtos;
using Application.Interfaces;
using Application.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuditoriumController : ControllerBase
    {
        private readonly IAuditoriumService _auditoriumService;

        public AuditoriumController(IAuditoriumService auditoriumService)
        {
            _auditoriumService = auditoriumService;
        }

        /// <summary>
        /// Create new auditorium.
        /// </summary>
        /// <param name="request">Information about auditorium</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Information about created auditorium</returns>
        [HttpPost("create-auditorium")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuditoriumDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Error))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Error))]
        public async Task<IActionResult> Create([FromBody] AuditoriumModel request, CancellationToken cancellationToken)
        {
            var result = await _auditoriumService.CreateAuditoriumAsync(request);

            return this.ToActionResult(result);
        }

        /// <summary>
        /// Update auditorium info.
        /// </summary>
        /// <param name="request">New information about auditorium</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Information about updated auditorium</returns>
        [HttpPut("update-auditorium")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuditoriumDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Error))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Error))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Error))]
        public async Task<IActionResult> Update([FromBody] AuditoriumModel request, CancellationToken cancellationToken)
        {
            var result = await _auditoriumService.UpdateAuditoriumAsync(request);

            return this.ToActionResult(result);
        }

        /// <summary>
        /// Remove auditorium.
        /// </summary>
        /// <param name="auditoriumId">Id auditorium that will removed</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Status of operation</returns>
        [HttpDelete("delete-auditorium/{auditoriumId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Error))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Error))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Error))]
        public async Task<IActionResult> Remove([FromRoute] Guid auditoriumId, CancellationToken cancellationToken)
        {
            var result = await _auditoriumService.RemoveAuditoriumAsync(auditoriumId);

            return this.ToActionResult(result);
        }

        /// <summary>
        /// Get free auditoriums.
        /// </summary>
        /// <param name="model">Requested auditorium</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of free auditoriums</returns>
        [HttpGet("get-free-auditoriums")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<AuditoriumDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Error))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Error))]
        public async Task<IActionResult> GetFreeAuditoriums([FromQuery] AuditoriumSearchFreeModel model, CancellationToken cancellationToken)
        {
            var result = await _auditoriumService.SearchFreeAsync(model);

            return this.ToActionResult(result);
        }
    }
}