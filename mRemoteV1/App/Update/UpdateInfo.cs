using System;
// ReSharper disable UnusedAutoPropertyAccessor.Local

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
#if !PORTABLE
        public string CertificateThumbprint { get; private set; }
#endif
        // ReSharper disable once MemberCanBePrivate.Global
        public string FileName { get; set; }
        public string Checksum { get; private set; }

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
                newInfo.Version = updateFile.GetVersion();
                newInfo.DownloadAddress = updateFile.GetUri("dURL");
                newInfo.ChangeLogAddress = updateFile.GetUri("clURL");
#if false
                newInfo.ImageAddress = updateFile.GetUri("imgURL");
                newInfo.ImageLinkAddress = updateFile.GetUri("imgURLLink");
#endif
#if !PORTABLE
                newInfo.CertificateThumbprint = updateFile.GetThumbprint();
#endif
                newInfo.FileName = updateFile.GetFileName();
                newInfo.Checksum = updateFile.GetChecksum();
                newInfo.IsValid = newInfo.CheckIfValid();
            }
            return newInfo;
        }

        public bool CheckIfValid()
        {
            if (string.IsNullOrEmpty(Version.ToString()))
                return false;
            if(string.IsNullOrEmpty(DownloadAddress.AbsoluteUri))
                return false;
            if (string.IsNullOrEmpty(ChangeLogAddress.AbsoluteUri))
                return false;
#if false            
            if (string.IsNullOrEmpty(ImageAddress.AbsoluteUri))
                return false;
            if (string.IsNullOrEmpty(ImageLinkAddress.AbsoluteUri))
                return false;
#endif
#if !PORTABLE
            if (string.IsNullOrEmpty(CertificateThumbprint))
                return false;
#endif
            if (string.IsNullOrEmpty(FileName))
                return false;
            // ReSharper disable once ConvertIfStatementToReturnStatement
            if (string.IsNullOrEmpty(Checksum))
                return false;

            return true;
        }
    }
}