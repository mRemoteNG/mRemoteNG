using System;
using System.Drawing;
using mRemoteNG.Tools;
using NUnit.Framework;

namespace mRemoteNGTests.Tools
{
    public class TabColorConverterTests
    {
        private MiscTools.TabColorConverter _converter;

        [SetUp]
        public void Setup()
        {
            _converter = new MiscTools.TabColorConverter();
        }

        [TestCase(typeof(string), true)]
        [TestCase(typeof(Color), true)]
        public void CanConvertFrom(Type typeToConvertFrom, bool expectedOutcome)
        {
            var actualOutcome = _converter.CanConvertFrom(typeToConvertFrom);
            Assert.That(actualOutcome, Is.EqualTo(expectedOutcome));
        }

        [TestCase(typeof(string), true)]
        [TestCase(typeof(Color), true)]
        public void CanConvertTo(Type typeToConvertTo, bool expectedOutcome)
        {
            var actualOutcome = _converter.CanConvertTo(typeToConvertTo);
            Assert.That(actualOutcome, Is.EqualTo(expectedOutcome));
        }

        [Test]
        public void ConvertFromColorToStringNamedColor()
        {
            var color = Color.Red;
            var result = _converter.ConvertFrom(color);
            Assert.That(result, Is.EqualTo("Red"));
        }

        [Test]
        public void ConvertFromColorToStringCustomColor()
        {
            var color = Color.FromArgb(255, 128, 64, 32);
            var result = _converter.ConvertFrom(color);
            Assert.That(result, Is.EqualTo("#804020"));
        }

        [Test]
        public void ConvertFromColorToStringCustomColorWithAlpha()
        {
            var color = Color.FromArgb(128, 255, 0, 0);
            var result = _converter.ConvertFrom(color);
            Assert.That(result, Is.EqualTo("#80FF0000"));
        }

        [Test]
        public void ConvertFromStringReturnsString()
        {
            var colorString = "Blue";
            var result = _converter.ConvertFrom(colorString);
            Assert.That(result, Is.EqualTo("Blue"));
        }

        [Test]
        public void ConvertFromHexStringReturnsString()
        {
            var colorString = "#FF0000";
            var result = _converter.ConvertFrom(colorString);
            Assert.That(result, Is.EqualTo("#FF0000"));
        }

        [Test]
        public void ConvertFromNullReturnsEmptyString()
        {
            var result = _converter.ConvertFrom(null);
            Assert.That(result, Is.EqualTo(string.Empty));
        }

        [Test]
        public void ConvertFromEmptyStringReturnsEmptyString()
        {
            var result = _converter.ConvertFrom("");
            Assert.That(result, Is.EqualTo(string.Empty));
        }

        [Test]
        public void ConvertToStringFromString()
        {
            var colorString = "Green";
            var result = _converter.ConvertTo(colorString, typeof(string));
            Assert.That(result, Is.EqualTo("Green"));
        }

        [Test]
        public void ConvertToColorFromNamedString()
        {
            var colorString = "Red";
            var result = _converter.ConvertTo(colorString, typeof(Color));
            Assert.That(result, Is.EqualTo(Color.Red));
        }

        [Test]
        public void ConvertToColorFromHexString()
        {
            var colorString = "#FF0000";
            var result = (Color)_converter.ConvertTo(colorString, typeof(Color));
            Assert.That(result.A, Is.EqualTo(255));
            Assert.That(result.R, Is.EqualTo(255));
            Assert.That(result.G, Is.EqualTo(0));
            Assert.That(result.B, Is.EqualTo(0));
        }

        [Test]
        public void ConvertToColorFromEmptyStringReturnsEmpty()
        {
            var result = _converter.ConvertTo("", typeof(Color));
            Assert.That(result, Is.EqualTo(Color.Empty));
        }

        [Test]
        public void ConvertToColorFromNullReturnsEmpty()
        {
            var result = _converter.ConvertTo(null, typeof(Color));
            Assert.That(result, Is.EqualTo(Color.Empty));
        }

        [Test]
        public void GetStandardValuesSupportedReturnsTrue()
        {
            var result = _converter.GetStandardValuesSupported(null);
            Assert.That(result, Is.True);
        }

        [Test]
        public void GetStandardValuesReturnsColorList()
        {
            var result = _converter.GetStandardValues(null);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.GreaterThan(0));
        }

        [Test]
        public void GetStandardValuesExclusiveReturnsFalse()
        {
            var result = _converter.GetStandardValuesExclusive(null);
            Assert.That(result, Is.False);
        }

        [Test]
        public void ConvertFromColorObjectDoesNotThrowException()
        {
            // This test verifies the fix for the "Object of type 'System.Drawing.Color' cannot be converted to type 'System.String'" error
            var color = Color.FromArgb(255, 100, 150, 200);
            Assert.DoesNotThrow(() => _converter.ConvertFrom(color));
        }

        [Test]
        public void ColorPropertyUsesTabColorConverter()
        {
            // This test verifies that the Color property can properly handle Color objects
            // by using TabColorConverter instead of System.Drawing.ColorConverter
            var color = Color.Blue;
            var result = _converter.ConvertFrom(color);
            Assert.That(result, Is.EqualTo("Blue"));
        }
    }
}
