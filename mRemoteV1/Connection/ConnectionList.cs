using System;
using System.Collections;


namespace mRemoteNG.Connection
{
	public class ConnectionList : CollectionBase
	{
        #region Public Properties
        public ConnectionRecordImp this[object Index]
		{
			get
			{
				if (Index is ConnectionRecordImp)
				{
					return (ConnectionRecordImp)Index;
				}
				else
				{
					return ((ConnectionRecordImp) (List[System.Convert.ToInt32(Index)]));
				}
			}
		}
			
        public new int Count
		{
			get
            { 
                return List.Count; 
            }
		}
        #endregion
			
        #region Public Methods
        public ConnectionRecord Add(ConnectionRecord cInfo)
		{
			this.List.Add(cInfo);
			return cInfo;
		}

        public void AddRange(ConnectionRecord[] cInfo)
		{
            foreach (ConnectionRecord cI in cInfo)
			{
				List.Add(cI);
			}
		}

        public ConnectionRecord FindByConstantID(string id)
		{
			foreach (ConnectionRecord conI in List)
			{
				if (conI.MetaData.ConstantID == id)
				{
					return conI;
				}
			}
				
			return null;
		}
			
		//Public Function Find(ByVal cInfo As Connection.Info)
		//    For Each cI As Connection.Info In List
			
		//    Next
		//End Function
			
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