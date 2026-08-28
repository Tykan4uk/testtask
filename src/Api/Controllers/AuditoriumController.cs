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
        public async Task<IActionResult> Create([FromBody] AuditoriumModel request, CancellationToken cancellationToken)
        {
            var result = await _auditoriumService.CreateAuditoriumAsync(request);

            return this.ToActionResult(result);
        }
    }
}