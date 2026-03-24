using CollegeManagementSystem.Services.Common;
using Microsoft.AspNetCore.Mvc;

namespace CollegeManagementSystem.Controllers;

public static class ControllerResultExtensions
{
    public static IActionResult ToActionResult(this ControllerBase controller, ServiceResult result)
    {
        if (result.Success)
        {
            return controller.NoContent();
        }

        return result.ErrorType switch
        {
            ServiceErrorType.NotFound => controller.NotFound(new { message = result.ErrorMessage }),
            ServiceErrorType.Conflict => controller.Conflict(new { message = result.ErrorMessage }),
            _ => controller.BadRequest(new { message = result.ErrorMessage })
        };
    }

    public static IActionResult ToActionResult<T>(this ControllerBase controller, ServiceResult<T> result)
    {
        if (result.Success)
        {
            return controller.Ok(result.Data);
        }

        return result.ErrorType switch
        {
            ServiceErrorType.NotFound => controller.NotFound(new { message = result.ErrorMessage }),
            ServiceErrorType.Conflict => controller.Conflict(new { message = result.ErrorMessage }),
            _ => controller.BadRequest(new { message = result.ErrorMessage })
        };
    }
}
