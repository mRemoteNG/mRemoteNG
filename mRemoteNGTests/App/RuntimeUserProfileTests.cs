using mRemoteNG.App;
using NUnit.Framework;

namespace mRemoteNGTests.App;

[TestFixture]
public class RuntimeUserProfileTests
{
    [Test]
    public void ProfilesAreOffByDefault()
    {
        mRemoteNG.Properties.OptionsDBsPage settings = new();
        Assert.That(settings.UseUserProfiles, Is.False);
        Assert.That(settings.UserProfilePromptDeclined, Is.False);
    }

    [Test]
    public void NoPromptWhenProfilesAreOffAndNoProfileStoreExists()
    {
        Assert.That(Runtime.ShouldUseUserProfiles(useUserProfiles: false, profileStoreExists: false, promptDeclined: false), Is.False);
    }

    [Test]
    public void PromptWhenProfilesAreTurnedOn()
    {
        Assert.That(Runtime.ShouldUseUserProfiles(useUserProfiles: true, profileStoreExists: false, promptDeclined: false), Is.True);
    }

    [Test]
    public void PromptWhenAProfileStoreAlreadyExists()
    {
        Assert.That(Runtime.ShouldUseUserProfiles(useUserProfiles: false, profileStoreExists: true, promptDeclined: false), Is.True);
    }

    [Test]
    public void NoPromptForAnExistingProfileStoreOnceTheUserStoppedIt()
    {
        Assert.That(Runtime.ShouldUseUserProfiles(useUserProfiles: false, profileStoreExists: true, promptDeclined: true), Is.False);
    }

    [Test]
    public void TurningProfilesOnPromptsAgainAfterTheUserStoppedIt()
    {
        Assert.That(Runtime.ShouldUseUserProfiles(useUserProfiles: true, profileStoreExists: true, promptDeclined: true), Is.True);
    }

    [TestCase(true, true)]
    [TestCase(false, false)]
    public void CancellingAsksWhetherToStopPrompting(bool answer, bool expected)
    {
        bool asked = false;
        bool result = Runtime.ShouldStopProfilePrompt(useUserProfiles: false, () => { asked = true; return answer; });

        Assert.That(asked, Is.True);
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void CancellingDoesNotAskWhenProfilesWereTurnedOn()
    {
        bool asked = false;
        bool result = Runtime.ShouldStopProfilePrompt(useUserProfiles: true, () => { asked = true; return true; });

        Assert.That(asked, Is.False);
        Assert.That(result, Is.False);
    }
}
