using System;
using System.Collections;

namespace mRemoteNG.Credential
{
    public class CredentialList : CollectionBase
    {
        #region Public Properties

        public CredentialInfo this[object Index]
        {
            get
            {
                if (Index is CredentialInfo)
                    return (CredentialInfo) Index;
                return (CredentialInfo) List[Convert.ToInt32(Index)];
            }
        }

        public new int Count
        {
            get { return List.Count; }
        }

        #endregion

        #region Public Methods

        public CredentialInfo Add(CredentialInfo credentialInfo)
        {
            List.Add(credentialInfo);
            return credentialInfo;
        }

        public void AddRange(CredentialInfo[] cInfo)
        {
            foreach (var cI in cInfo)
                List.Add(cI);
        }

        public CredentialList Copy()
        {
            try
            {
                return (CredentialList) MemberwiseClone();
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