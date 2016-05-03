using System;
using System.Collections;


namespace mRemoteNG.Connection
{
	public class ConnectionList : CollectionBase
	{
        #region Public Properties
        public Connection.ConnectionInfo this[object Index]
		{
			get
			{
				if (Index is Connection.ConnectionInfo)
				{
					return (Connection.ConnectionInfo)Index;
				}
				else
				{
					return ((Connection.ConnectionInfo) (List[System.Convert.ToInt32(Index)]));
				}
			}
		}
			
        public new int Count
		{
			get { return List.Count; }
		}
        #endregion
			
        #region Public Methods
		public Connection.ConnectionInfo Add(Connection.ConnectionInfo cInfo)
		{
			this.List.Add(cInfo);
			return cInfo;
		}
			
		public void AddRange(Connection.ConnectionInfo[] cInfo)
		{
			foreach (Connection.ConnectionInfo cI in cInfo)
			{
				List.Add(cI);
			}
		}
			
		public Connection.ConnectionInfo FindByConstantID(string id)
		{
			foreach (Connection.ConnectionInfo conI in List)
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