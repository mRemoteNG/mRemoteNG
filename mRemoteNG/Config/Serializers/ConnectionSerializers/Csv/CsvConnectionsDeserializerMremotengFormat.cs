using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Connection.Protocol.Http;
using mRemoteNG.Connection.Protocol.RDP;
using mRemoteNG.Connection.Protocol.VNC;
using mRemoteNG.Container;
using mRemoteNG.Security;
using mRemoteNG.Tree;
using mRemoteNG.Tree.Root;

namespace mRemoteNG.Config.Serializers.ConnectionSerializers.Csv
{
    [SupportedOSPlatform("windows")]
    public class CsvConnectionsDeserializerMremotengFormat : IDeserializer<string, ConnectionTreeModel>
    {
        public ConnectionTreeModel Deserialize(string serializedData)
        {
            string[] lines = serializedData.Split(new[] {"\r\n", "\r", "\n"}, StringSplitOptions.RemoveEmptyEntries);
            List<string> csvHeaders = new();
            // used to map a connectioninfo to it's parent's GUID
            Dictionary<ConnectionInfo, string> parentMapping = new();

            char delimiter = ';';
            if (lines.Length > 0)
            {
                int countSemicolon = lines[0].Count(c => c == ';');
                int countComma = lines[0].Count(c => c == ',');

                if (countComma > countSemicolon)
                {
                    delimiter = ',';
                }
            }

            for (int lineNumber = 0; lineNumber < lines.Length; lineNumber++)
            {
                string[] line = ParseCsvLine(lines[lineNumber], delimiter);
                if (lineNumber == 0)
                    csvHeaders = line.ToList();
                else
                {
                    ConnectionInfo connectionInfo = ParseConnectionInfo(csvHeaders, line);
                    int parentIdx = csvHeaders.IndexOf("Parent");
                    parentMapping.Add(connectionInfo, parentIdx >= 0 ? line[parentIdx] : "");
                }
            }

            RootNodeInfo root = CreateTreeStructure(parentMapping);
            ApplyAutoSortRecursive(root);
            ConnectionTreeModel connectionTreeModel = new();
            connectionTreeModel.AddRootNode(root);
            return connectionTreeModel;
        }

        /// <summary>
        /// Parses a single CSV line respecting RFC 4180 double-quoted fields.
        /// </summary>
        private static string[] ParseCsvLine(string line, char delimiter)
        {
            List<string> fields = new();
            int i = 0;
            while (i <= line.Length)
            {
                if (i == line.Length)
                {
                    fields.Add("");
                    break;
                }

                if (line[i] == '"')
                {
                    // Quoted field
                    i++; // skip opening quote
                    int start = i;
                    System.Text.StringBuilder sb = new();
                    while (i < line.Length)
                    {
                        if (line[i] == '"')
                        {
                            if (i + 1 < line.Length && line[i + 1] == '"')
                            {
                                sb.Append(line, start, i - start);
                                sb.Append('"');
                                i += 2;
                                start = i;
                            }
                            else
                            {
                                // End of quoted field
                                sb.Append(line, start, i - start);
                                i++; // skip closing quote
                                break;
                            }
                        }
                        else
                        {
                            i++;
                        }
                    }

                    if (i == line.Length && (start <= line.Length - 1) && (sb.Length == 0 || line[i - 1] != '"'))
                    {
                        sb.Append(line, start, i - start);
                    }

                    fields.Add(sb.ToString());

                    // skip delimiter after quoted field
                    if (i < line.Length && line[i] == delimiter)
                        i++;
                }
                else
                {
                    // Unquoted field
                    int next = line.IndexOf(delimiter, i);
                    if (next == -1)
                    {
                        fields.Add(line[i..]);
                        break;
                    }
                    else
                    {
                        fields.Add(line[i..next]);
                        i = next + 1;
                    }
                }
            }

            return fields.ToArray();
        }

        private static RootNodeInfo CreateTreeStructure(Dictionary<ConnectionInfo, string> parentMapping)
        {
            RootNodeInfo root = new(RootNodeType.Connection);

            foreach (KeyValuePair<ConnectionInfo, string> node in parentMapping)
            {
                // no parent mapped, add to root
                if (string.IsNullOrEmpty(node.Value))
                {
                    root.AddChild(node.Key);
                    continue;
                }

                // search for parent in the list by GUID
                ContainerInfo? parent = parentMapping
                             .Keys
                             .OfType<ContainerInfo>()
                             .FirstOrDefault(info => info.ConstantID == node.Value);

                if (parent != null)
                {
                    parent.AddChild(node.Key);
                }
                else
                {
                    root.AddChild(node.Key);
                }
            }

            return root;
        }

        private static void ApplyAutoSortRecursive(ContainerInfo parent)
        {
            foreach (ContainerInfo childContainer in parent.Children.OfType<ContainerInfo>())
                ApplyAutoSortRecursive(childContainer);

            if (parent.AutoSort)
                parent.Sort();
        }

        private static ConnectionInfo ParseConnectionInfo(IList<string> headers, string[] connectionCsv)
        {
            // Pad short rows to prevent IndexOutOfRangeException when a CSV data row
            // has fewer columns than the header (e.g. user-created or truncated CSV).
            if (connectionCsv.Length < headers.Count)
            {
                var padded = new string[headers.Count];
                Array.Copy(connectionCsv, padded, connectionCsv.Length);
                for (int i = connectionCsv.Length; i < headers.Count; i++)
                    padded[i] = string.Empty;
                connectionCsv = padded;
            }

            var headerSet = new HashSet<string>(headers, StringComparer.Ordinal);
            TreeNodeType nodeType = headerSet.Contains("NodeType")
                ? Enum.Parse<TreeNodeType>(connectionCsv[headers.IndexOf("NodeType")], true)
                : TreeNodeType.Connection;

            string nodeId = headerSet.Contains("Id")
                ? connectionCsv[headers.IndexOf("Id")]
                : Guid.NewGuid().ToString();

            ConnectionInfo connectionRecord = nodeType == TreeNodeType.Connection
                ? new ConnectionInfo(nodeId)
                : new ContainerInfo(nodeId);

            connectionRecord.Name = headerSet.Contains("Name")
                ? connectionCsv[headers.IndexOf("Name")]
                : "";

            connectionRecord.Description = headerSet.Contains("Description")
                ? connectionCsv[headers.IndexOf("Description")]
                : "";

            connectionRecord.Icon = headerSet.Contains("Icon")
                ? connectionCsv[headers.IndexOf("Icon")]
                : "";

            connectionRecord.Panel = headerSet.Contains("Panel")
                ? connectionCsv[headers.IndexOf("Panel")]
                : "";

            connectionRecord.UserViaAPI = headerSet.Contains("UserViaAPI")
                ? connectionCsv[headers.IndexOf("UserViaAPI")]
                : "";

            connectionRecord.Username = headerSet.Contains("Username")
                ? connectionCsv[headers.IndexOf("Username")]
                : "";

            if (connectionRecord is ContainerInfo containerForPassword)
            {
                // For containers, the "Password" column maps to the folder protection password
                if (headerSet.Contains("ContainerPassword"))
                    containerForPassword.ContainerPassword = connectionCsv[headers.IndexOf("ContainerPassword")];
                else if (headerSet.Contains("Password"))
                    containerForPassword.ContainerPassword = connectionCsv[headers.IndexOf("Password")];
            }
            else
            {
                connectionRecord.Password = headerSet.Contains("Password")
                    ? connectionCsv[headers.IndexOf("Password")]
                    : "";
            }

            connectionRecord.Domain = headerSet.Contains("Domain")
                ? connectionCsv[headers.IndexOf("Domain")]
                : "";

            connectionRecord.Hostname = headerSet.Contains("Hostname")
                ? connectionCsv[headers.IndexOf("Hostname")]
                : "";

            connectionRecord.AlternativeAddress = headerSet.Contains("AlternativeAddress")
                ? connectionCsv[headers.IndexOf("AlternativeAddress")]
                : "";

            connectionRecord.VmId = headerSet.Contains("VmId")
                ? connectionCsv[headers.IndexOf("VmId")] : "";

            connectionRecord.SSHOptions =headerSet.Contains("SSHOptions")
                ? connectionCsv[headers.IndexOf("SSHOptions")]
                : "";

            connectionRecord.SSHTunnelConnectionName = headerSet.Contains("SSHTunnelConnectionName")
                ? connectionCsv[headers.IndexOf("SSHTunnelConnectionName")]
                : "";

            connectionRecord.PuttySession = headerSet.Contains("PuttySession")
                ? connectionCsv[headers.IndexOf("PuttySession")]
                : "";

            connectionRecord.LoadBalanceInfo = headerSet.Contains("LoadBalanceInfo")
                ? connectionCsv[headers.IndexOf("LoadBalanceInfo")]
                : "";

            connectionRecord.OpeningCommand = headerSet.Contains("OpeningCommand")
                ? connectionCsv[headers.IndexOf("OpeningCommand")]
                : "";

            connectionRecord.PreExtApp = headerSet.Contains("PreExtApp")
                ? connectionCsv[headers.IndexOf("PreExtApp")]
                : "";

            connectionRecord.PostExtApp =
                headerSet.Contains("PostExtApp")
                ? connectionCsv[headers.IndexOf("PostExtApp")]
                : "";

            connectionRecord.MacAddress =
                headerSet.Contains("MacAddress")
                ? connectionCsv[headers.IndexOf("MacAddress")]
                : "";

            connectionRecord.UserField =
                headerSet.Contains("UserField")
                ? connectionCsv[headers.IndexOf("UserField")]
                : "";

            for (int i = 1; i <= 10; i++)
            {
                string key = $"UserField{i}";
                if (headerSet.Contains(key))
                {
                    typeof(ConnectionInfo).GetProperty(key)?.SetValue(connectionRecord, connectionCsv[headers.IndexOf(key)]);
                }
            }

            connectionRecord.EnvironmentTags =
                headerSet.Contains("EnvironmentTags")
                ? connectionCsv[headers.IndexOf("EnvironmentTags")]
                : "";

            connectionRecord.ExtApp = headerSet.Contains("ExtApp")
                ? connectionCsv[headers.IndexOf("ExtApp")] : "";

            connectionRecord.TabColor = headerSet.Contains("TabColor")
                ? connectionCsv[headers.IndexOf("TabColor")] : "";

            connectionRecord.ConnectionFrameColor = headerSet.Contains("ConnectionFrameColor")
                ? Enum.TryParse(connectionCsv[headers.IndexOf("ConnectionFrameColor")], out ConnectionFrameColor cfColor)
                    ? cfColor : default
                : default;

            connectionRecord.RedirectDiskDrivesCustom = headerSet.Contains("RedirectDiskDrivesCustom")
                ? connectionCsv[headers.IndexOf("RedirectDiskDrivesCustom")] : "";

            connectionRecord.EC2InstanceId = headerSet.Contains("EC2InstanceId")
                ? connectionCsv[headers.IndexOf("EC2InstanceId")] : "";

            connectionRecord.EC2Region = headerSet.Contains("EC2Region")
                ? connectionCsv[headers.IndexOf("EC2Region")] : "";

            connectionRecord.VNCProxyUsername = headerSet.Contains("VNCProxyUsername")
                ? connectionCsv[headers.IndexOf("VNCProxyUsername")]
                : "";

            connectionRecord.VNCProxyPassword = headerSet.Contains("VNCProxyPassword")
                ? connectionCsv[headers.IndexOf("VNCProxyPassword")]
                : "";

            connectionRecord.RDGatewayUsername = headerSet.Contains("RDGatewayUsername")
                ? connectionCsv[headers.IndexOf("RDGatewayUsername")]
                : "";

            connectionRecord.RDGatewayPassword = headerSet.Contains("RDGatewayPassword")
                ? connectionCsv[headers.IndexOf("RDGatewayPassword")]
                : "";

            connectionRecord.RDGatewayDomain = headerSet.Contains("RDGatewayDomain")
                ? connectionCsv[headers.IndexOf("RDGatewayDomain")]
                : "";

            connectionRecord.RDGatewayHostname = headerSet.Contains("RDGatewayHostname")
                ? connectionCsv[headers.IndexOf("RDGatewayHostname")]
                : "";

            if (headerSet.Contains("RDGatewayExternalCredentialProvider"))
            {
                if (Enum.TryParse(connectionCsv[headers.IndexOf("RDGatewayExternalCredentialProvider")], out ExternalCredentialProvider value))
                    connectionRecord.RDGatewayExternalCredentialProvider = value;
            }

            connectionRecord.RDGatewayUserViaAPI = headerSet.Contains("RDGatewayUserViaAPI")
                ? connectionCsv[headers.IndexOf("RDGatewayUserViaAPI")]
                : "";


            connectionRecord.VNCProxyIP = headerSet.Contains("VNCProxyIP")
                ? connectionCsv[headers.IndexOf("VNCProxyIP")]
                : "";


            connectionRecord.RDPStartProgram = headerSet.Contains("RDPStartProgram")
                ? connectionCsv[headers.IndexOf("RDPStartProgram")]
                : "";

            connectionRecord.RDPStartProgramWorkDir = headerSet.Contains("RDPStartProgramWorkDir")
                ? connectionCsv[headers.IndexOf("RDPStartProgramWorkDir")]
                : "";

            if (headerSet.Contains("Protocol"))
            {
                if (Enum.TryParse(connectionCsv[headers.IndexOf("Protocol")], out ProtocolType protocolType))
                    connectionRecord.Protocol = protocolType;
            }

            if (headerSet.Contains("Port"))
            {
                if (int.TryParse(connectionCsv[headers.IndexOf("Port")], out int port))
                    connectionRecord.Port = port;
            }

            if (headerSet.Contains("ConnectToConsole"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("ConnectToConsole")], out bool useConsoleSession))
                    connectionRecord.UseConsoleSession = useConsoleSession;
            }

            if (headerSet.Contains("UseCredSsp"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("UseCredSsp")], out bool value))
                    connectionRecord.UseCredSsp = value;
            }

            if (headerSet.Contains("UseRestrictedAdmin"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("UseRestrictedAdmin")], out bool value))
                    connectionRecord.UseRestrictedAdmin = value;
            }
            if (headerSet.Contains("UseRCG"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("UseRCG")], out bool value))
                    connectionRecord.UseRCG = value;
            }


            if (headerSet.Contains("UseVmId"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("UseVmId")], out bool value))
                    connectionRecord.UseVmId = value;
            }

            if (headerSet.Contains("UseEnhancedMode"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("UseEnhancedMode")], out bool value))
                    connectionRecord.UseEnhancedMode = value;
            }

            if (headerSet.Contains("RenderingEngine"))
            {
                if (Enum.TryParse(connectionCsv[headers.IndexOf("RenderingEngine")], out HTTPBase.RenderingEngine value))
                    connectionRecord.RenderingEngine = value;
            }

            if (headerSet.Contains("RDPAuthenticationLevel"))
            {
                if (Enum.TryParse(connectionCsv[headers.IndexOf("RDPAuthenticationLevel")], out AuthenticationLevel value))
                    connectionRecord.RDPAuthenticationLevel = value;
            }

            if (headerSet.Contains("Colors"))
            {
                if (Enum.TryParse(connectionCsv[headers.IndexOf("Colors")], out RDPColors value))
                    connectionRecord.Colors = value;
            }

            if (headerSet.Contains("Resolution"))
            {
                if (Enum.TryParse(connectionCsv[headers.IndexOf("Resolution")], out RDPResolutions value))
                    connectionRecord.Resolution = value;
            }

            if (headerSet.Contains("AutomaticResize"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("AutomaticResize")], out bool value))
                    connectionRecord.AutomaticResize = value;
            }

            if (headerSet.Contains("DisplayWallpaper"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("DisplayWallpaper")], out bool value))
                    connectionRecord.DisplayWallpaper = value;
            }

            if (headerSet.Contains("DisplayThemes"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("DisplayThemes")], out bool value))
                    connectionRecord.DisplayThemes = value;
            }

            if (headerSet.Contains("EnableFontSmoothing"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("EnableFontSmoothing")], out bool value))
                    connectionRecord.EnableFontSmoothing = value;
            }

            if (headerSet.Contains("EnableDesktopComposition"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("EnableDesktopComposition")], out bool value))
                    connectionRecord.EnableDesktopComposition = value;
            }

            if (headerSet.Contains("DisableFullWindowDrag"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("DisableFullWindowDrag")], out bool value))
                    connectionRecord.DisableFullWindowDrag = value;
            }

            if (headerSet.Contains("DisableMenuAnimations"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("DisableMenuAnimations")], out bool value))
                    connectionRecord.DisableMenuAnimations = value;
            }

            if (headerSet.Contains("DisableCursorShadow"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("DisableCursorShadow")], out bool value))
                    connectionRecord.DisableCursorShadow = value;
            }

            if (headerSet.Contains("DisableCursorBlinking"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("DisableCursorBlinking")], out bool value))
                    connectionRecord.DisableCursorBlinking = value;
            }

            if (headerSet.Contains("CacheBitmaps"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("CacheBitmaps")], out bool value))
                    connectionRecord.CacheBitmaps = value;
            }

            if (headerSet.Contains("RedirectDiskDrives"))
            {
                if (Enum.TryParse(connectionCsv[headers.IndexOf("RedirectDiskDrives")], out RDPDiskDrives value))
                    connectionRecord.RedirectDiskDrives = value;
            }

            if (headerSet.Contains("RedirectPorts"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("RedirectPorts")], out bool value))
                    connectionRecord.RedirectPorts = value;
            }

            if (headerSet.Contains("RedirectPrinters"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("RedirectPrinters")], out bool value))
                    connectionRecord.RedirectPrinters = value;
            }

            if (headerSet.Contains("RedirectClipboard"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("RedirectClipboard")], out bool value))
                    connectionRecord.RedirectClipboard = value;
            }

            if (headerSet.Contains("RedirectSmartCards"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("RedirectSmartCards")], out bool value))
                    connectionRecord.RedirectSmartCards = value;
            }

            if (headerSet.Contains("RedirectSound"))
            {
                if (Enum.TryParse(connectionCsv[headers.IndexOf("RedirectSound")], out RDPSounds value))
                    connectionRecord.RedirectSound = value;
            }

            if (headerSet.Contains("RedirectAudioCapture"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("RedirectAudioCapture")], out bool value))
                    connectionRecord.RedirectAudioCapture = value;
            }

            if (headerSet.Contains("RedirectKeys"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("RedirectKeys")], out bool value))
                    connectionRecord.RedirectKeys = value;
            }

            if (headerSet.Contains("VNCCompression"))
            {
                if (Enum.TryParse(connectionCsv[headers.IndexOf("VNCCompression")], out ProtocolVNC.Compression value))
                    connectionRecord.VNCCompression = value;
            }

            if (headerSet.Contains("VNCEncoding"))
            {
                if (Enum.TryParse(connectionCsv[headers.IndexOf("VNCEncoding")], out ProtocolVNC.Encoding value))
                    connectionRecord.VNCEncoding = value;
            }

            if (headerSet.Contains("VNCAuthMode"))
            {
                if (Enum.TryParse(connectionCsv[headers.IndexOf("VNCAuthMode")], out ProtocolVNC.AuthMode value))
                    connectionRecord.VNCAuthMode = value;
            }

            if (headerSet.Contains("VNCProxyType"))
            {
                if (Enum.TryParse(connectionCsv[headers.IndexOf("VNCProxyType")], out ProtocolVNC.ProxyType value))
                    connectionRecord.VNCProxyType = value;
            }

            if (headerSet.Contains("VNCProxyPort"))
            {
                if (int.TryParse(connectionCsv[headers.IndexOf("VNCProxyPort")], out int value))
                    connectionRecord.VNCProxyPort = value;
            }

            if (headerSet.Contains("VNCColors"))
            {
                if (Enum.TryParse(connectionCsv[headers.IndexOf("VNCColors")], out ProtocolVNC.Colors value))
                    connectionRecord.VNCColors = value;
            }

            if (headerSet.Contains("VNCSmartSizeMode"))
            {
                if (Enum.TryParse(connectionCsv[headers.IndexOf("VNCSmartSizeMode")], out ProtocolVNC.SmartSizeMode value))
                    connectionRecord.VNCSmartSizeMode = value;
            }

            if (headerSet.Contains("VNCViewOnly"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("VNCViewOnly")], out bool value))
                    connectionRecord.VNCViewOnly = value;
            }

            if (headerSet.Contains("VNCClipboardRedirect"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("VNCClipboardRedirect")], out bool value))
                    connectionRecord.VNCClipboardRedirect = value;
            }

            if (headerSet.Contains("RDGatewayUsageMethod"))
            {
                if (Enum.TryParse(connectionCsv[headers.IndexOf("RDGatewayUsageMethod")], out RDGatewayUsageMethod value))
                    connectionRecord.RDGatewayUsageMethod = value;
            }

            if (headerSet.Contains("RDGatewayUseConnectionCredentials"))
            {
                if (Enum.TryParse(connectionCsv[headers.IndexOf("RDGatewayUseConnectionCredentials")], out RDGatewayUseConnectionCredentials value))
                    connectionRecord.RDGatewayUseConnectionCredentials = value;
            }

            if (headerSet.Contains("Favorite"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("Favorite")], out bool value))
                    connectionRecord.Favorite = value;
            }

            if (connectionRecord is ContainerInfo containerRecord && headerSet.Contains("AutoSort"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("AutoSort")], out bool value))
                    containerRecord.AutoSort = value;
            }

            if (headerSet.Contains("RdpVersion"))
            {
                if (Enum.TryParse(connectionCsv[headers.IndexOf("RdpVersion")], true, out RdpVersion version))
                    connectionRecord.RdpVersion = version;
            }
            if (headerSet.Contains("ExternalCredentialProvider"))
            {
                if (Enum.TryParse(connectionCsv[headers.IndexOf("ExternalCredentialProvider")], out ExternalCredentialProvider value))
                    connectionRecord.ExternalCredentialProvider = value;
            }
            if (headerSet.Contains("ExternalAddressProvider"))
            {
                if (Enum.TryParse(connectionCsv[headers.IndexOf("ExternalAddressProvider")], out ExternalAddressProvider value))
                    connectionRecord.ExternalAddressProvider = value;
            }

            if (headerSet.Contains("PrivateKeyPath"))
            {
                connectionRecord.PrivateKeyPath = connectionCsv[headers.IndexOf("PrivateKeyPath")];
            }

            if (headerSet.Contains("UsePersistentBrowser"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("UsePersistentBrowser")], out bool value))
                    connectionRecord.UsePersistentBrowser = value;
            }

            if (headerSet.Contains("ScriptErrorsSuppressed"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("ScriptErrorsSuppressed")], out bool value))
                    connectionRecord.ScriptErrorsSuppressed = value;
            }

            if (headerSet.Contains("DesktopScaleFactor"))
            {
                if (Enum.TryParse(connectionCsv[headers.IndexOf("DesktopScaleFactor")], true, out RDPDesktopScaleFactor value))
                    connectionRecord.DesktopScaleFactor = value;
            }

            if (headerSet.Contains("CredentialId"))
            {
                connectionRecord.CredentialId = connectionCsv[headers.IndexOf("CredentialId")];
            }

            if (headerSet.Contains("RDPSignScope"))
            {
                connectionRecord.RDPSignScope = connectionCsv[headers.IndexOf("RDPSignScope")];
            }

            if (headerSet.Contains("RDPSignature"))
            {
                connectionRecord.RDPSignature = connectionCsv[headers.IndexOf("RDPSignature")];
            }

            if (headerSet.Contains("RDPSizingMode"))
            {
                if (Enum.TryParse(connectionCsv[headers.IndexOf("RDPSizingMode")], out RDPSizingMode value))
                    connectionRecord.RDPSizingMode = value;
            }

            if (headerSet.Contains("ResolutionWidth"))
            {
                if (int.TryParse(connectionCsv[headers.IndexOf("ResolutionWidth")], out int value))
                    connectionRecord.ResolutionWidth = value;
            }

            if (headerSet.Contains("ResolutionHeight"))
            {
                if (int.TryParse(connectionCsv[headers.IndexOf("ResolutionHeight")], out int value))
                    connectionRecord.ResolutionHeight = value;
            }

            if (headerSet.Contains("RDPUseMultimon"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("RDPUseMultimon")], out bool value))
                    connectionRecord.RDPUseMultimon = value;
            }

            if (headerSet.Contains("Notes"))
            {
                connectionRecord.Notes = connectionCsv[headers.IndexOf("Notes")];
            }

            if (headerSet.Contains("RetryOnFirstConnect"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("RetryOnFirstConnect")], out bool value))
                    connectionRecord.RetryOnFirstConnect = value;
            }

            if (headerSet.Contains("WaitForIPAvailability"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("WaitForIPAvailability")], out bool value))
                    connectionRecord.WaitForIPAvailability = value;
            }

            if (headerSet.Contains("WaitForIPTimeout"))
            {
                if (int.TryParse(connectionCsv[headers.IndexOf("WaitForIPTimeout")], out int value))
                    connectionRecord.WaitForIPTimeout = value;
            }

            if (headerSet.Contains("ConnectionAddressPrimary"))
            {
                if (Enum.TryParse(connectionCsv[headers.IndexOf("ConnectionAddressPrimary")], out ConnectionAddressPrimary value))
                    connectionRecord.ConnectionAddressPrimary = value;
            }

            if (headerSet.Contains("IPAddress"))
            {
                connectionRecord.IPAddress = connectionCsv[headers.IndexOf("IPAddress")];
            }

            #region Inheritance

            if (headerSet.Contains("InheritCacheBitmaps"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritCacheBitmaps")], out bool value))
                    connectionRecord.Inheritance.CacheBitmaps = value;
            }

            if (headerSet.Contains("InheritColors"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritColors")], out bool value))
                    connectionRecord.Inheritance.Colors = value;
            }

            if (headerSet.Contains("InheritDescription"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritDescription")], out bool value))
                    connectionRecord.Inheritance.Description = value;
            }

            if (headerSet.Contains("InheritDisplayThemes"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritDisplayThemes")], out bool value))
                    connectionRecord.Inheritance.DisplayThemes = value;
            }

            if (headerSet.Contains("InheritDisplayWallpaper"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritDisplayWallpaper")], out bool value))
                    connectionRecord.Inheritance.DisplayWallpaper = value;
            }

            if (headerSet.Contains("InheritEnableFontSmoothing"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritEnableFontSmoothing")], out bool value))
                    connectionRecord.Inheritance.EnableFontSmoothing = value;
            }

            if (headerSet.Contains("InheritEnableDesktopComposition"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritEnableDesktopComposition")], out bool value))
                    connectionRecord.Inheritance.EnableDesktopComposition = value;
            }

            if (headerSet.Contains("InheritDisableFullWindowDrag"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritDisableFullWindowDrag")], out bool value))
                    connectionRecord.Inheritance.DisableFullWindowDrag = value;
            }

            if (headerSet.Contains("InheritDisableMenuAnimations"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritDisableMenuAnimations")], out bool value))
                    connectionRecord.Inheritance.DisableMenuAnimations = value;
            }

            if (headerSet.Contains("InheritDisableCursorShadow"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritDisableCursorShadow")], out bool value))
                    connectionRecord.Inheritance.DisableCursorShadow = value;
            }

            if (headerSet.Contains("InheritDisableCursorBlinking"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritDisableCursorBlinking")], out bool value))
                    connectionRecord.Inheritance.DisableCursorBlinking = value;
            }

            if (headerSet.Contains("InheritDomain"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritDomain")], out bool value))
                    connectionRecord.Inheritance.Domain = value;
            }

            if (headerSet.Contains("InheritIcon"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritIcon")], out bool value))
                    connectionRecord.Inheritance.Icon = value;
            }

            if (headerSet.Contains("InheritPanel"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritPanel")], out bool value))
                    connectionRecord.Inheritance.Panel = value;
            }

            if (headerSet.Contains("InheritTabColor"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritTabColor")], out bool value))
                    connectionRecord.Inheritance.TabColor = value;
            }

            if (headerSet.Contains("InheritConnectionFrameColor"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritConnectionFrameColor")], out bool value))
                    connectionRecord.Inheritance.ConnectionFrameColor = value;
            }

            if (headerSet.Contains("InheritColor"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritColor")], out bool value))
                    connectionRecord.Inheritance.Color = value;
            }

            if (headerSet.Contains("InheritPassword"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritPassword")], out bool value))
                    connectionRecord.Inheritance.Password = value;
            }

            if (headerSet.Contains("InheritPort"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritPort")], out bool value))
                    connectionRecord.Inheritance.Port = value;
            }

            if (headerSet.Contains("InheritProtocol"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritProtocol")], out bool value))
                    connectionRecord.Inheritance.Protocol = value;
            }

            if (headerSet.Contains("InheritSSHTunnelConnectionName"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritSSHTunnelConnectionName")], out bool value))
                    connectionRecord.Inheritance.SSHTunnelConnectionName = value;
            }

            if (headerSet.Contains("InheritOpeningCommand"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritOpeningCommand")], out bool value))
                    connectionRecord.Inheritance.OpeningCommand = value;
            }

            if (headerSet.Contains("InheritSSHOptions"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritSSHOptions")], out bool value))
                    connectionRecord.Inheritance.SSHOptions = value;
            }

            if (headerSet.Contains("InheritPuttySession"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritPuttySession")], out bool value))
                    connectionRecord.Inheritance.PuttySession = value;
            }

            if (headerSet.Contains("InheritRedirectDiskDrives"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritRedirectDiskDrives")], out bool value))
                    connectionRecord.Inheritance.RedirectDiskDrives = value;
            }
			
            if (headerSet.Contains("InheritRedirectDiskDrivesCustom"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritRedirectDiskDrivesCustom")], out bool value))
                    connectionRecord.Inheritance.RedirectDiskDrivesCustom = value;
            }

            if (headerSet.Contains("InheritRedirectKeys"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritRedirectKeys")], out bool value))
                    connectionRecord.Inheritance.RedirectKeys = value;
            }

            if (headerSet.Contains("InheritRedirectPorts"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritRedirectPorts")], out bool value))
                    connectionRecord.Inheritance.RedirectPorts = value;
            }

            if (headerSet.Contains("InheritRedirectPrinters"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritRedirectPrinters")], out bool value))
                    connectionRecord.Inheritance.RedirectPrinters = value;
            }

            if (headerSet.Contains("InheritRedirectClipboard"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritRedirectClipboard")], out bool value))
                    connectionRecord.Inheritance.RedirectClipboard = value;
            }

            if (headerSet.Contains("InheritRedirectSmartCards"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritRedirectSmartCards")], out bool value))
                    connectionRecord.Inheritance.RedirectSmartCards = value;
            }

            if (headerSet.Contains("InheritRedirectSound"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritRedirectSound")], out bool value))
                    connectionRecord.Inheritance.RedirectSound = value;
            }

            if (headerSet.Contains("InheritResolution"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritResolution")], out bool value))
                    connectionRecord.Inheritance.Resolution = value;
            }

            if (headerSet.Contains("InheritAutomaticResize"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritAutomaticResize")], out bool value))
                    connectionRecord.Inheritance.AutomaticResize = value;
            }

            if (headerSet.Contains("InheritUseConsoleSession"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritUseConsoleSession")], out bool value))
                    connectionRecord.Inheritance.UseConsoleSession = value;
            }

            if (headerSet.Contains("InheritUseCredSsp"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritUseCredSsp")], out bool value))
                    connectionRecord.Inheritance.UseCredSsp = value;
            }

            if (headerSet.Contains("InheritUseRestrictedAdmin"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritUseRestrictedAdmin")], out bool value))
                    connectionRecord.Inheritance.UseRestrictedAdmin = value;
            }

            if (headerSet.Contains("InheritUseRCG"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritUseRCG")], out bool value))
                    connectionRecord.Inheritance.UseRCG = value;
            }


            if (headerSet.Contains("InheritUseVmId"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritUseVmId")], out bool value))
                    connectionRecord.Inheritance.UseVmId = value;
            }

            if (headerSet.Contains("InheritUseEnhancedMode"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritUseEnhancedMode")], out bool value))
                    connectionRecord.Inheritance.UseEnhancedMode = value;
            }

            if (headerSet.Contains("InheritRenderingEngine"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritRenderingEngine")], out bool value))
                    connectionRecord.Inheritance.RenderingEngine = value;
            }

            if (headerSet.Contains("InheritExternalCredentialProvider"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritExternalCredentialProvider")], out bool value))
                    connectionRecord.Inheritance.ExternalCredentialProvider = value;
            }
            if (headerSet.Contains("InheritUserViaAPI"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritUserViaAPI")], out bool value))
                    connectionRecord.Inheritance.UserViaAPI = value;
            }

            if (headerSet.Contains("InheritUsername"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritUsername")], out bool value))
                    connectionRecord.Inheritance.Username = value;
            }

            if (headerSet.Contains("InheritVmId"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritVmId")], out bool value))
                    connectionRecord.Inheritance.VmId = value;
            }

            if (headerSet.Contains("InheritRDPAuthenticationLevel"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritRDPAuthenticationLevel")], out bool value))
                    connectionRecord.Inheritance.RDPAuthenticationLevel = value;
            }

            if (headerSet.Contains("InheritLoadBalanceInfo"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritLoadBalanceInfo")], out bool value))
                    connectionRecord.Inheritance.LoadBalanceInfo = value;
            }

            if (headerSet.Contains("InheritOpeningCommand"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritOpeningCommand")], out bool value))
                    connectionRecord.Inheritance.OpeningCommand = value;
            }

            if (headerSet.Contains("InheritPreExtApp"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritPreExtApp")], out bool value))
                    connectionRecord.Inheritance.PreExtApp = value;
            }

            if (headerSet.Contains("InheritPostExtApp"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritPostExtApp")], out bool value))
                    connectionRecord.Inheritance.PostExtApp = value;
            }

            if (headerSet.Contains("InheritMacAddress"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritMacAddress")], out bool value))
                    connectionRecord.Inheritance.MacAddress = value;
            }

            if (headerSet.Contains("InheritUserField"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritUserField")], out bool value))
                    connectionRecord.Inheritance.UserField = value;
            }

            for (int i = 1; i <= 10; i++)
            {
                string key = $"InheritUserField{i}";
                if (headerSet.Contains(key))
                {
                    if (bool.TryParse(connectionCsv[headers.IndexOf(key)], out bool value))
                    {
                        typeof(ConnectionInfoInheritance).GetProperty($"UserField{i}")?.SetValue(connectionRecord.Inheritance, value);
                    }
                }
            }

            if (headerSet.Contains("InheritHostname"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritHostname")], out bool value))
                    connectionRecord.Inheritance.Hostname = value;
            }

            if (headerSet.Contains("InheritAlternativeAddress"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritAlternativeAddress")], out bool value))
                    connectionRecord.Inheritance.AlternativeAddress = value;
            }

            if (headerSet.Contains("InheritEnvironmentTags"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritEnvironmentTags")], out bool value))
                    connectionRecord.Inheritance.EnvironmentTags = value;
            }

            if (headerSet.Contains("InheritFavorite"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritFavorite")], out bool value))
                    connectionRecord.Inheritance.Favorite = value;
            }

            if (headerSet.Contains("InheritAutoSort"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritAutoSort")], out bool value))
                    connectionRecord.Inheritance.AutoSort = value;
            }

            if (headerSet.Contains("InheritExtApp"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritExtApp")], out bool value))
                    connectionRecord.Inheritance.ExtApp = value;
            }

            if (headerSet.Contains("InheritVNCCompression"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritVNCCompression")], out bool value))
                    connectionRecord.Inheritance.VNCCompression = value;
            }

            if (headerSet.Contains("InheritVNCEncoding"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritVNCEncoding")], out bool value))
                    connectionRecord.Inheritance.VNCEncoding = value;
            }

            if (headerSet.Contains("InheritVNCAuthMode"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritVNCAuthMode")], out bool value))
                    connectionRecord.Inheritance.VNCAuthMode = value;
            }

            if (headerSet.Contains("InheritVNCProxyType"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritVNCProxyType")], out bool value))
                    connectionRecord.Inheritance.VNCProxyType = value;
            }

            if (headerSet.Contains("InheritVNCProxyIP"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritVNCProxyIP")], out bool value))
                    connectionRecord.Inheritance.VNCProxyIP = value;
            }

            if (headerSet.Contains("InheritVNCProxyPort"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritVNCProxyPort")], out bool value))
                    connectionRecord.Inheritance.VNCProxyPort = value;
            }

            if (headerSet.Contains("InheritVNCProxyUsername"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritVNCProxyUsername")], out bool value))
                    connectionRecord.Inheritance.VNCProxyUsername = value;
            }

            if (headerSet.Contains("InheritVNCProxyPassword"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritVNCProxyPassword")], out bool value))
                    connectionRecord.Inheritance.VNCProxyPassword = value;
            }

            if (headerSet.Contains("InheritVNCColors"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritVNCColors")], out bool value))
                    connectionRecord.Inheritance.VNCColors = value;
            }

            if (headerSet.Contains("InheritVNCSmartSizeMode"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritVNCSmartSizeMode")], out bool value))
                    connectionRecord.Inheritance.VNCSmartSizeMode = value;
            }

            if (headerSet.Contains("InheritVNCViewOnly"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritVNCViewOnly")], out bool value))
                    connectionRecord.Inheritance.VNCViewOnly = value;
            }

            if (headerSet.Contains("InheritVNCClipboardRedirect"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritVNCClipboardRedirect")], out bool value))
                    connectionRecord.Inheritance.VNCClipboardRedirect = value;
            }

            if (headerSet.Contains("InheritRDGatewayUsageMethod"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritRDGatewayUsageMethod")], out bool value))
                    connectionRecord.Inheritance.RDGatewayUsageMethod = value;
            }

            if (headerSet.Contains("InheritRDGatewayHostname"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritRDGatewayHostname")], out bool value))
                    connectionRecord.Inheritance.RDGatewayHostname = value;
            }

            if (headerSet.Contains("InheritRDGatewayUseConnectionCredentials"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritRDGatewayUseConnectionCredentials")],
                                  out bool value))
                    connectionRecord.Inheritance.RDGatewayUseConnectionCredentials = value;
            }

            if (headerSet.Contains("InheritRDGatewayUsername"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritRDGatewayUsername")], out bool value))
                    connectionRecord.Inheritance.RDGatewayUsername = value;
            }

            if (headerSet.Contains("InheritRDGatewayPassword"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritRDGatewayPassword")], out bool value))
                    connectionRecord.Inheritance.RDGatewayPassword = value;
            }

            if (headerSet.Contains("InheritRDGatewayDomain"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritRDGatewayDomain")], out bool value))
                    connectionRecord.Inheritance.RDGatewayDomain = value;
            }

            if (headerSet.Contains("InheritRDGatewayExternalCredentialProvider"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritRDGatewayExternalCredentialProvider")], out bool value))
                    connectionRecord.Inheritance.RDGatewayExternalCredentialProvider = value;
            }
            if (headerSet.Contains("InheritRDGatewayUserViaAPI"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritRDGatewayUserViaAPI")], out bool value))
                    connectionRecord.Inheritance.RDGatewayUserViaAPI = value;
            }


            if (headerSet.Contains("InheritRDPAlertIdleTimeout"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritRDPAlertIdleTimeout")], out bool value))
                    connectionRecord.Inheritance.RDPAlertIdleTimeout = value;
            }

            if (headerSet.Contains("InheritRDPMinutesToIdleTimeout"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritRDPMinutesToIdleTimeout")], out bool value))
                    connectionRecord.Inheritance.RDPMinutesToIdleTimeout = value;
            }

            if (headerSet.Contains("InheritSoundQuality"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritSoundQuality")], out bool value))
                    connectionRecord.Inheritance.SoundQuality = value;
            }

            if (headerSet.Contains("InheritRedirectAudioCapture"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritRedirectAudioCapture")], out bool value))
                    connectionRecord.Inheritance.RedirectAudioCapture = value;
            }

            if (headerSet.Contains("InheritRdpVersion"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritRdpVersion")], out bool value))
                    connectionRecord.Inheritance.RdpVersion = value;
            }

            if (headerSet.Contains("InheritPrivateKeyPath"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritPrivateKeyPath")], out bool value))
                    connectionRecord.Inheritance.PrivateKeyPath = value;
            }

            if (headerSet.Contains("InheritDesktopScaleFactor"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritDesktopScaleFactor")], out bool value))
                    connectionRecord.Inheritance.DesktopScaleFactor = value;
            }

            if (headerSet.Contains("InheritScriptErrorsSuppressed"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritScriptErrorsSuppressed")], out bool value))
                    connectionRecord.Inheritance.ScriptErrorsSuppressed = value;
            }

            if (headerSet.Contains("InheritIPAddress"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritIPAddress")], out bool value))
                    connectionRecord.Inheritance.IPAddress = value;
            }

            if (headerSet.Contains("InheritConnectionAddressPrimary"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritConnectionAddressPrimary")], out bool value))
                    connectionRecord.Inheritance.ConnectionAddressPrimary = value;
            }

            if (headerSet.Contains("InheritRDPSignScope"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritRDPSignScope")], out bool value))
                    connectionRecord.Inheritance.RDPSignScope = value;
            }

            if (headerSet.Contains("InheritRDPSignature"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritRDPSignature")], out bool value))
                    connectionRecord.Inheritance.RDPSignature = value;
            }

            if (headerSet.Contains("InheritRDPSizingMode"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritRDPSizingMode")], out bool value))
                    connectionRecord.Inheritance.RDPSizingMode = value;
            }

            if (headerSet.Contains("InheritResolutionWidth"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritResolutionWidth")], out bool value))
                    connectionRecord.Inheritance.ResolutionWidth = value;
            }

            if (headerSet.Contains("InheritResolutionHeight"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritResolutionHeight")], out bool value))
                    connectionRecord.Inheritance.ResolutionHeight = value;
            }

            if (headerSet.Contains("InheritRDPUseMultimon"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritRDPUseMultimon")], out bool value))
                    connectionRecord.Inheritance.RDPUseMultimon = value;
            }

            if (headerSet.Contains("InheritNotes"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritNotes")], out bool value))
                    connectionRecord.Inheritance.Notes = value;
            }

            if (headerSet.Contains("InheritRetryOnFirstConnect"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritRetryOnFirstConnect")], out bool value))
                    connectionRecord.Inheritance.RetryOnFirstConnect = value;
            }

            if (headerSet.Contains("InheritWaitForIPAvailability"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritWaitForIPAvailability")], out bool value))
                    connectionRecord.Inheritance.WaitForIPAvailability = value;
            }

            if (headerSet.Contains("InheritWaitForIPTimeout"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritWaitForIPTimeout")], out bool value))
                    connectionRecord.Inheritance.WaitForIPTimeout = value;
            }

            #endregion

            return connectionRecord;
        }
    }
}