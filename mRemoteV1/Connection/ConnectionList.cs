using System;
using System.Collections;


namespace mRemoteNG.Connection
{
	public class ConnectionList : CollectionBase
	{
        public ConnectionInfo this[object Index]
		{
			get
			{
				if (Index is ConnectionInfo)
					return (ConnectionInfo)Index;
				else
					return ((ConnectionInfo) (List[Convert.ToInt32(Index)]));
			}
		}
			
        public new int Count
		{
			get { return List.Count; }
		}
			
		public ConnectionInfo Add(ConnectionInfo connectionInfo)
		{
            List.Add(connectionInfo);
			return connectionInfo;
		}
			
		public void AddRange(ConnectionInfo[] connectionInfoArray)
		{
			foreach (ConnectionInfo connectionInfo in connectionInfoArray)
			{
				List.Add(connectionInfo);
			}
		}
			
		public ConnectionInfo FindByConstantID(string id)
		{
			foreach (ConnectionInfo connectionInfo in List)
			{
				if (connectionInfo.ConstantID == id)
					return connectionInfo;
			}
				
			return null;
		}
			
		public ConnectionList Copy()
		{
			try
			{
                return (ConnectionList)MemberwiseClone();
			}
			catch (Exception)
			{
			}
				
			return null;
		}
			
		public new void Clear()
		{
            List.Clear();
		}
	}
}