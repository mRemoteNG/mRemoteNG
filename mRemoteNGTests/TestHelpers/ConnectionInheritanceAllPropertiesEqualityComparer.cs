using System.Collections.Generic;
using System.Linq;
using mRemoteNG.Connection;

namespace mRemoteNGTests.TestHelpers
{
	public class ConnectionInheritanceAllPropertiesEqualityComparer : IEqualityComparer<ConnectionInfoInheritance>
	{
		public bool Equals(ConnectionInfoInheritance x, ConnectionInfoInheritance y)
		{
			if (x == null && y == null)
				return true;
			if ((x == null) != (y == null))
				return false;
			return GetHashCode(x) == GetHashCode(y);
		}

		public int GetHashCode(ConnectionInfoInheritance inheritance)
		{
			var allProperties = inheritance.GetProperties();

			unchecked // Overflow is fine, just wrap
			{
				return allProperties
					.Aggregate(17,
						(current, prop) => current * 23 + prop.GetValue(inheritance).GetHashCode());
			}
		}
	}
}
