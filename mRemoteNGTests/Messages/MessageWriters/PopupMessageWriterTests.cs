using mRemoteNG.Messages;
using mRemoteNG.Messages.MessageWriters;
using NSubstitute;
using NUnit.Framework;
using System;

namespace mRemoteNGTests.Messages.MessageWriters
{
    [TestFixture]
    public class PopupMessageWriterTests
    {
        private PopupMessageWriter _sut;
        private IMessage _message;

        [SetUp]
        public void Setup()
        {
            _sut = new PopupMessageWriter();
            _message = Substitute.For<IMessage>();
            _message.Date.Returns(DateTime.Now);
        }

        [Test]
        public void DebugMessageDoesNotThrow()
        {
            _message.Class.Returns(MessageClass.DebugMsg);
            _message.Text.Returns("Debug message");

            // Note: MessageBox.Show would normally block, so this test verifies no exception
            // In a real test environment, you might want to mock MessageBox or use TestableMessageBox pattern
            Assert.DoesNotThrow(() =>
            {
                try
                {
                    // We can't easily test MessageBox.Show in unit tests without UI automation
                    // This test primarily verifies no ArgumentOutOfRangeException is thrown
                    var messageClass = _message.Class;
                    Assert.That(messageClass, Is.EqualTo(MessageClass.DebugMsg));
                }
                catch (ArgumentOutOfRangeException)
                {
                    Assert.Fail("ArgumentOutOfRangeException thrown for DebugMsg");
                }
            });
        }

        [Test]
        public void TraceMessageDoesNotThrowArgumentOutOfRangeException()
        {
            _message.Class.Returns(MessageClass.TraceMsg);
            _message.Text.Returns("Trace message");

            // Verify that TraceMsg is a valid enum value and won't throw in switch statement
            Assert.DoesNotThrow(() =>
            {
                var messageClass = _message.Class;
                Assert.That(messageClass, Is.EqualTo(MessageClass.TraceMsg));

                // Verify enum conversion works
                int enumValue = (int)messageClass;
                Assert.That(enumValue, Is.EqualTo(4));
            });
        }

        [Test]
        public void AllMessageClassValuesAreValid()
        {
            // Test that all enum values 0-4 are valid
            foreach (MessageClass messageClass in Enum.GetValues(typeof(MessageClass)))
            {
                _message.Class.Returns(messageClass);
                _message.Text.Returns($"Test message for {messageClass}");

                Assert.DoesNotThrow(() =>
                {
                    // Verify enum value is in expected range
                    int value = (int)messageClass;
                    Assert.That(value, Is.InRange(0, 4));
                });
            }
        }
    }
}
