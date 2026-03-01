using System.Runtime.Versioning;
using mRemoteNG.Connection;

namespace mRemoteNG.Tools
{
    [SupportedOSPlatform("windows")]
    public static class ConnectionNameFormatter
    {
        /// <summary>
        /// Returns the display name for a connection, resolving any tokens
        /// (e.g. %HOSTNAME%, %PORT%) embedded in the Name field.
        /// If the name contains no tokens, the raw name is returned as-is.
        /// </summary>
        public static string FormatName(ConnectionInfo connectionInfo)
        {
            string name = connectionInfo.Name;
            if (!name.Contains('%'))
                return name;

            var parser = new ExternalToolArgumentParser(connectionInfo);
            return parser.ParseArguments(name, escapeForShell: false);
        }
    }
}
