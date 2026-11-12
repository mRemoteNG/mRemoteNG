using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using mRemoteNG.Connection;
using mRemoteNG.UI.Controls.ConnectionInfoPropertyGrid;
using NUnit.Framework;
using PropertyGridControl = mRemoteNG.UI.Controls.ConnectionInfoPropertyGrid.ConnectionInfoPropertyGrid;

namespace mRemoteNGTests.UI.Controls;

[Apartment(ApartmentState.STA)]
public class ConnectionInfoPropertyGridTests
{
    [Test]
    public void EditingTheInheritanceOfAConnectionRaisesAnInheritanceChange()
    {
        ConnectionInfo connection = new();
        using TestablePropertyGrid grid = new();
        grid.SelectedConnectionInfo = connection;
        grid.PropertyMode = PropertyMode.Inheritance;
        List<string> changedProperties = new();
        connection.PropertyChanged += (sender, args) => changedProperties.Add(args.PropertyName);

        grid.RaisePropertyValueChanged();

        Assert.That(changedProperties, Is.EqualTo(new[] { nameof(ConnectionInfo.Inheritance) }));
    }

    private sealed class TestablePropertyGrid : PropertyGridControl
    {
        public void RaisePropertyValueChanged() => OnPropertyValueChanged(new PropertyValueChangedEventArgs(null, false));
    }
}
