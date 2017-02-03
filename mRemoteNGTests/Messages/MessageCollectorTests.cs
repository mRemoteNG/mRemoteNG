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
    }
}