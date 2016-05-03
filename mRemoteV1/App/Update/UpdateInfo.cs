using System;
using System.Collections.Generic;
using System.Text;

namespace mRemoteNG.App.Update
{
    public class UpdateInfo
    {
        public bool IsValid { get; set; }
        public Version Version { get; set; }
        public Uri DownloadAddress { get; set; }
        public string UpdateFilePath { get; set; }
        public Uri ChangeLogAddress { get; set; }
        public Uri ImageAddress { get; set; }
        public Uri ImageLinkAddress { get; set; }
        public string CertificateThumbprint { get; set; }

        public static UpdateInfo FromString(string input)
        {
            UpdateInfo newInfo = new UpdateInfo();
            if (string.IsNullOrEmpty(input))
            {
                newInfo.IsValid = false;
            }
            else
            {
                UpdateFile updateFile = new UpdateFile(input);
                newInfo.Version = updateFile.GetVersion("Version");
                newInfo.DownloadAddress = updateFile.GetUri("dURL");
                newInfo.ChangeLogAddress = updateFile.GetUri("clURL");
                newInfo.ImageAddress = updateFile.GetUri("imgURL");
                newInfo.ImageLinkAddress = updateFile.GetUri("imgURLLink");
                newInfo.CertificateThumbprint = updateFile.GetThumbprint("CertificateThumbprint");
                newInfo.IsValid = true;
            }
            return newInfo;
        }
    }
}