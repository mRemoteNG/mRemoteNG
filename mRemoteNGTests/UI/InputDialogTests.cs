using System.Runtime.Versioning;
using NUnit.Framework;

namespace mRemoteNGTests.UI
{
    /// <summary>
    /// Tests for InputDialog utility. Note: ShowDialog tests require STA thread
    /// and cannot be fully automated without UI interaction, so we test the
    /// static method exists and is callable.
    /// </summary>
    [TestFixture]
    [SupportedOSPlatform("windows")]
    public class InputDialogTests
    {
        [Test]
        public void InputDialog_PromptMethod_Exists()
        {
            // Verify the static method is accessible via reflection
            var method = typeof(mRemoteNG.UI.InputDialog).GetMethod("Prompt",
                System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);

            Assert.That(method, Is.Not.Null, "InputDialog.Prompt static method should exist");
            Assert.That(method.ReturnType, Is.EqualTo(typeof(string)));
            Assert.That(method.GetParameters().Length, Is.EqualTo(3));
        }
    }
}
