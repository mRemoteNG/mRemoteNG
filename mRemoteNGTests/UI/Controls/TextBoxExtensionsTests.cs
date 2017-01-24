using mRemoteNG.UI.Controls;
using NUnit.Extensions.Forms;
using NUnit.Framework;

namespace mRemoteNGTests.UI.Controls
{
    [TestFixture]
    public class TextBoxExtensionsTests
    {
        private TextBoxExtensionsTestForm _textBoxExtensionsTestForm;

        [SetUp]
        public void Setup()
        {
            _textBoxExtensionsTestForm = new TextBoxExtensionsTestForm();
            _textBoxExtensionsTestForm.Show();
        }

        [TearDown]
        public void Teardown()
        {
            _textBoxExtensionsTestForm.Dispose();
            while (_textBoxExtensionsTestForm.Disposing)
            { }
            _textBoxExtensionsTestForm = null;
        }

        [Test]
        public void SetCueBannerSetsTheBannerText()
        {
            const string text = "Type Here";
            var textBox = new TextBoxTester(_textBoxExtensionsTestForm.textBox1.Name);
            Assert.That(textBox.Properties.SetCueBannerText(text), Is.True);
        }

        [Test]
        public void GetCueBannerReturnsCorrectValue()
        {
            const string text = "Type Here";
            var textBox = new TextBoxTester(_textBoxExtensionsTestForm.textBox1.Name);
            textBox.Properties.SetCueBannerText(text);
            Assert.That(textBox.Properties.GetCueBannerText(), Is.EqualTo(text));
        }
    }
}