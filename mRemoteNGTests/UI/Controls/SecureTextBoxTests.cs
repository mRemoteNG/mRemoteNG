using mRemoteNG.Security;
using mRemoteNGTests.NUnitExtensions;
using NUnit.Framework;

namespace mRemoteNGTests.UI.Controls
{
    public class SecureTextBoxTests
    {
        private SecureTextBoxTestForm _testForm;

        [SetUp]
        public void Setup()
        {
            _testForm = new SecureTextBoxTestForm();
            _testForm.Show();
        }

        [TearDown]
        public void TearDown()
        {
            _testForm.Close();
            while (_testForm.Disposing) { }
            _testForm = null;
        }

        [Test]
        public void TextboxInputGetsAddedToSecureString()
        {
            var textBox = new SecureTextBoxTester(_testForm.secureTextBox1.Name);
            const string textToSend = "abc123";
            textBox.Properties.Text = textToSend;
            Assert.That(textBox.Properties.SecString.ConvertToUnsecureString(), Is.EqualTo(textToSend));
        }
    }
}