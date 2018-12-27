using System;
using System.Drawing;
using mRemoteNG.UI;
using mRemoteNG.UI.GraphicsUtilities;
using mRemoteNGTests.Properties;
using NSubstitute;
using NUnit.Framework;

namespace mRemoteNGTests.UI
{
    public class DisplayPropertiesTests
    {
        [Test]
        public void ScaleHeightReturnsValueScaledByHeightScalingFactor()
        {
            var graphics = Substitute.For<IGraphicsProvider>();
            graphics.GetResolutionScalingFactor().Returns(new SizeF(4, 2));
            var sut = new DisplayProperties(graphics);

            var initialValue = 10;
            var scaledValue = sut.ScaleHeight(initialValue);

            Assert.That(scaledValue, Is.EqualTo(sut.ResolutionScalingFactor.Height * initialValue));
        }

        [Test]
        public void ScaleWidthReturnsValueScaledByWidthScalingFactor()
        {
            var graphics = Substitute.For<IGraphicsProvider>();
            graphics.GetResolutionScalingFactor().Returns(new SizeF(4, 2));
            var sut = new DisplayProperties(graphics);

            var initialValue = 10;
            var scaledValue = sut.ScaleWidth(initialValue);

            Assert.That(scaledValue, Is.EqualTo(sut.ResolutionScalingFactor.Width * initialValue));
        }

        [Test]
        public void ScaleSizeReturnsNewSizeWithCorrectlyScaledHeight()
        {
            var graphics = Substitute.For<IGraphicsProvider>();
            graphics.GetResolutionScalingFactor().Returns(new SizeF(4, 2));
            var sut = new DisplayProperties(graphics);

            var initialValue = new Size(12, 16);
            var scaledValue = sut.ScaleSize(initialValue);

            Assert.That(scaledValue.Height, Is.EqualTo(sut.ResolutionScalingFactor.Height * initialValue.Height));
        }

        [Test]
        public void ScaleSizeReturnsNewSizeWithCorrectlyScaledWidth()
        {
            var graphics = Substitute.For<IGraphicsProvider>();
            graphics.GetResolutionScalingFactor().Returns(new SizeF(4, 2));
            var sut = new DisplayProperties(graphics);

            var initialValue = new Size(12, 16);
            var scaledValue = sut.ScaleSize(initialValue);

            Assert.That(scaledValue.Width, Is.EqualTo(sut.ResolutionScalingFactor.Width * initialValue.Width));
        }

        [Test]
        public void ScaleImageReturnsNewImageWithCorrectlyScaledHeight()
        {
            var graphics = Substitute.For<IGraphicsProvider>();
            graphics.GetResolutionScalingFactor().Returns(new SizeF(4, 2));
            var sut = new DisplayProperties(graphics);

            var initialValue = Resources.TestImage;
            var scaledValue = sut.ScaleImage(initialValue);

            Assert.That(scaledValue.Height, Is.EqualTo(sut.ResolutionScalingFactor.Height * initialValue.Height));
        }

        [Test]
        public void ScaleImageReturnsNewImageWithCorrectlyScaledWidth()
        {
            var graphics = Substitute.For<IGraphicsProvider>();
            graphics.GetResolutionScalingFactor().Returns(new SizeF(4, 2));
            var sut = new DisplayProperties(graphics);

            var initialValue = Resources.TestImage;
            var scaledValue = sut.ScaleImage(initialValue);

            Assert.That(scaledValue.Width, Is.EqualTo(sut.ResolutionScalingFactor.Width * initialValue.Width));
        }

        [Test]
        public void ResolutionScalingFactorAlwaysReturnsMostUpdatedValue()
        {
            var graphics = Substitute.For<IGraphicsProvider>();
            graphics.GetResolutionScalingFactor().Returns(new SizeF(4, 4));
            var sut = new DisplayProperties(graphics);

            graphics.GetResolutionScalingFactor().Returns(new SizeF(8, 8));
            Assert.That(sut.ResolutionScalingFactor.Width, Is.EqualTo(8));
        }

        [Test]
        public void AttemptingToScaleANullImageWillThrowAnException()
        {
            var sut = new DisplayProperties(Substitute.For<IGraphicsProvider>());
            Assert.Throws<ArgumentNullException>(() => sut.ScaleImage((Image)null));
        }

        [Test]
        public void AttemptingToScaleANullIconWillThrowAnException()
        {
            var sut = new DisplayProperties(Substitute.For<IGraphicsProvider>());
            Assert.Throws<ArgumentNullException>(() => sut.ScaleImage((Icon)null));
        }

        [Test]
        public void AttemptingToCallConstructorWithNullGraphicsProviderWillThrow()
        {
            // ReSharper disable once ObjectCreationAsStatement
            Assert.Throws<ArgumentNullException>(() => new DisplayProperties(null));
        }
    }
}
