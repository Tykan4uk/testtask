using Api.Extensions;
using Application.Common;
using Application.Dtos;
using Application.Interfaces;
using Application.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/reports")]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        /// <summary>
        /// Get list of reserves from time to time and total price for that period
        /// </summary>
        /// <param name="request">Perion from time to time</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of reserves from time to time and total price for that period</returns>
        [HttpGet("reserves-report")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuditoriumReserveReportDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Error))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Error))]
        public async Task<IActionResult> Login([FromQuery] AuditoriumReserveReportModel request, CancellationToken cancellationToken)
        {
            var result = await _reportService.GetReserveReportAsync(request);

            return this.ToActionResult(result);
        }
    }
}
