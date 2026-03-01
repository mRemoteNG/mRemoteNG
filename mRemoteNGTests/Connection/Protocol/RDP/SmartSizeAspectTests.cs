using System;
using System.Drawing;
using mRemoteNG.Connection.Protocol.RDP;
using NUnit.Framework;

namespace mRemoteNGTests.Connection.Protocol.RDP
{
    [TestFixture]
    public class SmartSizeAspectTests
    {
        [Test]
        public void SmartSizeAspect_EnumExists()
        {
            // Verify the enum value exists (compilation check essentially)
            var value = RDPResolutions.SmartSizeAspect;
            Assert.That(value.ToString(), Is.EqualTo("SmartSizeAspect"));
        }

        [Test]
        public void GetResolutionRectangle_ReturnsZero_ForSmartSizeAspect()
        {
            // Arrange
            var resolution = RDPResolutions.SmartSizeAspect;

            // Act
            Rectangle rect = resolution.GetResolutionRectangle();

            // Assert
            Assert.That(rect.Width, Is.EqualTo(0));
            Assert.That(rect.Height, Is.EqualTo(0));
        }

        [Test]
        public void GetResolutionRectangle_ReturnsZero_ForSmartSize()
        {
            // Verify existing behavior
            var resolution = RDPResolutions.SmartSize;
            Rectangle rect = resolution.GetResolutionRectangle();
            Assert.That(rect.Width, Is.EqualTo(0));
            Assert.That(rect.Height, Is.EqualTo(0));
        }

        [Test]
        public void GetResolutionRectangle_ReturnsValues_ForExplicitResolution()
        {
            // Verify existing behavior
            var resolution = RDPResolutions.Res1920x1080;
            Rectangle rect = resolution.GetResolutionRectangle();
            Assert.That(rect.Width, Is.EqualTo(1920));
            Assert.That(rect.Height, Is.EqualTo(1080));
        }
    }
}
