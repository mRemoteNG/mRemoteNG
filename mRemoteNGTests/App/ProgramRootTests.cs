using mRemoteNG.App;
using NUnit.Framework;

namespace mRemoteNGTests.App;

[TestFixture]
public class ProgramRootTests
{
    private string? _originalSkipRuntimeChecksValue;

    [SetUp]
    public void SetUp()
    {
        _originalSkipRuntimeChecksValue = Environment.GetEnvironmentVariable("MREMOTENG_SKIP_RUNTIME_CHECKS");
    }

    [TearDown]
    public void TearDown()
    {
        Environment.SetEnvironmentVariable("MREMOTENG_SKIP_RUNTIME_CHECKS", _originalSkipRuntimeChecksValue);
    }

    [Test]
    public void ShouldSkipNativeRuntimeChecks_WhenCliSwitchIsProvided_ReturnsTrue()
    {
        Environment.SetEnvironmentVariable("MREMOTENG_SKIP_RUNTIME_CHECKS", null);

        bool shouldSkip = ProgramRoot.ShouldSkipNativeRuntimeChecks(new[] { "--skip-runtime-checks" });

        Assert.That(shouldSkip, Is.True);
    }

    [TestCase("1")]
    [TestCase("true")]
    [TestCase("TRUE")]
    public void ShouldSkipNativeRuntimeChecks_WhenEnvironmentRequestsBypass_ReturnsTrue(string envValue)
    {
        Environment.SetEnvironmentVariable("MREMOTENG_SKIP_RUNTIME_CHECKS", envValue);

        bool shouldSkip = ProgramRoot.ShouldSkipNativeRuntimeChecks(Array.Empty<string>());

        Assert.That(shouldSkip, Is.True);
    }

    [Test]
    public void ShouldSkipNativeRuntimeChecks_WhenNoBypassConfigured_ReturnsExpectedValueForBuildType()
    {
        Environment.SetEnvironmentVariable("MREMOTENG_SKIP_RUNTIME_CHECKS", null);

        bool shouldSkip = ProgramRoot.ShouldSkipNativeRuntimeChecks(Array.Empty<string>());

#if PORTABLE
        Assert.That(shouldSkip, Is.True);
#else
        Assert.That(shouldSkip, Is.False);
#endif
    }
}
