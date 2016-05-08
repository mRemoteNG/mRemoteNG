using System;
using System.Collections;


namespace mRemoteNG.Connection
{
	public class ConnectionList : CollectionBase
	{
        #region Public Properties
        public ConnectionInfo this[object Index]
		{
			get
			{
				if (Index is ConnectionInfo)
				{
					return (ConnectionInfo)Index;
				}
				else
				{
					return ((ConnectionInfo) (List[Convert.ToInt32(Index)]));
				}
			}
		}
			
        public new int Count
		{
			get { return List.Count; }
		}
        #endregion
			
        #region Public Methods
		public ConnectionInfo Add(ConnectionInfo cInfo)
		{
			this.List.Add(cInfo);
			return cInfo;
		}
			
		public void AddRange(ConnectionInfo[] cInfo)
		{
			foreach (ConnectionInfo cI in cInfo)
			{
				List.Add(cI);
			}
		}
			
		public ConnectionInfo FindByConstantID(string id)
		{
			foreach (ConnectionInfo conI in List)
			{
				if (conI.ConstantID == id)
				{
					return conI;
				}
			}
				
			return null;
		}
			
		public Connection.ConnectionList Copy()
		{
			try
			{
                return (Connection.ConnectionList)this.MemberwiseClone();
			}
			catch (Exception)
			{
			}
				
			return null;
		}
			
		public new void Clear()
		{
			this.List.Clear();
		}
        #endregion
	}
}