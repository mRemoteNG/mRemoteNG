using System.Threading.Tasks;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using NUnit.Framework;

namespace mRemoteNGTests.Connection
{
    /// <summary>
    /// The previous session reopens its connections one at a time, each once the one before has
    /// connected, failed or been closed.
    /// </summary>
    public class ConnectionOutcomeTests
    {
        private TestProtocol _protocol;
        private ConnectionInitiator.ConnectionOutcome _outcome;

        [SetUp]
        public void Setup()
        {
            _protocol = new TestProtocol();
            _outcome = new ConnectionInitiator.ConnectionOutcome();
            _outcome.Watch(_protocol);
        }

        [Test]
        public async Task ConnectedIsSuccess()
        {
            _protocol.RaiseConnected();

            Assert.That(await _outcome.Task, Is.True);
        }

        [Test]
        public async Task DisconnectedIsFailure()
        {
            _outcome.ConnectStarted();
            _protocol.RaiseDisconnected();

            Assert.That(await _outcome.Task, Is.False);
        }

        [Test]
        public async Task ErrorIsFailure()
        {
            _outcome.ConnectStarted();
            _protocol.RaiseError();

            Assert.That(await _outcome.Task, Is.False);
        }

        [Test]
        public async Task ClosedIsFailure()
        {
            _outcome.ConnectStarted();
            _protocol.RaiseClosed();

            Assert.That(await _outcome.Task, Is.False);
        }

        [Test]
        public async Task AnAttemptThatEndsBeforeConnectingIsFailure()
        {
            _outcome.AttemptEnded();

            Assert.That(await _outcome.Task, Is.False);
        }

        [Test]
        public void AnAttemptThatStartedConnectingWaitsForTheProtocol()
        {
            _outcome.ConnectStarted();
            _outcome.AttemptEnded();

            Assert.That(_outcome.Task.IsCompleted, Is.False);
        }

        [Test]
        public async Task TheFirstOutcomeCounts()
        {
            _protocol.RaiseConnected();
            _protocol.RaiseClosed();

            Assert.That(await _outcome.Task, Is.True);
        }

        private class TestProtocol : ProtocolBase
        {
            public void RaiseConnected() => Event_Connected(this);
            public void RaiseDisconnected() => Event_Disconnected(this, "timed out", 264);
            public void RaiseError() => Event_ErrorOccured(this, "error", 1);
            public void RaiseClosed() => Event_Closed(this);
        }
    }
}