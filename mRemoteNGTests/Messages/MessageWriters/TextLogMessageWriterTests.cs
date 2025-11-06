using mRemoteNG.App;
using mRemoteNG.Messages;
using mRemoteNG.Messages.MessageWriters;
using NSubstitute;
using NUnit.Framework;
using log4net;

namespace mRemoteNGTests.Messages.MessageWriters
{
    [TestFixture]
    public class TextLogMessageWriterTests
    {
        private TextLogMessageWriter _sut;
        private Logger _mockLogger;
        private ILog _mockLog;
        private IMessage _message;

        [SetUp]
        public void Setup()
        {
            _mockLogger = Substitute.For<Logger>();
            _mockLog = Substitute.For<ILog>();
            _mockLogger.Log.Returns(_mockLog);
            _sut = new TextLogMessageWriter(_mockLogger);
            _message = Substitute.For<IMessage>();
        }

        [Test]
        public void DebugMessageCallsLogDebug()
        {
            _message.Class.Returns(MessageClass.DebugMsg);
            _message.Text.Returns("Debug test message");

            _sut.Write(_message);

            _mockLog.Received(1).Debug("Debug test message");
        }

        [Test]
        public void InformationMessageCallsLogInfo()
        {
            _message.Class.Returns(MessageClass.InformationMsg);
            _message.Text.Returns("Info test message");

            _sut.Write(_message);

            _mockLog.Received(1).Info("Info test message");
        }

        [Test]
        public void WarningMessageCallsLogWarn()
        {
            _message.Class.Returns(MessageClass.WarningMsg);
            _message.Text.Returns("Warning test message");

            _sut.Write(_message);

            _mockLog.Received(1).Warn("Warning test message");
        }

        [Test]
        public void ErrorMessageCallsLogError()
        {
            _message.Class.Returns(MessageClass.ErrorMsg);
            _message.Text.Returns("Error test message");

            _sut.Write(_message);

            _mockLog.Received(1).Error("Error test message");
        }

        [Test]
        public void TraceMessageCallsLogDebugWithTracePrefix()
        {
            _message.Class.Returns(MessageClass.TraceMsg);
            _message.Text.Returns("[SSH_DotNet TRACE] Some trace info");

            _sut.Write(_message);

            _mockLog.Received(1).Debug("TRACE - [SSH_DotNet TRACE] Some trace info");
        }

        [Test]
        public void TraceMessageDoesNotThrowException()
        {
            _message.Class.Returns(MessageClass.TraceMsg);
            _message.Text.Returns("Trace message");

            Assert.DoesNotThrow(() => _sut.Write(_message));
        }
    }
}
