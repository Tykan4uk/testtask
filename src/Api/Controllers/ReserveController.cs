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
    public class ReserveController : ControllerBase
    {
        private readonly IReserveService _reserveService;

        public ReserveController(IReserveService reserveService)
        {
            _reserveService = reserveService;
        }

        /// <summary>
        /// Create new auditorium reserve.
        /// </summary>
        /// <param name="request">Information about reserve</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Information about created reserve</returns>
        [HttpPost("create-reserve")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuditoriumReserveModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Error))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Error))]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(Error))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Error))]
        public async Task<IActionResult> Create([FromBody] AuditoriumReserveModel request, CancellationToken cancellationToken)
        {
            var result = await _reserveService.CreateReserveAsync(request);

            return this.ToActionResult(result);
        }
    }
}