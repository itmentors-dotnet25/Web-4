using System.Reflection;

namespace BookLibrary.Bootstrap;

public static class Bootstrap
{
    public static void ExecuteBuilderSteps(WebApplicationBuilder builder)
    {
        ExecuteSteps(builder);
    }

    public static void ExecuteAppSteps(WebApplication app)
    {
        ExecuteSteps(app);
    }

    private static void ExecuteSteps<T>(T target)
    {
        var assembly = typeof(Bootstrap).Assembly;
        var isTestEnvironment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Test";

        var bootstrapSteps = assembly.GetTypes()
            .SelectMany(t => t.GetMethods(BindingFlags.Public | BindingFlags.Static))
            .Where(m => m.GetCustomAttribute<BootstrapStepAttribute>() != null)
            .Select(m => new
            {
                Method = m,
                Attribute = m.GetCustomAttribute<BootstrapStepAttribute>()!
            })
            .Where(x => x.Method.GetParameters().Length == 1 && 
                        x.Method.GetParameters()[0].ParameterType == typeof(T))
            .Where(x => !x.Attribute.SkipInTests || !isTestEnvironment)
            .OrderBy(x => x.Attribute.Order)
            .ToList();

        foreach (var step in bootstrapSteps)
        {
            try
            {
                step.Method.Invoke(null, [target]);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ {step.Attribute.Description}");
                Console.WriteLine($"  Error: {ex.Message}");
                throw;
            }
        }
    }
}
