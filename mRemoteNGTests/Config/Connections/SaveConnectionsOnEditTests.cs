using System.Collections.Specialized;
using System.ComponentModel;
using mRemoteNG.Config.Connections;
using mRemoteNG.Connection;
using mRemoteNG.Tree;
using NSubstitute;
using NUnit.Framework;

namespace mRemoteNGTests.Config.Connections
{
    public class SaveConnectionsOnEditTests
    {
        private IConnectionsService _connectionsService;

        [SetUp]
        public void Setup()
        {
            _connectionsService = Substitute.For<IConnectionsService>();
            var treeModel = Substitute.For<IConnectionTreeModel>();
            _connectionsService.ConnectionTreeModel.Returns(treeModel);
        }

        [Test]
        public void SaveConnectionsCalledWhenConnectionCollectionChanges()
        {
            new SaveConnectionsOnEdit().Subscribe(_connectionsService);

            _connectionsService.ConnectionTreeModel.CollectionChanged += Raise.Event<NotifyCollectionChangedEventHandler>(new object(), 
                new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, new List()));

            _connectionsService.Received(1).SaveConnectionsAsync();
        }

        [Test]
        public void SaveConnectionsCalledWhenConnectionPropertyChanges()
        {
            new SaveConnectionsOnEdit().Subscribe(_connectionsService);

            _connectionsService.ConnectionTreeModel.PropertyChanged += Raise.Event<PropertyChangedEventHandler>(new object(),
                new PropertyChangedEventArgs(""));

            _connectionsService.Received(1).SaveConnectionsAsync();
        }

        [Test]
        public void SaveConnectionsNotCalledWhenConnectionCollectionChangesAfterUnsubscribing()
        {
            var sut = new SaveConnectionsOnEdit();
            sut.Subscribe(_connectionsService);
            sut.Unsubscribe();

            _connectionsService.ConnectionTreeModel.CollectionChanged += Raise.Event<NotifyCollectionChangedEventHandler>(new object(),
                new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, new List()));

            _connectionsService.DidNotReceive().SaveConnectionsAsync();
        }

        [Test]
        public void SaveConnectionsNotCalledWhenConnectionPropertyChangesAfterUnsubscribing()
        {
            var sut = new SaveConnectionsOnEdit();
            sut.Subscribe(_connectionsService);
            sut.Unsubscribe();

            _connectionsService.ConnectionTreeModel.PropertyChanged += Raise.Event<PropertyChangedEventHandler>(new object(),
                new PropertyChangedEventArgs(""));

            _connectionsService.DidNotReceive().SaveConnectionsAsync();
        }
    }
}
