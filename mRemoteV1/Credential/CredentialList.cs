using System;
using System.Collections;
using System.Linq;


namespace mRemoteNG.Credential
{
	public class CredentialList : CollectionBase
	{
        #region Public Properties
        public CredentialInfo this[object index]
		{
			get
			{
			    var info = index as CredentialInfo;
			    if (info != null)
                    return info;
			    return (CredentialInfo) List[Convert.ToInt32(index)];
			}
		}
			
        public new int Count => List.Count;

	    #endregion
			
        #region Public Methods
		public void Add(CredentialInfo credentialInfo)
		{
			List.Add(credentialInfo);
		}
			
		public void AddRange(CredentialInfo[] cInfo)
		{
			foreach (CredentialInfo cI in cInfo)
			{
				List.Add(cI);
			}
		}

	    public void Remove(CredentialInfo credentialInfo)
	    {
	        foreach (CredentialInfo cred in List)
	        {
	            if (cred.Uuid == credentialInfo.Uuid)
                    List.Remove(cred);
	        }
	    }

	    public bool Contains(CredentialInfo targetCredentialinfo)
	    {
	        return List.Cast<CredentialInfo>().Any(credential => credential.Uuid == targetCredentialinfo.Uuid);
	    }

        public void Replace(CredentialInfo replacementCredentialInfo)
        {
            if (Contains(replacementCredentialInfo))
                Remove(replacementCredentialInfo);
            Add(replacementCredentialInfo);
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