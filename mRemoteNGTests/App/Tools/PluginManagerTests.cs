using System;
using System.Reflection;
using mRemoteNG.Connection;
using mRemoteNG.Tools;
using NUnit.Framework;
using mRemoteNG.App;

namespace mRemoteNGTests.App.Tools
{
    [TestFixture]
    public class PluginManagerTests
    {
        [SetUp]
        public void Setup()
        {
            // Reset PluginManager instance
            var field = typeof(PluginManager).GetField("_instance", BindingFlags.Static | BindingFlags.NonPublic);
            field?.SetValue(null, null);

            // Create a new ConnectionInitiator for testing
            Runtime.ConnectionInitiator = new ConnectionInitiator();
        }

        [TearDown]
        public void TearDown()
        {
            // Reset PluginManager instance
            var field = typeof(PluginManager).GetField("_instance", BindingFlags.Static | BindingFlags.NonPublic);
            field?.SetValue(null, null);
        }

        [Test]
        public void Instance_ShouldNotBeNull()
        {
            Assert.That(PluginManager.Instance, Is.Not.Null);
        }

        [Test]
        public void OnConnectionOpened_ShouldBeTriggered_WhenConnectionInitiatorFires()
        {
            bool eventFired = false;
            string receivedHost = null;
            string receivedProtocol = null;

            PluginManager.Instance.OnConnectionOpened += (host, protocol) =>
            {
                eventFired = true;
                receivedHost = host;
                receivedProtocol = protocol;
            };

            // We need to invoke the event on ConnectionInitiator.
            // Since the event is public, we can't invoke it directly from outside unless we have a method in ConnectionInitiator to do so,
            // or we use reflection to get the backing delegate.
            // However, we added ConnectionOpened to ConnectionInitiator.
            // The event can only be invoked from within the class.
            
            // To simulate the event, we can use reflection to find the event delegate and invoke it.
            var initiator = Runtime.ConnectionInitiator;
            var eventField = typeof(ConnectionInitiator).GetField("ConnectionOpened", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            if (eventField == null)
            {
                 // Try getting property? Events sometimes are backing fields.
                 // In C#, field-like events generate a private delegate field with the same name.
                 eventField = typeof(ConnectionInitiator).GetField("ConnectionOpened", BindingFlags.Instance | BindingFlags.NonPublic);
            }
            
            Assert.That(eventField, Is.Not.Null, "Could not find ConnectionOpened event backing field");

            MulticastDelegate eventDelegate = (MulticastDelegate)eventField.GetValue(initiator);
            if (eventDelegate != null)
            {
                eventDelegate.DynamicInvoke("test-host", "RDP");
            }
            else
            {
                // If null, it means no one subscribed? But PluginManager should have subscribed.
                Assert.Fail("ConnectionOpened delegate is null, PluginManager did not subscribe?");
            }

            Assert.That(eventFired, Is.True);
            Assert.That(receivedHost, Is.EqualTo("test-host"));
            Assert.That(receivedProtocol, Is.EqualTo("RDP"));
        }

        [Test]
        public void OnConnectionClosed_ShouldBeTriggered_WhenConnectionInitiatorFires()
        {
            bool eventFired = false;
            string receivedHost = null;
            string receivedProtocol = null;

            PluginManager.Instance.OnConnectionClosed += (host, protocol) =>
            {
                eventFired = true;
                receivedHost = host;
                receivedProtocol = protocol;
            };

            var initiator = Runtime.ConnectionInitiator;
            var eventField = typeof(ConnectionInitiator).GetField("ConnectionClosed", BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.That(eventField, Is.Not.Null, "Could not find ConnectionClosed event backing field");

            MulticastDelegate eventDelegate = (MulticastDelegate)eventField.GetValue(initiator);
            Assert.That(eventDelegate, Is.Not.Null, "ConnectionClosed delegate is null");
            
            eventDelegate.DynamicInvoke("test-host", "SSH");

            Assert.That(eventFired, Is.True);
            Assert.That(receivedHost, Is.EqualTo("test-host"));
            Assert.That(receivedProtocol, Is.EqualTo("SSH"));
        }
        
        [Test]
        public void RootNodes_ShouldNotBeNull()
        {
             // Runtime.ConnectionsService might be null or its properties null in test env.
             // We need to ensure it returns Empty list instead of throwing.
             Assert.That(PluginManager.Instance.RootNodes, Is.Not.Null);
        }
    }
}
