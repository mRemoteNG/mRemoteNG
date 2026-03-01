using System;
using System.Text.Json;

// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace mRemoteNG.App.Update
{
    public class UpdateInfo
    {
        public bool IsValid { get; private set; }
        public bool IsGitHubSource { get; private set; }
        public Version? Version { get; private set; }
        public Uri? DownloadAddress { get; private set; }
        public Uri? ReleasePageUrl { get; private set; }
        public string? UpdateFilePath { get; set; }
        public Uri? ChangeLogAddress { get; private set; }
        public string? ChangeLogBody { get; private set; }
        public Uri? ImageAddress { get; private set; }
        public Uri? ImageLinkAddress { get; private set; }
#if !PORTABLE
        public string? CertificateThumbprint { get; private set; }
#endif
        // ReSharper disable once MemberCanBePrivate.Global
        public string? FileName { get; set; }
        public string? Checksum { get; private set; }

        public static UpdateInfo FromGitHubJson(string json)
        {
            UpdateInfo newInfo = new() { IsGitHubSource = true };
            if (string.IsNullOrEmpty(json))
                return newInfo;

            try
            {
                using JsonDocument doc = JsonDocument.Parse(json);
                JsonElement root = doc.RootElement;

                if (root.TryGetProperty("tag_name", out JsonElement tagEl))
                {
                    string? tag = tagEl.GetString()?.TrimStart('v');
                    if (!string.IsNullOrEmpty(tag) && Version.TryParse(tag, out Version? ver))
                        newInfo.Version = ver;
                }

                if (root.TryGetProperty("html_url", out JsonElement htmlUrlEl))
                {
                    string? htmlUrl = htmlUrlEl.GetString();
                    if (!string.IsNullOrEmpty(htmlUrl) && Uri.TryCreate(htmlUrl, UriKind.Absolute, out Uri? releaseUri))
                    {
                        newInfo.ChangeLogAddress = releaseUri;
                        newInfo.ReleasePageUrl = releaseUri;
                    }
                }

                if (root.TryGetProperty("body", out JsonElement bodyEl))
                    newInfo.ChangeLogBody = bodyEl.GetString();

                newInfo.IsValid = newInfo.Version != null;
            }
            catch
            {
                newInfo.IsValid = false;
            }

            return newInfo;
        }

        public static UpdateInfo FromString(string input)
        {
            UpdateInfo newInfo = new();
            if (string.IsNullOrEmpty(input))
            {
                newInfo.IsValid = false;
            }
            else
            {
                UpdateFile updateFile = new(input);
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
            if (Version is null || string.IsNullOrEmpty(Version.ToString()))
                return false;
            if (DownloadAddress is null || string.IsNullOrEmpty(DownloadAddress.AbsoluteUri))
                return false;
            if (ChangeLogAddress is null || string.IsNullOrEmpty(ChangeLogAddress.AbsoluteUri))
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