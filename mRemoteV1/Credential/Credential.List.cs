using System;
using System.Collections;


namespace mRemoteNG.Credential
{
	public class List : CollectionBase
	{
        #region Public Properties
        public Credential.CredentialRecordImp this[object Index]
		{
			get
			{
				if (Index is Credential.CredentialRecordImp)
				{
                    return (Credential.CredentialRecordImp)Index;
				}
				else
				{
					return ((Credential.CredentialRecordImp) (List[System.Convert.ToInt32(Index)]));
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
		public Credential.CredentialRecordImp Add(Credential.CredentialRecordImp cInfo)
		{
			List.Add(cInfo);
			return cInfo;
		}
			
		public void AddRange(Credential.CredentialRecordImp[] cInfo)
		{
			foreach (Credential.CredentialRecordImp cI in cInfo)
			{
				List.Add(cI);
			}
		}
			
		public Credential.List Copy()
		{
			try
			{
                return (Credential.List)this.MemberwiseClone();
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
        #endregion
	}
}
