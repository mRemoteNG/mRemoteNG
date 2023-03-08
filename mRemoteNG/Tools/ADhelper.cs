using System;
using System.Collections;
using System.DirectoryServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace mRemoteNG.Tools
{
    [SupportedOSPlatform("windows")]
    public class AdHelper
    {
        private DirectoryEntry _dEntry;

        public AdHelper(string domain)
        {
            Children = new Hashtable();
            Domain = domain;
        }

        public Hashtable Children { get; }

        private string Domain { get; }

        public void GetChildEntries(string adPath = "")
        {
            _dEntry = adPath.Length <= 0
                ? Domain.Length <= 0 ? new DirectoryEntry() : new DirectoryEntry("LDAP://" + Domain)
                : new DirectoryEntry(adPath);
            try
            {
                foreach (DirectoryEntry child in _dEntry.Children)
                    Children.Add(child.Name, child.Path);
            }
            catch (COMException ex)
            {
                if (ex.Message.ToLower().Equals("the server is not operational"))
                    throw new Exception("Could not find AD Server", ex);
            }
        }
    }
}