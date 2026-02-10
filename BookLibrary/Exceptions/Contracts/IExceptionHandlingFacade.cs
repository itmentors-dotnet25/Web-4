namespace BookLibrary.Exceptions.Contracts;

public interface IExceptionHandlingFacade
{
    //IActionResult HandleException(Exception ex);
    Task HandleExceptionAsync(HttpContext context, Exception ex);
}
