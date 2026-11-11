using System.Collections.Generic;
using mRemoteNG.App.Initialization;
using mRemoteNG.Messages;
using mRemoteNG.Messages.MessageWriters;
using NSubstitute;
using NUnit.Framework;

namespace mRemoteNGTests.App.Initialization
{
    public class MessageCollectorSetupTests
    {
        [Test]
        public void MessagesCollectedBeforeTheWritersExistedAreWritten()
        {
            MessageCollector collector = new();
            IMessage early = Substitute.For<IMessage>();
            collector.AddMessage(early);
            IMessageWriter writer = Substitute.For<IMessageWriter>();
            List<IMessageWriter> writers = [];

            MessageCollectorSetup.SetupMessageCollector(collector, writers);
            writers.Add(writer);
            MessageCollectorSetup.WriteCollectedMessages(collector, writers);

            writer.Received(1).Write(early);
        }

        [Test]
        public void MessagesAddedAfterwardsAreWrittenOnce()
        {
            MessageCollector collector = new();
            IMessageWriter writer = Substitute.For<IMessageWriter>();
            List<IMessageWriter> writers = [];

            MessageCollectorSetup.SetupMessageCollector(collector, writers);
            writers.Add(writer);
            MessageCollectorSetup.WriteCollectedMessages(collector, writers);
            IMessage later = Substitute.For<IMessage>();
            collector.AddMessage(later);

            writer.Received(1).Write(later);
        }
    }
}
