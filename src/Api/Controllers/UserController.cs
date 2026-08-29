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
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Create new user.
        /// </summary>
        /// <param name="request">Information about user for registration</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Information about created user</returns>
        [HttpPost("create-user")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDto))]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(Error))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Error))]
        public async Task<IActionResult> Create([FromBody] UserModel request, CancellationToken cancellationToken)
        {
            var result = await _userService.CreateAsync(request);

            return this.ToActionResult(result);
        }

        /// <summary>
        /// Get JWT for work in system
        /// </summary>
        /// <param name="request">User info for login</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Token for authorization in system</returns>
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoginDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Error))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Error))]
        public async Task<IActionResult> Login([FromBody] LoginModel request, CancellationToken cancellationToken)
        {
            var result = await _userService.LoginAsync(request);

            return this.ToActionResult(result);
        }
    }
}