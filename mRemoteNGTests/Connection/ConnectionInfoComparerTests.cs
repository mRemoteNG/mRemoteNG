using System.ComponentModel;
using mRemoteNG.Connection;
using NUnit.Framework;


namespace mRemoteNGTests.Connection;

public class ConnectionInfoComparerTests
{
    private ConnectionInfo _con1;
    private ConnectionInfo _con2;

    [OneTimeSetUp]
    public void OnetimeSetup()
    {
        _con1 = new ConnectionInfo { Name = "a" };
        _con2 = new ConnectionInfo { Name = "b" };
    }

    [Test]
    public void SortAscendingOnName()
    {
        var comparer = new ConnectionInfoComparer<string>(node => node.Name)
        {
            SortDirection = ListSortDirection.Ascending
        };
        var compareReturn = comparer.Compare(_con1, _con2);
        Assert.That(compareReturn, Is.Negative);
    }

    [Test]
    public void SortDescendingOnName()
    {
        var comparer = new ConnectionInfoComparer<string>(node => node.Name)
        {
            SortDirection = ListSortDirection.Descending
        };
        var compareReturn = comparer.Compare(_con1, _con2);
        Assert.That(compareReturn, Is.Positive);
    }
}