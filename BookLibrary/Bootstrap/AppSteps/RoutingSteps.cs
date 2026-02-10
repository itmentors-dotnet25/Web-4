namespace BookLibrary.Bootstrap.AppSteps;

public class RoutingSteps
{
    [BootstrapStep(60, "Configure routing and endpoints")]
    public static void ConfigureRouting(WebApplication app)
    {
        app.UseRouting();
        app.MapControllers();
    }
}
