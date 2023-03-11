using System;
using System.Diagnostics;
using mRemoteNG.Connection.Protocol;
using System.IO;
using System.Runtime.Versioning;

namespace mRemoteNG.Tools
{
    [SupportedOSPlatform("windows")]
    public class PuttyTypeDetector
    {
        public static PuttyType GetPuttyType()
        {
            return GetPuttyType(PuttyBase.PuttyPath);
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

        private static bool IsPutty(string filename)
        {
            return !string.IsNullOrEmpty(filename) && File.Exists(filename) &&
                   Convert.ToBoolean(FileVersionInfo.GetVersionInfo(filename).InternalName.Contains("PuTTY"));
        }

        private static bool IsPuttyNg(string filename)
        {
            return !string.IsNullOrEmpty(filename) && File.Exists(filename) &&
                   Convert.ToBoolean(FileVersionInfo.GetVersionInfo(filename).InternalName.Contains("PuTTYNG"));
        }

        private static bool IsKitty(string filename)
        {
            return !string.IsNullOrEmpty(filename) && File.Exists(filename) && Convert.ToBoolean(
                                                                                                 FileVersionInfo
                                                                                                     .GetVersionInfo(filename)
                                                                                                     .InternalName
                                                                                                     .Contains("PuTTY") &&
                                                                                                 FileVersionInfo
                                                                                                     .GetVersionInfo(filename)
                                                                                                     .Comments
                                                                                                     .Contains("KiTTY"));
        }

        private static bool IsXming(string filename)
        {
            return !string.IsNullOrEmpty(filename) && File.Exists(filename) && Convert.ToBoolean(
                                                                                                 FileVersionInfo
                                                                                                     .GetVersionInfo(filename)
                                                                                                     .InternalName
                                                                                                     .Contains("PuTTY") &&
                                                                                                 FileVersionInfo
                                                                                                     .GetVersionInfo(filename)
                                                                                                     .ProductVersion
                                                                                                     .Contains("Xming"));
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