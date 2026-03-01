using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using mRemoteNG.Connection;
using mRemoteNG.Container;
using mRemoteNG.Tools;
using mRemoteNG.Tree.Root;
using NUnit.Framework;

namespace mRemoteNGTests.Connection;

[TestFixture]
public class ConnectionInfoInheritanceTests
{
    private readonly PropertyInfo[] _inheritanceProperties = typeof(ConnectionInfoInheritance).GetProperties();


    [Test]
    public void TurnOffInheritanceCompletely()
    {
        var inheritance = new ConnectionInfoInheritance(new ConnectionInfo()) { Username = true };
        inheritance.TurnOffInheritanceCompletely();
        Assert.That(AllInheritancePropertiesAreFalse(inheritance), Is.True);
    }

    [Test]
    public void TurnOnInheritanceCompletely()
    {
        var inheritance = new ConnectionInfoInheritance(new ConnectionInfo());
        inheritance.TurnOnInheritanceCompletely();
        Assert.That(AllInheritancePropertiesAreTrue(inheritance), Is.True);
    }

    [Test]
    public void InheritanceIsDisabledWhenAttachedToARootNode()
    {
        var inheritance = new ConnectionInfoInheritance(new RootNodeInfo(RootNodeType.Connection));
        Assert.That(inheritance.InheritanceActive, Is.False);
    }

    [Test]
    public void InheritanceIsDisabledWhenAttachedToAPuttyRootNode()
    {
        var inheritance = new ConnectionInfoInheritance(new RootNodeInfo(RootNodeType.PuttySessions));
        Assert.That(inheritance.InheritanceActive, Is.False);
    }

    [Test]
    public void InheritanceIsDisabledWhenAttachedToAPuttyNode()
    {
        var inheritance = new ConnectionInfoInheritance(new RootPuttySessionsNodeInfo());
        Assert.That(inheritance.InheritanceActive, Is.False);
    }

    [Test]
    public void InheritanceIsDisabledWhenAttachedToANodeDirectlyUnderTheRootNode()
    {
        var con = new ConnectionInfo();
        new RootNodeInfo(RootNodeType.Connection).AddChild(con);
        Assert.That(con.Inheritance.InheritanceActive, Is.False);
    }

    [Test]
    public void InheritanceIsEnabledWhenAttachedToNormalConnectionInfo()
    {
        var inheritance = new ConnectionInfoInheritance(new ConnectionInfo());
        Assert.That(inheritance.InheritanceActive, Is.True);
    }

    [Test]
    public void InheritanceIsEnabledWhenAttachedToNormalContainerInfo()
    {
        var inheritance = new ConnectionInfoInheritance(new ContainerInfo());
        Assert.That(inheritance.InheritanceActive, Is.True);
    }

    [Test]
    public void GetPropertiesReturnsListOfSettableProperties()
    {
        var inheritance = new ConnectionInfoInheritance(new ConnectionInfo());
        var hasIconProperty =
            ConnectionInfoInheritance.GetProperties().Contains(typeof(ConnectionInfoInheritance).GetProperty("Icon"));
        Assert.That(hasIconProperty, Is.True);
    }

    [Test]
    public void GetPropertiesExludesPropertiesThatShouldNotBeSet()
    {
        var inheritance = new ConnectionInfoInheritance(new ConnectionInfo());
        var hasEverythingInheritedProperty = ConnectionInfoInheritance.GetProperties()
            .Contains(typeof(ConnectionInfoInheritance).GetProperty("EverythingInherited"));
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
            Inheritance = { AutomaticResize = true }
        };
        container.AddChild(con1);

        Assert.That(con1.AutomaticResize, Is.EqualTo(expectedSetting));
    }

    [Test]
    public void ApplyAutomaticInheritanceFromParentSetsOnlyMatchingProperties()
    {
        var root = new RootNodeInfo(RootNodeType.Connection);
        var parentContainer = new ContainerInfo
        {
            Username = "parentUser",
            Domain = "parentDomain",
            Port = 3389
        };
        var childConnection = new ConnectionInfo
        {
            Username = "parentUser",
            Domain = "childDomain",
            Port = 3389
        };

        root.AddChild(parentContainer);
        parentContainer.AddChild(childConnection);

        childConnection.Inheritance.TurnOffInheritanceCompletely();
        childConnection.Inheritance.ApplyAutomaticInheritanceFromParent();

        Assert.Multiple(() =>
        {
            Assert.That(childConnection.Inheritance.Username, Is.True);
            Assert.That(childConnection.Inheritance.Port, Is.True);
            Assert.That(childConnection.Inheritance.Domain, Is.False);
        });
    }

    [Test]
    public void ApplyAutomaticInheritanceFromParentComparesStoredParentValues()
    {
        var root = new RootNodeInfo(RootNodeType.Connection);
        var grandParent = new ContainerInfo { Username = "grandParentUser" };
        var parent = new ContainerInfo { Username = "parentRawUser" };
        var child = new ConnectionInfo { Username = "grandParentUser" };

        root.AddChild(grandParent);
        grandParent.AddChild(parent);
        parent.AddChild(child);

        parent.Inheritance.Username = true;
        child.Inheritance.TurnOffInheritanceCompletely();

        child.Inheritance.ApplyAutomaticInheritanceFromParent();

        Assert.That(child.Inheritance.Username, Is.False);
    }

    [Test]
    public void EverythingInheritedAutoOptionTriggersAutomaticComparison()
    {
        var root = new RootNodeInfo(RootNodeType.Connection);
        var parentContainer = new ContainerInfo
        {
            Username = "parentUser",
            Domain = "parentDomain"
        };
        var childConnection = new ConnectionInfo
        {
            Username = "parentUser",
            Domain = "childDomain"
        };

        root.AddChild(parentContainer);
        parentContainer.AddChild(childConnection);

        childConnection.Inheritance.TurnOffInheritanceCompletely();

        var converter = new MiscTools.YesNoAutoTypeConverter();
        var context = new InheritanceTypeDescriptorContext(childConnection.Inheritance);
        var convertedValue = converter.ConvertFrom(context, culture: null, value: "Auto");
        childConnection.Inheritance.EverythingInherited = (bool)convertedValue!;

        Assert.Multiple(() =>
        {
            Assert.That(childConnection.Inheritance.Username, Is.True);
            Assert.That(childConnection.Inheritance.Domain, Is.False);
        });
    }

    private bool AllInheritancePropertiesAreTrue(ConnectionInfoInheritance inheritance)
    {
        var allPropertiesTrue = true;
        foreach (var property in _inheritanceProperties)
            if (PropertyIsBoolean(property) && PropertyIsChangedWhenSettingInheritAll(property) &&
                BooleanPropertyIsSetToFalse(property, inheritance))
                allPropertiesTrue = false;
        return allPropertiesTrue;
    }

    private bool AllInheritancePropertiesAreFalse(ConnectionInfoInheritance inheritance)
    {
        var allPropertiesFalse = true;
        foreach (var property in _inheritanceProperties)
            if (PropertyIsBoolean(property) && PropertyIsChangedWhenSettingInheritAll(property) &&
                BooleanPropertyIsSetToTrue(property, inheritance))
                allPropertiesFalse = false;
        return allPropertiesFalse;
    }

    private static bool PropertyIsChangedWhenSettingInheritAll(PropertyInfo property)
    {
        var propertiesIgnoredByInheritAll = new[]
        {
            nameof(ConnectionInfoInheritance.EverythingInherited),
            nameof(ConnectionInfoInheritance.InheritanceActive)
        };
        return !propertiesIgnoredByInheritAll.Contains(property.Name);
    }

    private static bool PropertyIsBoolean(PropertyInfo property)
    {
        return property.PropertyType.Name == typeof(bool).Name;
    }

    private static bool BooleanPropertyIsSetToFalse(PropertyInfo property, ConnectionInfoInheritance inheritance)
    {
        return (bool)property.GetValue(inheritance) == false;
    }

    private static bool BooleanPropertyIsSetToTrue(PropertyInfo property, ConnectionInfoInheritance inheritance)
    {
        return (bool)property.GetValue(inheritance);
    }

    private sealed class InheritanceTypeDescriptorContext : ITypeDescriptorContext
    {
        public InheritanceTypeDescriptorContext(ConnectionInfoInheritance inheritance)
        {
            Instance = inheritance;
        }

        public IContainer? Container => null;
        public object Instance { get; }
        public PropertyDescriptor? PropertyDescriptor => null;

        public void OnComponentChanged()
        {
        }

        public bool OnComponentChanging()
        {
            return true;
        }

        public object? GetService(Type serviceType)
        {
            return null;
        }
    }
}
