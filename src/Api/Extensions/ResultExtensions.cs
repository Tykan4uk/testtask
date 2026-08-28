using Application.Common;
using Microsoft.AspNetCore.Mvc;

namespace Api.Extensions
{
    public static class ResultExtensions
    {
        public static IActionResult ToActionResult(this ControllerBase controller, Result result)
        {
            if (result.IsSuccess)
                return controller.Ok();

            return result.Error!.Type switch
            {
                ErrorType.NotFound => controller.NotFound(result.Error),

                ErrorType.BadRequest => controller.BadRequest(result.Error),

                ErrorType.Conflict => controller.Conflict(result.Error),

                _ => controller.StatusCode(StatusCodes.Status500InternalServerError, result.Error)
            };
        }

        public static IActionResult ToActionResult<T>(this ControllerBase controller, Result<T> result)
        {
            if (result.IsSuccess)
                return controller.Ok(result.Value);

            return result.Error!.Type switch
            {
                ErrorType.NotFound => controller.NotFound(result.Error),

                ErrorType.BadRequest => controller.BadRequest(result.Error),

                ErrorType.Conflict => controller.Conflict(result.Error),

                _ => controller.StatusCode(StatusCodes.Status500InternalServerError, result.Error)
            };
        }
    }
}
