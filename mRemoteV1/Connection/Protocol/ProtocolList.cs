using System;
using System.Collections;


namespace mRemoteNG.Connection.Protocol
{
	public class ProtocolList : CollectionBase
	{
        #region Public Properties
        public ProtocolBase this[object Index]
		{
			get
			{
				if (Index is ProtocolBase)
                    return (ProtocolBase)Index;
				else
					return ((ProtocolBase) (List[Convert.ToInt32(Index)]));
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
		public ProtocolBase Add(ProtocolBase cProt)
		{
			this.List.Add(cProt);
			return cProt;
		}
				
		public void AddRange(ProtocolBase[] cProt)
		{
			foreach (ProtocolBase cP in cProt)
			{
				List.Add(cP);
			}
		}
				
		public void Remove(ProtocolBase cProt)
		{
			try
			{
				this.List.Remove(cProt);
			}
			catch (Exception)
			{
			}
		}
				
		public new void Clear()
		{
			this.List.Clear();
		}
        #endregion
	}
}