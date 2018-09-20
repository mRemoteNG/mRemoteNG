using mRemoteNG.Connection;
using mRemoteNG.Container;
using NUnit.Framework;
using System.Collections;
using System.Linq;
using System.Reflection;

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
            Assert.That(AllInheritancePropertiesAreFalse(), Is.True);
        }

        [Test]
        public void TurnOnInheritanceCompletely()
        {
            _inheritance.TurnOnInheritanceCompletely();
            Assert.That(AllInheritancePropertiesAreTrue(), Is.True);
        }

        [Test]
        public void DisableInheritanceTurnsOffAllInheritance()
        {
            _inheritance.Username = true;
            _inheritance.DisableInheritance();
            Assert.That(AllInheritancePropertiesAreFalse(), Is.True);
        }

        [Test]
        public void EnableInheritanceRestoresPreviousInheritanceValues()
        {
            _inheritance.Username = true;
            _inheritance.DisableInheritance();
            _inheritance.EnableInheritance();
            Assert.That(_inheritance.Username, Is.True);
        }

        [Test]
        public void GetPropertiesReturnsListOfSettableProperties()
        {
            var hasIconProperty = _inheritance.GetProperties().Contains(typeof(ConnectionInfoInheritance).GetProperty("Icon"));
            Assert.That(hasIconProperty, Is.True);
        }

        [Test]
        public void GetPropertiesExludesPropertiesThatShouldNotBeSet()
        {
            var hasEverythingInheritedProperty = _inheritance.GetProperties().Contains(typeof(ConnectionInfoInheritance).GetProperty("EverythingInherited"));
            Assert.That(hasEverythingInheritedProperty, Is.False);
        }

        [Test]
        public void AlwaysReturnInheritedValueIfRequested()
        {
            var expectedSetting = false;

            var container = new ContainerInfo { AutomaticResize = expectedSetting };
            var con1 = new ConnectionInfo
            {
                AutomaticResize = true,
                Inheritance = {AutomaticResize = true}
            };
            container.AddChild(con1);

            Assert.That(con1.AutomaticResize, Is.EqualTo(expectedSetting));
        }

        private bool AllInheritancePropertiesAreTrue()
        {
            var allPropertiesTrue = true;
            foreach (var property in _inheritanceProperties)
            {
                if (PropertyIsBoolean(property) && PropertyIsChangedWhenSettingInheritAll(property) && BooleanPropertyIsSetToFalse(property))
                    allPropertiesTrue = false;
            }
            return allPropertiesTrue;
        }

        private bool AllInheritancePropertiesAreFalse()
        {
            var allPropertiesFalse = true;
            foreach (var property in _inheritanceProperties)
            {
                if (PropertyIsBoolean(property) && PropertyIsChangedWhenSettingInheritAll(property) && BooleanPropertyIsSetToTrue(property))
                    allPropertiesFalse = false;
            }
            return allPropertiesFalse;
        }

        private bool PropertyIsChangedWhenSettingInheritAll(PropertyInfo property)
        {
            var propertiesIgnoredByInheritAll = new ArrayList {"IsDefault"};
            return propertiesIgnoredByInheritAll.Contains(property);
        }

        private bool PropertyIsBoolean(PropertyInfo property)
        {
            return (property.PropertyType.Name == typeof(bool).Name);
        }

        private bool BooleanPropertyIsSetToFalse(PropertyInfo property)
        {
            return (bool)property.GetValue(_inheritance) == false;
        }

        private bool BooleanPropertyIsSetToTrue(PropertyInfo property)
        {
            return (bool)property.GetValue(_inheritance);
        }
    }
}