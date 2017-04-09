using mRemoteNG.Connection;
using mRemoteNG.Container;
using mRemoteNG.Tree;
using mRemoteNG.Tree.Root;

namespace mRemoteNGTests.TestHelpers
{
    public class ConnectionTreeModelBuilder
    {
        public ConnectionTreeModel Build()
        {
            var model = new ConnectionTreeModel();
            var root = new RootNodeInfo(RootNodeType.Connection);
            var folder1 = new ContainerInfo { Name = "folder1", Username = "user1", Domain = "domain1", Password = "password1" };
            var con1 = new ConnectionInfo { Name = "Con1", Username = "user1", Domain = "domain1", Password = "password1" };
            var con2 = new ConnectionInfo { Name = "Con2", Username = "user2", Domain = "domain2", Password = "password2" };

            root.AddChild(folder1);
            root.AddChild(con2);
            folder1.AddChild(con1);
            model.AddRootNode(root);
            return model;
        }
    }
}