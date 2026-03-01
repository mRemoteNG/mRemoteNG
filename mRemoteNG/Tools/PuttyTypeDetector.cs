using System;
using System.Diagnostics;
using mRemoteNG.Connection.Protocol;
using System.IO;
using System.Runtime.Versioning;

namespace mRemoteNG.Tools
{
    [SupportedOSPlatform("windows")]
    public static class PuttyTypeDetector
    {
        public static PuttyType GetPuttyType()
        {
            return GetPuttyType(PuttyBase.PuttyPath ?? string.Empty);
        }

        public static PuttyType GetPuttyType(string filename)
        {
            if (IsPuttyNg(filename))
            {
                return PuttyType.PuttyNg;
            }

            if (IsKitty(filename))
            {
                return PuttyType.Kitty;
            }

            if (IsXming(filename))
            {
                return PuttyType.Xming;
            }

            // Check this last
            if (IsPutty(filename))
            {
                return PuttyType.Putty;
            }

            return PuttyType.Unknown;
        }

        public static Version GetPuttyVersion(string filename)
        {
            if (string.IsNullOrEmpty(filename) || !File.Exists(filename))
            {
                return new Version(0, 0);
            }

            try
            {
                var versionInfo = FileVersionInfo.GetVersionInfo(filename);
                return new Version(versionInfo.FileMajorPart, versionInfo.FileMinorPart, versionInfo.FileBuildPart, versionInfo.FilePrivatePart);
            }
            catch
            {
                return new Version(0, 0);
            }
        }

        private static bool IsPutty(string filename)
        {
            return !string.IsNullOrEmpty(filename) && File.Exists(filename) &&
                   (FileVersionInfo.GetVersionInfo(filename).InternalName?.Contains("PuTTY", StringComparison.Ordinal) == true);
        }

        private static bool IsPuttyNg(string filename)
        {
            return !string.IsNullOrEmpty(filename) && File.Exists(filename) &&
                   (FileVersionInfo.GetVersionInfo(filename).InternalName?.Contains("PuTTYNG", StringComparison.Ordinal) == true);
        }

        private static bool IsKitty(string filename)
        {
            if (string.IsNullOrEmpty(filename) || !File.Exists(filename))
                return false;
            var versionInfo = FileVersionInfo.GetVersionInfo(filename);
            return versionInfo.InternalName?.Contains("PuTTY", StringComparison.Ordinal) == true &&
                   versionInfo.Comments?.Contains("KiTTY", StringComparison.Ordinal) == true;
        }

        private static bool IsXming(string filename)
        {
            if (string.IsNullOrEmpty(filename) || !File.Exists(filename))
                return false;
            var versionInfo = FileVersionInfo.GetVersionInfo(filename);
            return versionInfo.InternalName?.Contains("PuTTY", StringComparison.Ordinal) == true &&
                   versionInfo.ProductVersion?.Contains("Xming", StringComparison.Ordinal) == true;
        }

        public enum PuttyType
        {
            Unknown = 0,
            Putty,
            PuttyNg,
            Kitty,
            Xming
        }
    }
}