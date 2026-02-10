using Serilog;

namespace BookLibrary.Bootstrap.BuilderSteps;

public class LoggingSteps
{
    [BootstrapStep(20, "Configure logging", skipInTests: true)]
    public static void ConfigureLogging(WebApplicationBuilder builder)
    {
        builder.Logging.ClearProviders();
        
        builder.Host.UseSerilog((context, services, loggerConfiguration) =>
        {
            loggerConfiguration
                .ReadFrom.Configuration(builder.Configuration);
        });
    }
}
