using System.Linq;
using mRemoteNG.Connection;
using mRemoteNG.Container;
using mRemoteNG.Messages;
using mRemoteNG.Security;
using mRemoteNG.Tree;
using mRemoteNG.Tree.Root;
using NUnit.Framework;

namespace mRemoteNGTests.Security;

public class DefaultKeyWarningTests
{
    private ConnectionTreeModel _model;
    private RootNodeInfo _root;
    private ConnectionInfo _connection;
    private MessageCollector _messageCollector;

    [SetUp]
    public void Setup()
    {
        DefaultKeyWarning.Reset();
        _messageCollector = new MessageCollector();
        _model = new ConnectionTreeModel();
        _root = new RootNodeInfo(RootNodeType.Connection);
        var folder = new ContainerInfo { Name = "folder" };
        _connection = new ConnectionInfo { Name = "con1", Password = "a stored password" };
        // A new connection takes the default inheritance, which other tests may leave turned on;
        // inherited from the folder, its passwords would read empty.
        _connection.Inheritance.TurnOffInheritanceCompletely();
        _root.AddChild(folder);
        folder.AddChild(_connection);
        _model.AddRootNode(_root);
    }

    private int Warnings => _messageCollector.Messages.Count(m => m.Class == MessageClass.WarningMsg);

    [Test]
    public void AppliesWhenTheRootHasNoPasswordOfItsOwn()
    {
        Assert.That(DefaultKeyWarning.Applies(_model), Is.True);
    }

    [Test]
    public void DoesNotApplyWhenTheRootHasItsOwnPassword()
    {
        _root.PasswordString = "a password of its own";

        Assert.That(DefaultKeyWarning.Applies(_model), Is.False);
    }

    [Test]
    public void DoesNotApplyWhenNoPasswordIsStored()
    {
        _connection.Password = "";

        Assert.That(DefaultKeyWarning.Applies(_model), Is.False);
    }

    [Test]
    public void AppliesToAGatewayPasswordToo()
    {
        _connection.Password = "";
        _connection.RDGatewayPassword = "a gateway password";

        Assert.That(DefaultKeyWarning.Applies(_model), Is.True);
    }

    [Test]
    public void DoesNotApplyToAnEmptyList()
    {
        Assert.That(DefaultKeyWarning.Applies(null), Is.False);
        Assert.That(DefaultKeyWarning.Applies(new ConnectionTreeModel()), Is.False);
    }

    [Test]
    public void ConnectionsFileIsWarnedAboutOnceASession()
    {
        DefaultKeyWarning.ForConnectionsFile(_model, dpapiProtected: false, _messageCollector);
        DefaultKeyWarning.ForConnectionsFile(_model, dpapiProtected: false, _messageCollector);

        Assert.That(Warnings, Is.EqualTo(1));
    }

    [Test]
    public void FileProtectedWithDpapiIsNotWarnedAbout()
    {
        DefaultKeyWarning.ForConnectionsFile(_model, dpapiProtected: true, _messageCollector);

        Assert.That(Warnings, Is.Zero);
    }

    [Test]
    public void DatabaseIsWarnedAboutOnceASessionOnItsOwn()
    {
        DefaultKeyWarning.ForConnectionsFile(_model, dpapiProtected: false, _messageCollector);
        DefaultKeyWarning.ForDatabase(_model, _messageCollector);
        DefaultKeyWarning.ForDatabase(_model, _messageCollector);

        Assert.That(Warnings, Is.EqualTo(2));
    }
}
