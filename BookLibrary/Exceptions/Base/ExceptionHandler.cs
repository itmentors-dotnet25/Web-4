using BookLibrary.Config;
using BookLibrary.Data.Responses;
using BookLibrary.Exceptions.Contracts;
using Microsoft.Extensions.Options;

namespace BookLibrary.Exceptions.Base;

public class ExceptionHandler(
    ILogger<ExceptionHandler> logger,
    IExceptionMetadataProvider metadataProvider,
    IOptions<ApplicationSettings> settings) : IExceptionHandlingFacade
{
    public async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        var metadata = metadataProvider.GetMetadata(ex);

        if (metadataProvider.ShouldLog(ex))
        {
            LogException(ex, metadata);
        }
        
        if (metadataProvider.ShouldReturnEmptyBody(ex))
        {
            context.Response.StatusCode = metadata.StatusCode;
            await context.Response.CompleteAsync();
            return;
        }

        var errorResponse = new ApiErrorDto(
            metadata.MessageCategory,
            new ErrorDetailDto(metadata.ErrorCode, metadata.ErrorMessage)
            { 
                Details = GetExceptionContext(ex)
            }
        );

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = metadata.StatusCode;
        
        await context.Response.WriteAsJsonAsync(errorResponse);
    }

    private void LogException(Exception ex, ExceptionMetadataAttribute metadata)
    {
        var contextData = new Dictionary<string, object?>
        {
            ["Type"] = ex.GetType().Name,
            ["Message"] = ex.Message,
            ["Context"] = GetExceptionContext(ex)
        };

        if (settings.Value.Logging.IncludeStackTraceInLogs && ex.StackTrace != null)
        {
            contextData["StackTrace"] = ex.StackTrace;
        }

        logger.Log(
            metadata.LogLevel,
            "Exception occurred: {@Context}",
            contextData);
    }

    private Dictionary<string, object?>? GetExceptionContext(Exception ex)
    {
        return ex switch
        {
            ModelNotFoundException m => new Dictionary<string, object?>
            {
                ["modelName"] = m.ModelName,
                ["searchCriteria"] = m.Context
            },
            ForbiddenException f => f.Context,
            ValidationException v => new Dictionary<string, object?>
            {
                ["errors"] = v.Errors
            },
            _ => null
        };
    }
}
