using mRemoteNG.Security;
using NUnit.Extensions.Forms;
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
            var textToSend = "abc123";
            textBox.Properties.Text = textToSend;
            Assert.That(textBox.Properties.SecureString.ConvertToUnsecureString(), Is.EqualTo(textToSend));
        }
    }
}