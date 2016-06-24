using System;
using System.Collections;
using System.Collections.Specialized;
using System.Linq;


namespace mRemoteNG.Credential
{
	public class CredentialList : CollectionBase, INotifyCollectionChanged
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

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        #endregion

        #region Public Methods
        public void Add(CredentialInfo credentialInfo)
        {
            List.Add(credentialInfo);
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, credentialInfo));
        }

	    public void AddRange(CredentialInfo[] cInfo)
		{
			foreach (CredentialInfo cI in cInfo)
			{
				List.Add(cI);
			}
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, cInfo));
		}

	    public void Remove(CredentialInfo credentialInfo)
	    {
	        foreach (CredentialInfo cred in List)
	        {
	            if (cred.Uuid != credentialInfo.Uuid) continue;
	            List.Remove(cred);
	            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, cred));
                return;
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
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, replacementCredentialInfo));
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
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }
        #endregion
	}
}