using mRemoteNG.App;
using NUnit.Framework;

namespace mRemoteNGTests.App;

[TestFixture]
public class RuntimeUserProfileTests
{
    [Test]
    public void ProfilesAreOffByDefault()
    {
        Assert.That(new mRemoteNG.Properties.OptionsDBsPage().UseUserProfiles, Is.False);
    }

    [Test]
    public void NoPromptWhenProfilesAreOffAndNoProfileStoreExists()
    {
        Assert.That(Runtime.ShouldUseUserProfiles(useUserProfiles: false, profileStoreExists: false), Is.False);
    }

    [Test]
    public void PromptWhenProfilesAreTurnedOn()
    {
        Assert.That(Runtime.ShouldUseUserProfiles(useUserProfiles: true, profileStoreExists: false), Is.True);
    }

    [Test]
    public void PromptWhenAProfileStoreAlreadyExists()
    {
        Assert.That(Runtime.ShouldUseUserProfiles(useUserProfiles: false, profileStoreExists: true), Is.True);
    }
}
