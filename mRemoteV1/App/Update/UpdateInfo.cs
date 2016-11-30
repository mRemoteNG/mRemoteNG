using System;

namespace mRemoteNG.App.Update
{
    public class UpdateInfo
    {
        public bool IsValid { get; private set; }
        public Version Version { get; private set; }
        public Uri DownloadAddress { get; private set; }
        public string UpdateFilePath { get; set; }
        public Uri ChangeLogAddress { get; private set; }
        public Uri ImageAddress { get; private set; }
        public Uri ImageLinkAddress { get; private set; }
        public string CertificateThumbprint { get; private set; }
        public string FileName { get; private set; }

        public static UpdateInfo FromString(string input)
        {
            var newInfo = new UpdateInfo();
            if (string.IsNullOrEmpty(input))
            {
                newInfo.IsValid = false;
            }
            else
            {
                var updateFile = new UpdateFile(input);
                newInfo.Version = updateFile.GetVersion("Version");
                newInfo.DownloadAddress = updateFile.GetUri("dURL");
                newInfo.ChangeLogAddress = updateFile.GetUri("clURL");
                newInfo.ImageAddress = updateFile.GetUri("imgURL");
                newInfo.ImageLinkAddress = updateFile.GetUri("imgURLLink");
                newInfo.CertificateThumbprint = updateFile.GetThumbprint("CertificateThumbprint");
                newInfo.FileName = updateFile.GetFileName();
                newInfo.IsValid = true;
            }
            return newInfo;
        }
    }
}