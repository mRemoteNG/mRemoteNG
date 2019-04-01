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
						(current, prop) => current * 23 + prop.GetValue(connectionInfo).GetHashCode());
			}
		}
	}
}
