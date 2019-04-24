using mRemoteNG.Connection;
using mRemoteNG.Container;

namespace mRemoteNGTests.TestHelpers
{
	internal static class ConnectionInfoHelpers
	{
		/// <summary>
		/// Returns a <see cref="ConnectionInfo"/> object with randomized
		/// values in all fields.
		/// </summary>
		internal static ConnectionInfo GetRandomizedConnectionInfo(bool randomizeInheritance = false)
		{
			var connectionInfo = new ConnectionInfo().RandomizeValues();

			if (randomizeInheritance)
				connectionInfo.Inheritance = GetRandomizedInheritance(connectionInfo);

			return connectionInfo;
		}

		internal static ContainerInfo GetRandomizedContainerInfo(bool randomizeInheritance = false)
		{
			var containerInfo = new ContainerInfo().RandomizeValues();

			if (randomizeInheritance)
				containerInfo.Inheritance = GetRandomizedInheritance(containerInfo);

			return containerInfo;
		}

		internal static ConnectionInfoInheritance GetRandomizedInheritance(ConnectionInfo parent)
		{
			var inheritance = new ConnectionInfoInheritance(parent, true);
			foreach (var property in inheritance.GetProperties())
			{
				property.SetValue(inheritance, Randomizer.RandomBool());
			}
			return inheritance;
		}
    }
}
