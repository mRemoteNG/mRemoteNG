using System;
using System.Reflection;
using mRemoteNG.Config.Connections;
using mRemoteNG.Config.Putty;
using mRemoteNG.Connection;
using mRemoteNG.Container;
using mRemoteNG.Tree;
using mRemoteNG.Tree.Root;
using NSubstitute;
using NUnit.Framework;

namespace mRemoteNGTests.Tree;

[NonParallelizable]
public class SqlDraftTrackingTests
{
    private ConnectionTreeModel _model;
    private RootNodeInfo _root;
    private ConnectionInfo _connection;

    [SetUp]
    public void SetUp()
    {
        _model = new ConnectionTreeModel { SqlTreeRevision = 7 };
        _root = new RootNodeInfo(RootNodeType.Connection);
        _connection = new ConnectionInfo { Name = "Original" };
        _root.AddChild(_connection);
        _model.AddRootNode(_root);
        _model.MarkSqlChangesSaved(_model.SqlEditGeneration);
    }

    [Test]
    public void SharedEdits_IncludeRootMetadataInheritanceAndCollections()
    {
        void Check(Action edit)
        {
            edit();
            Assert.That(_model.HasPendingSqlChanges, Is.True);
            _model.MarkSqlChangesSaved(_model.SqlEditGeneration);
        }
        Check(() => _connection.Name = "Changed");
        Check(() => _root.Name = "New root");
        Check(() => _root.PasswordString = "Synthetic password");
        Check(() => { _connection.Inheritance.Username = true; _connection.RaiseInheritanceChanged(); });
        ConnectionInfo added = new();
        Check(() => _root.AddChild(added));
        Check(() => _root.SetChildPosition(added, 0));
        Check(() => _root.RemoveChild(added));
        ContainerInfo secondRoot = new();
        Check(() => _model.AddRootNode(secondRoot));
        Check(() => _model.MoveRootNode(secondRoot, 0));
        Check(() => _model.RemoveRootNode(secondRoot));
    }

    [Test]
    public void LocalProperties_DoNotDirtySharedTree()
    {
        _root.IsExpanded = !_root.IsExpanded;
        _connection.Favorite = !_connection.Favorite;
        Assert.That(_model.HasPendingSqlChanges, Is.False);
    }

    [Test]
    public void SaveAcknowledgement_DoesNotDiscardEditsMadeDuringSave()
    {
        _connection.Name = "Saved generation";
        long snapshot = _model.SqlEditGeneration;
        _connection.Name = "Later draft";
        _model.MarkSqlChangesSaved(snapshot);
        Assert.That(_model.HasPendingSqlChanges, Is.True);
        _model.MarkSqlChangesSaved(_model.SqlEditGeneration);
        Assert.That(_model.HasPendingSqlChanges, Is.False);
        _model.HasSqlSaveConflict = true;
        Assert.That(_model.HasPendingSqlChanges, Is.True);
    }

    [TestCase(false)]
    [TestCase(true)]
    public void Refresh_EditDuringLoadOrBeforeUiDispatch_RetainsDraft(bool editDuringLoad)
    {
        ConnectionsService service = new(PuttySessionsManager.Instance);
        typeof(ConnectionsService).GetProperty(nameof(service.ConnectionTreeModel)).SetValue(service, _model);
        typeof(ConnectionsService).GetProperty(nameof(service.UsingDatabase)).SetValue(service, true);
        IConnectionsLoader loader = Substitute.For<IConnectionsLoader>();
        loader.Load().Returns(_ =>
        {
            if (editDuringLoad) _connection.Name = "During load";
            return new ConnectionTreeModel { SqlTreeRevision = 8 };
        });
        bool replaced = service.TryReloadSqlConnections(loader, action =>
        {
            if (!editDuringLoad) _connection.Name = "Before publish";
            action();
        });
        Assert.That(replaced, Is.False);
        Assert.That(service.ConnectionTreeModel, Is.SameAs(_model));
        Assert.That(_model.HasPendingSqlChanges, Is.True);
    }

    [Test]
    public void Refresh_AlreadyDirty_DoesNotFetchSnapshot()
    {
        ConnectionsService service = new(PuttySessionsManager.Instance);
        typeof(ConnectionsService).GetProperty(nameof(service.ConnectionTreeModel)).SetValue(service, _model);
        typeof(ConnectionsService).GetProperty(nameof(service.UsingDatabase)).SetValue(service, true);
        IConnectionsLoader loader = Substitute.For<IConnectionsLoader>();
        _connection.Name = "Draft";
        Assert.That(service.TryReloadSqlConnections(loader, _ => Assert.Fail("Must not dispatch")), Is.False);
        loader.DidNotReceive().Load();
    }
}
