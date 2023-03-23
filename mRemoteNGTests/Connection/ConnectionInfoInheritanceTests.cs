using System.Collections;
using System.Linq;
using System.Reflection;
using mRemoteNG.Connection;
using mRemoteNG.Container;
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
            inheritance.GetProperties().Contains(typeof(ConnectionInfoInheritance).GetProperty("Icon"));
        Assert.That(hasIconProperty, Is.True);
    }

    [Test]
    public void GetPropertiesExludesPropertiesThatShouldNotBeSet()
    {
        var inheritance = new ConnectionInfoInheritance(new ConnectionInfo());
        var hasEverythingInheritedProperty = inheritance.GetProperties()
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

    private bool PropertyIsChangedWhenSettingInheritAll(PropertyInfo property)
    {
        var propertiesIgnoredByInheritAll = new ArrayList { "IsDefault" };
        return propertiesIgnoredByInheritAll.Contains(property);
    }

    private bool PropertyIsBoolean(PropertyInfo property)
    {
        return property.PropertyType.Name == typeof(bool).Name;
    }

    private bool BooleanPropertyIsSetToFalse(PropertyInfo property, ConnectionInfoInheritance inheritance)
    {
        return (bool)property.GetValue(inheritance) == false;
    }

    private bool BooleanPropertyIsSetToTrue(PropertyInfo property, ConnectionInfoInheritance inheritance)
    {
        return (bool)property.GetValue(inheritance);
    }
}