using System;
using System.Runtime.Versioning;
using mRemoteNG.Properties;

// ReSharper disable InconsistentNaming

namespace mRemoteNG.App.Info
{
    [SupportedOSPlatform("windows")]
    public static class UpdateChannelInfo
    {
        public const string STABLE = "Stable";
        public const string PREVIEW = "Preview";
        public const string NIGHTLY = "Nightly";
        public const string GITHUB = "GitHub";

        public const string StablePortable = "update-portable.txt";
        public const string PreviewPortable = "preview-update-portable.txt";
        public const string NightlyPortable = "nightly-update-portable.txt";

        public const string StableMsi = "update.txt";
        public const string PreviewMsi = "preview-update.txt";
        public const string NightlyMsi = "nightly-update.txt";

        private const string GITHUB_API_URI = "https://api.github.com/repos/robertpopa22/mRemoteNG/releases/latest";

        public static Uri GetUpdateChannelInfo()
        {
            string channel = IsValidChannel(Properties.OptionsUpdatesPage.Default.UpdateChannel)
                ? Properties.OptionsUpdatesPage.Default.UpdateChannel
                : GITHUB;

            if (channel == GITHUB)
                return new Uri(GITHUB_API_URI);

            return GetUpdateTxtUri(channel);
        }

        public static bool IsGitHubUri(Uri uri)
        {
            return uri.AbsoluteUri.Equals(GITHUB_API_URI, StringComparison.OrdinalIgnoreCase);
        }

        private static string GetChannelFileName(string channel)
        {
            return Runtime.IsPortableEdition
                ? GetChannelFileNamePortableEdition(channel)
                : GetChannelFileNameNormalEdition(channel);
        }

        private static string GetChannelFileNameNormalEdition(string channel)
        {
            switch (channel)
            {
                case STABLE:
                    return StableMsi;
                case PREVIEW:
                    return PreviewMsi;
                case NIGHTLY:
                    return NightlyMsi;
                default:
                    return StableMsi;
            }
        }

        private static string GetChannelFileNamePortableEdition(string channel)
        {
            switch (channel)
            {
                case STABLE:
                    return StablePortable;
                case PREVIEW:
                    return PreviewPortable;
                case NIGHTLY:
                    return NightlyPortable;
                default:
                    return StablePortable;
            }
        }

        private static Uri GetUpdateTxtUri(string channel)
        {
            return new Uri(new Uri(Properties.OptionsUpdatesPage.Default.UpdateAddress),
                           new Uri(GetChannelFileName(channel), UriKind.Relative));
        }

        private static bool IsValidChannel(string s)
        {
            return s.Equals(STABLE, StringComparison.Ordinal) || s.Equals(PREVIEW, StringComparison.Ordinal) || s.Equals(NIGHTLY, StringComparison.Ordinal) || s.Equals(GITHUB, StringComparison.Ordinal);
        }
    }
}