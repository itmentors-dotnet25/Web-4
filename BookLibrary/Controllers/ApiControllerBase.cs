using BookLibrary.Data.Responses;
using Microsoft.AspNetCore.Mvc;

namespace BookLibrary.Controllers;

public abstract class ApiControllerBase : ControllerBase
{

    protected IActionResult Success<T>(T data, string message = "Operation completed successfully")
    {
        var response = new ApiResponseDto<T>(true, data, message);
        return Ok(response);
    }

    protected IActionResult Fail<T>(T data, string message = "Operation failed")
    {
        var response = new ApiResponseDto<T>(false, data, message);
        return BadRequest(response);
    }

    protected IActionResult SuccessNoContent(string message = "Operation completed successfully")
    {
        var response = new ApiResponseDto<object?>(true, null, message);
        return Ok(response);
    }

    protected IActionResult Created<T>(T data, string message = "Resource created successfully")
    {
        var response = new ApiResponseDto<T>(true, data, message);
        return StatusCode(StatusCodes.Status201Created, response);
    }
}
