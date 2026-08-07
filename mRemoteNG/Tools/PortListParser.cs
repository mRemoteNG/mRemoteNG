using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace mRemoteNG.Tools
{
    /// <summary>
    /// Parses the port scanner's custom port field: a comma/semicolon/space separated list of
    /// individual ports and port ranges, e.g. <c>22, 80, 443, 3389, 8000-8100</c>.
    /// </summary>
    public static class PortListParser
    {
        public const int MinPort = 1;
        public const int MaxPort = 65535;

        /// <summary>Example text shown to the user when the field is empty or unparsable.</summary>
        public const string SyntaxHint = "Enter at least one port, e.g. 22, 80, 443, 3389, 8000-8100";

        /// <summary>
        /// Attempts to parse <paramref name="portListText"/>. On success <paramref name="ports"/> is
        /// sorted and free of duplicates; on failure <paramref name="error"/> explains why.
        /// </summary>
        public static bool TryParse(string? portListText, out List<int> ports, out string error)
        {
            ports = [];
            error = string.Empty;

            SortedSet<int> parsed = [];
            foreach (string part in (portListText ?? string.Empty).Split([',', ';', ' '],
                                                                        StringSplitOptions.RemoveEmptyEntries |
                                                                        StringSplitOptions.TrimEntries))
            {
                int dashIndex = part.IndexOf('-');
                if (dashIndex < 0)
                {
                    if (!TryParsePort(part, out int port, out error))
                        return false;

                    parsed.Add(port);
                    continue;
                }

                if (!TryParsePort(part[..dashIndex].Trim(), out int rangeStart, out error) ||
                    !TryParsePort(part[(dashIndex + 1)..].Trim(), out int rangeEnd, out error))
                    return false;

                // Accept the endpoints in either order.
                if (rangeStart > rangeEnd)
                    (rangeStart, rangeEnd) = (rangeEnd, rangeStart);

                for (int port = rangeStart; port <= rangeEnd; port++)
                    parsed.Add(port);
            }

            if (parsed.Count == 0)
            {
                error = SyntaxHint;
                return false;
            }

            ports = [.. parsed];
            return true;
        }

        /// <summary>Every port from <see cref="MinPort"/> to <see cref="MaxPort"/>.</summary>
        public static List<int> AllPorts() => [.. Enumerable.Range(MinPort, MaxPort - MinPort + 1)];

        private static bool TryParsePort(string text, out int port, out string error)
        {
            if (int.TryParse(text, NumberStyles.None, CultureInfo.InvariantCulture, out port) &&
                port is >= MinPort and <= MaxPort)
            {
                error = string.Empty;
                return true;
            }

            error = $"'{text}' is not a valid port; ports must be between {MinPort} and {MaxPort}.";
            return false;
        }
    }
}
