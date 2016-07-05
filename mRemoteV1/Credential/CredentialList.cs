using System;
using System.Collections;
using System.Collections.Specialized;
using System.Linq;


namespace mRemoteNG.Credential
{
	public class CredentialList : CollectionBase, INotifyCollectionChanged
	{
	    public CredentialInfo this[int index]
	    {
	        get
	        {
                if (index < Count)
                    return (CredentialInfo)List[Convert.ToInt32(index)];
	            return null;
	        }
	    }

        public CredentialInfo this[CredentialInfo index]
		{
			get
			{
			    if (index != null && Contains(index))
                    return (CredentialInfo)List[IndexOf(index.Uuid)];
			    return null;
			}
		}
			
        public new int Count => List.Count;

        public event NotifyCollectionChangedEventHandler CollectionChanged;


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
                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, cInfo));
            }
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

	    public int IndexOf(string uuid)
	    {
	        var index = -1;
	        for (var i=0; i < List.Count; i++)
	        {
	            if (this[i].Uuid != uuid) continue;
	            index = i;
	            break;
	        }
            return index;
	    }

        public void Replace(CredentialInfo replacementCredentialInfo)
        {
            var targetCredentialIndex = IndexOf(replacementCredentialInfo.Uuid);
            if (targetCredentialIndex < 0) return;
            var oldCredentialItem = List[targetCredentialIndex];
            List[targetCredentialIndex] = replacementCredentialInfo;
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, replacementCredentialInfo, oldCredentialItem, targetCredentialIndex));
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