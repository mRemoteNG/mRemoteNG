using System.Collections.Generic;
using System.Linq;
using mRemoteNG.Connection;

namespace mRemoteNGTests.TestHelpers
{
	public class ConnectionInfoAllConnectionPropertiesEqualityComparer : IEqualityComparer<ConnectionInfo>
	{
		public bool Equals(ConnectionInfo x, ConnectionInfo y)
		{
			if (x == null && y == null)
				return true;
			if ((x == null) != (y == null))
				return false;
			return GetHashCode(x) == GetHashCode(y);
		}

		public int GetHashCode(ConnectionInfo connectionInfo)
		{
			var allProperties = connectionInfo.GetSerializableProperties();

			unchecked // Overflow is fine, just wrap
			{
				return allProperties
					.Aggregate(17, 
						(current, prop) => current * 23 + HashOf(prop.GetValue(connectionInfo)));
			}
		}

		private static int HashOf(object value) => value switch
		{
			// Password and the Vault properties are null until they are set.
			null => 0,
			// PluginProperties: what it holds, not which dictionary instance it is.
			IReadOnlyDictionary<string, string> entries => entries
				.OrderBy(entry => entry.Key, System.StringComparer.Ordinal)
				.Aggregate(19, (hash, entry) => unchecked((hash * 31 + entry.Key.GetHashCode()) * 31 + (entry.Value?.GetHashCode() ?? 0))),
			_ => value.GetHashCode()
		};
	}
}
