using System;
using System.Collections;


namespace mRemoteNG.Credential
{
	public class CredentialList : CollectionBase
	{
        #region Public Properties
        public CredentialInfo this[object index]
		{
			get
			{
			    if (index is CredentialInfo)
                    return (CredentialInfo)index;
			    return (CredentialInfo) List[Convert.ToInt32(index)];
			}
		}
			
        public new int Count => List.Count;

	    #endregion
			
        #region Public Methods
		public CredentialInfo Add(CredentialInfo credentialInfo)
		{
			List.Add(credentialInfo);
			return credentialInfo;
		}
			
		public void AddRange(CredentialInfo[] cInfo)
		{
			foreach (CredentialInfo cI in cInfo)
			{
				List.Add(cI);
			}
		}
			
		public CredentialList Copy()
		{
			try
			{
                return (CredentialList)MemberwiseClone();
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