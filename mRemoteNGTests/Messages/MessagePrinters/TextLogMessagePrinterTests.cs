using log4net;
using mRemoteNG.Messages;
using mRemoteNG.Messages.MessagePrinters;
using NSubstitute;
using NUnit.Framework;

namespace mRemoteNGTests.Messages.MessagePrinters
{
    public class TextLogMessagePrinterTests
    {
        private TextLogMessagePrinter _messagePrinter;
        private ILog _log4NetLogger;

        [SetUp]
        public void Setup()
        {
            _log4NetLogger = Substitute.For<ILog>();
            _messagePrinter = new TextLogMessagePrinter(_log4NetLogger);
        }

        [Test]
        public void MultipleMessagesPassedToLog4Net()
        {
            var message1 = Substitute.For<IMessage>();
            var message2 = Substitute.For<IMessage>();
            message1.Class.Returns(MessageClass.InformationMsg);
            message2.Class.Returns(MessageClass.InformationMsg);
            _messagePrinter.Print(new[] {message1, message2});
            _log4NetLogger.ReceivedWithAnyArgs(2).Info("");
        }

        [Test]
        public void InfoMessagesPassedToLog4Net()
        {
            var message = Substitute.For<IMessage>();
            message.Class.Returns(MessageClass.InformationMsg);
            _messagePrinter.Print(message);
            _log4NetLogger.ReceivedWithAnyArgs(1).Info("");
        }

        [Test]
        public void DebugMessagesPassedToLog4Net()
        {
            var message = Substitute.For<IMessage>();
            message.Class.Returns(MessageClass.DebugMsg);
            _messagePrinter.Print(message);
            _log4NetLogger.ReceivedWithAnyArgs(1).Debug("");
        }

        [Test]
        public void WarningMessagesPassedToLog4Net()
        {
            var message = Substitute.For<IMessage>();
            message.Class.Returns(MessageClass.WarningMsg);
            _messagePrinter.Print(message);
            _log4NetLogger.ReceivedWithAnyArgs(1).Warn("");
        }

        [Test]
        public void ErrorMessagesPassedToLog4Net()
        {
            var message = Substitute.For<IMessage>();
            message.Class.Returns(MessageClass.ErrorMsg);
            _messagePrinter.Print(message);
            _log4NetLogger.ReceivedWithAnyArgs(1).Error("");
        }

        [Test]
        public void InfoMessagesNotPassedToLog4NetIfTurnedOff()
        {
            var message = Substitute.For<IMessage>();
            message.Class.Returns(MessageClass.InformationMsg);
            _messagePrinter.PrintInfoMessages = false;
            _messagePrinter.Print(message);
            _log4NetLogger.DidNotReceiveWithAnyArgs().Info("");
        }

        [Test]
        public void DebugMessagesNotPassedToLog4NetIfTurnedOff()
        {
            var message = Substitute.For<IMessage>();
            message.Class.Returns(MessageClass.DebugMsg);
            _messagePrinter.PrintDebugMessages = false;
            _messagePrinter.Print(message);
            _log4NetLogger.DidNotReceiveWithAnyArgs().Debug("");
        }

        [Test]
        public void WarningMessagesNotPassedToLog4NetIfTurnedOff()
        {
            var message = Substitute.For<IMessage>();
            message.Class.Returns(MessageClass.WarningMsg);
            _messagePrinter.PrintWarningMessages = false;
            _messagePrinter.Print(message);
            _log4NetLogger.DidNotReceiveWithAnyArgs().Warn("");
        }

        [Test]
        public void ErrorMessagesNotPassedToLog4NetIfTurnedOff()
        {
            var message = Substitute.For<IMessage>();
            message.Class.Returns(MessageClass.ErrorMsg);
            _messagePrinter.PrintErrorMessages = false;
            _messagePrinter.Print(message);
            _log4NetLogger.DidNotReceiveWithAnyArgs().Error("");
        }
    }
}