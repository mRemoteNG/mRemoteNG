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

    [Test]
    public void TurningThePromptOnClearsAnEarlierStop()
    {
        mRemoteNG.Properties.OptionsDBsPage settings = new() { UserProfilePromptDeclined = true };

        Runtime.SetProfilePromptEnabled(settings, true);

        Assert.That(settings.UseUserProfiles, Is.True);
        Assert.That(settings.UserProfilePromptDeclined, Is.False);
        Assert.That(Runtime.ShouldUseUserProfiles(settings.UseUserProfiles, profileStoreExists: true, settings.UserProfilePromptDeclined), Is.True);
    }

    [Test]
    public void TurningThePromptOffStopsItForAnExistingProfileStore()
    {
        mRemoteNG.Properties.OptionsDBsPage settings = new() { UseUserProfiles = true };

        Runtime.SetProfilePromptEnabled(settings, false);

        Assert.That(Runtime.ShouldUseUserProfiles(settings.UseUserProfiles, profileStoreExists: true, settings.UserProfilePromptDeclined), Is.False);
    }

    [Test]
    public void StoppingThePromptAfterProfilesWereTurnedOnStopsIt()
    {
        mRemoteNG.Properties.OptionsDBsPage settings = new();
        Runtime.SetProfilePromptEnabled(settings, true);

        Runtime.SetProfilePromptEnabled(settings, false);

        Assert.That(Runtime.ShouldUseUserProfiles(settings.UseUserProfiles, profileStoreExists: true, settings.UserProfilePromptDeclined), Is.False);
    }
}
