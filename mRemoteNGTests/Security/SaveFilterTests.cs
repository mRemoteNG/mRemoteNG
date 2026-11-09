using mRemoteNG.Security;
using NUnit.Framework;

namespace mRemoteNGTests.Security
{
    [TestFixture]
    public class SaveFilterTests
    {
        [Test]
        public void SelectionsCannotRelaxPasswordOrUsernameLimits()
        {
            var filter = new SaveFilter(allowPasswords: false, allowUsernames: false);
            filter.SavePassword = true;
            filter.SaveUsername = true;

            Assert.That(filter.SavePassword, Is.False);
            Assert.That(filter.SaveUsername, Is.False);
        }

        [Test]
        public void ExportCopyPreservesLimitsAndSelections()
        {
            var filter = new SaveFilter(allowPasswords: false, allowUsernames: false)
            {
                SavePassword = true,
                SaveUsername = true,
                SaveDomain = false,
                SaveInheritance = false
            };

            var export = filter.RestrictForExport();
            export.SavePassword = true;
            export.SaveUsername = true;

            Assert.That(export.SavePassword, Is.False);
            Assert.That(export.SaveUsername, Is.False);
            Assert.That(export.SaveDomain, Is.False);
            Assert.That(export.SaveInheritance, Is.False);
        }
    }
}
