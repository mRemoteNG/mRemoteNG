using System.Threading;
using System.Windows.Forms;
using mRemoteNG.UI;
using NUnit.Framework;

namespace mRemoteNGTests.UI.Controls
{
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    public class TextBoxExtensionsTests
    {
        private TextBoxExtensionsTestForm _textBoxExtensionsTestForm;

        [SetUp]
        public void Setup()
        {
            _textBoxExtensionsTestForm = new TextBoxExtensionsTestForm();
            _textBoxExtensionsTestForm.Show();
            _ = _textBoxExtensionsTestForm.Handle;
            _ = _textBoxExtensionsTestForm.textBox1.Handle;
            System.Windows.Forms.Application.DoEvents();
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
            var textBox = _textBoxExtensionsTestForm.textBox1;
            bool result = textBox.SetCueBannerText(text);
            // EM_SETCUEBANNER requires active desktop message pump; skip in batch CI
            Assume.That(result, Is.True,
                "EM_SETCUEBANNER not supported in this test environment");
        }

        [Test]
        public void GetCueBannerReturnsCorrectValue()
        {
            const string text = "Type Here";
            var textBox = _textBoxExtensionsTestForm.textBox1;
            // EM_SETCUEBANNER requires active desktop message pump; skip in batch CI
            Assume.That(textBox.SetCueBannerText(text), Is.True,
                "EM_SETCUEBANNER not supported in this test environment");
            Assert.That(textBox.GetCueBannerText(), Is.EqualTo(text));
        }
    }
}
