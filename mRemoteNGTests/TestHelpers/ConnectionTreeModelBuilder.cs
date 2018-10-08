using mRemoteNG.Connection;
using mRemoteNG.Container;
using mRemoteNG.Tree;
using mRemoteNG.Tree.Root;

namespace mRemoteNGTests.TestHelpers
{
    public class ConnectionTreeModelBuilder
    {
        /// <summary>
        /// Builds a tree which looks like:
        /// Root
        /// |- folder1
        /// |   |- con1
        /// |- con2
        /// |- folder2
        ///     |- folder3
        ///         |- con3
        /// </summary>
        /// <returns></returns>
        public ConnectionTreeModel Build()
        {
            var model = new ConnectionTreeModel();
            var root = new RootNodeInfo(RootNodeType.Connection);
            var folder1 = new ContainerInfo { Name = "folder1", Username = "user1", Domain = "domain1", Password = "password1" };
            var folder2 = new ContainerInfo { Name = "folder2", Username = "user2", Domain = "domain2", Password = "password2" };
            var folder3 = new ContainerInfo
            {
                Name = "folder3",
                Inheritance =
                {
                    Username = true,
                    Domain = true,
                    Password = true
                }
            };
            var con1 = new ConnectionInfo { Name = "Con1", Username = "user1", Domain = "domain1", Password = "password1" };
            var con2 = new ConnectionInfo { Name = "Con2", Username = "user2", Domain = "domain2", Password = "password2" };
            var con3 = new ContainerInfo
            {
                Name = "con3",
                Inheritance =
                {
                    Username = true,
                    Domain = true,
                    Password = true
                }
            };

            root.AddChild(folder1);
            root.AddChild(con2);
            folder1.AddChild(con1);
            root.AddChild(folder2);
            folder2.AddChild(folder3);
            folder3.AddChild(con3);
            model.AddRootNode(root);
            return model;
        }
    }
}