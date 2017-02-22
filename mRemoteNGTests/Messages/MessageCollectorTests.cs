using mRemoteNG.Messages;
using NSubstitute;
using NUnit.Framework;


namespace mRemoteNGTests.Messages
{
    public class MessageCollectorTests
    {
        private MessageCollector _messageCollector;

        [SetUp]
        public void Setup()
        {
            _messageCollector = new MessageCollector();
        }

        [Test]
        public void InitializesWithNoMessages()
        {
            Assert.That(_messageCollector.Messages, Is.Empty);
        }

        [Test]
        public void MessageAddedToList()
        {
            var message = Substitute.For<IMessage>();
            _messageCollector.AddMessage(message);
            Assert.That(_messageCollector.Messages, Does.Contain(message));
        }

        [Test]
        public void ListCanBeCleared()
        {
            var message = Substitute.For<IMessage>();
            _messageCollector.AddMessage(message);
            _messageCollector.ClearMessages();
            Assert.That(_messageCollector.Messages, Is.Empty);
        }

        [Test]
        public void MessagesAreUnique()
        {
            var message = Substitute.For<IMessage>();
            _messageCollector.AddMessage(message);
            _messageCollector.AddMessage(message);
            Assert.That(_messageCollector.Messages, Is.Unique);
        }

        [Test]
        public void NotifiedWhenMessageAdded()
        {
            var wasCalled = false;
            _messageCollector.CollectionChanged += (sender, args) => wasCalled = true;
            var message = Substitute.For<IMessage>();
            _messageCollector.AddMessage(message);
            Assert.That(wasCalled, Is.True);
        }

        [Test]
        public void BatchAddAddsAllItems()
        {
            var msg1 = Substitute.For<IMessage>();
            var msg2 = Substitute.For<IMessage>();
            var msgCollection = new[] {msg1, msg2};
            _messageCollector.AddMessages(msgCollection);
            Assert.That(_messageCollector.Messages, Is.EquivalentTo(msgCollection));
        }

        [Test]
        public void OneNotificationRaisedForBatchAdd()
        {
            var notificationCount = 0;
            _messageCollector.CollectionChanged += (sender, args) => notificationCount++;
            var msg1 = Substitute.For<IMessage>();
            var msg2 = Substitute.For<IMessage>();
            _messageCollector.AddMessages(new[] { msg1, msg2 });
            Assert.That(notificationCount, Is.EqualTo(1));
        }

        [Test]
        public void EventNotRaisedIfMsgIsntUnique()
        {
            var notificationCount = 0;
            var msg1 = Substitute.For<IMessage>();
            _messageCollector.AddMessage(msg1);
            _messageCollector.CollectionChanged += (sender, args) => notificationCount++;
            _messageCollector.AddMessage(msg1);
            Assert.That(notificationCount, Is.EqualTo(0));
        }
    }
}