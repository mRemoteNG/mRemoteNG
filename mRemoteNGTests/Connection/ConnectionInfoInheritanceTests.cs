using NUnit.Framework;
using mRemoteNG.Connection;
using System.Reflection;
using System;

namespace mRemoteNGTests.Connection
{
    [TestFixture]
    public class ConnectionInfoInheritanceTests
    {
        private ConnectionInfo _connectionInfo;
        private ConnectionInfoInheritance _inheritance;
        private PropertyInfo[] _inheritanceProperties = typeof(ConnectionInfoInheritance).GetProperties();

        [SetUp]
        public void Setup()
        {
            _connectionInfo = new ConnectionInfo();
            _inheritance = new ConnectionInfoInheritance(_connectionInfo);
        }

        [TearDown]
        public void Teardown()
        {
            _connectionInfo = null;
            _inheritance = null;
        }

        [Test]
        public void TurnOffInheritanceCompletely()
        {
            _inheritance.Username = true;
            _inheritance.TurnOffInheritanceCompletely();
            bool allPropertiesFalse = true;
            foreach (var property in _inheritanceProperties)
            {
                if (property.PropertyType.Name == typeof(Boolean).Name && (bool)property.GetValue(_inheritance) == true)
                    allPropertiesFalse = false;
            }
            Assert.That(allPropertiesFalse, Is.True);
        }

        [Test]
        public void TurnOnInheritanceCompletely()
        {
            _inheritance.TurnOnInheritanceCompletely();
            bool allPropertiesTrue = true;
            foreach(var property in _inheritanceProperties)
            {
                if (property.PropertyType.Name == typeof(Boolean).Name && (bool)property.GetValue(_inheritance) == false)
                    allPropertiesTrue = false;
            }
            Assert.That(allPropertiesTrue, Is.True);
        }

        [Test]
        public void DisableInheritanceTurnsOffAllInheritance()
        {
            _inheritance.Username = true;
            _inheritance.DisableInheritance();
            bool allPropertiesFalse = true;
            foreach (var property in _inheritanceProperties)
            {
                if (property.PropertyType.Name == typeof(Boolean).Name && (bool)property.GetValue(_inheritance) == true)
                    allPropertiesFalse = false;
            }
            Assert.That(allPropertiesFalse, Is.True);
        }

        [Test]
        public void EnableInheritanceRestoresPreviousInheritanceValues()
        {
            _inheritance.Username = true;
            _inheritance.DisableInheritance();
            _inheritance.EnableInheritance();
            Assert.That(_inheritance.Username, Is.True);
        }
    }
}