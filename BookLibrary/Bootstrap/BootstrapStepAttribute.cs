namespace BookLibrary.Bootstrap;

[AttributeUsage(AttributeTargets.Method)]
public class BootstrapStepAttribute(double order, string? description = null, bool skipInTests = false) : Attribute
{
    public double Order { get; } = order;
    public string? Description { get; } = description;
    public bool SkipInTests { get; } = skipInTests;
}
