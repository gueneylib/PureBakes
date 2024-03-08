namespace Purebakes.Tests.Fixtures;

public static class FixtureExtensions
{
    /// <summary>
    /// Configures the given fixture so that it omits recursion.
    /// </summary>
    /// <param name="fixture">The fixture to configure.</param>
    /// <param name="recursionDepth">The max recursion depth.</param>
    public static IFixture ConfigureToSuppressCircularReferences(this IFixture fixture, int recursionDepth = 0)
    {
        // Disable recursion to avoid cycles.
        fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => fixture.Behaviors.Remove(b));
        fixture.Behaviors.OfType<OmitOnRecursionBehavior>().ToList().ForEach(b => fixture.Behaviors.Remove(b));
        if (recursionDepth < 1)
        {
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }
        else
        {
            fixture.Behaviors.Add(new OmitOnRecursionBehavior(recursionDepth));
        }

        return fixture;
    }
}