using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Xml;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Container;
using mRemoteNG.Security;
using mRemoteNG.Tree;
using mRemoteNG.Tree.Root;

namespace mRemoteNG.Config.Serializers.MiscSerializers
{
    /// <summary>
    /// Reads a Royal TS / Royal TSX document (.rtsz). The root element holds a flat list of
    /// objects, each with its own ID and the ParentID of its folder, ordered by PositionNr;
    /// every value is a child element.
    /// Passwords are not imported: Royal stores them encrypted with the document's key.
    /// </summary>
    [SupportedOSPlatform("windows")]
    public class RoyalTsDocumentDeserializer
    {
        private const string FolderElement = "RoyalFolder";
        private const string CredentialElement = "RoyalCredential";

        // CredentialMode 2 is "specify a username and password" on the connection itself;
        // the other modes point at a shared RoyalCredential.
        private const string CredentialModeSpecified = "2";

        private readonly Dictionary<string, int> _skippedObjectTypes = new(StringComparer.Ordinal);
        private readonly Dictionary<string, string> _sharedUsernamesById = new(StringComparer.Ordinal);
        private readonly Dictionary<string, string> _sharedUsernamesByName = new(StringComparer.Ordinal);

        /// <summary>The name of the Royal document, or an empty string when it has none.</summary>
        public string DocumentName { get; private set; } = string.Empty;

        /// <summary>Royal object types left out because mRemoteNG has no equivalent, with their count.</summary>
        public IReadOnlyDictionary<string, int> SkippedObjectTypes => _skippedObjectTypes;

        /// <summary>How many objects in the document carried a password that was not imported.</summary>
        public int PasswordsNotImported { get; private set; }

        public ConnectionTreeModel Deserialize(string content)
        {
            DocumentName = string.Empty;
            PasswordsNotImported = 0;
            _skippedObjectTypes.Clear();
            _sharedUsernamesById.Clear();
            _sharedUsernamesByName.Clear();

            ConnectionTreeModel connectionTreeModel = new();
            RootNodeInfo root = new(RootNodeType.Connection);
            connectionTreeModel.AddRootNode(root);

            XmlDocument xmlDocument = SecureXmlHelper.LoadXmlFromString(content);
            XmlElement documentElement = xmlDocument.DocumentElement;
            if (documentElement?.Name != "RTSZDocument")
                throw new FormatException("This is not a Royal TS document: the root element is not RTSZDocument.");

            List<XmlElement> objects = documentElement.ChildNodes.OfType<XmlElement>().ToList();
            foreach (XmlElement credential in objects.Where(element => element.Name == CredentialElement))
            {
                string username = Text(credential, "UserName");
                AddSharedUsername(_sharedUsernamesById, Text(credential, "ID"), username);
                AddSharedUsername(_sharedUsernamesByName, Text(credential, "Name"), username);
                if (Text(credential, "Password").Length > 0)
                    PasswordsNotImported++;
            }

            Dictionary<string, ConnectionInfo> nodes = new(StringComparer.Ordinal);
            Dictionary<string, string> parentIds = new(StringComparer.Ordinal);
            List<(string Id, int Position)> order = new();

            foreach (XmlElement element in objects)
            {
                switch (element.Name)
                {
                    case "RoyalDocument":
                        DocumentName = Text(element, "Name");
                        continue;
                    // Deleted objects stay in the document under RoyalTrash; shared credentials were read above.
                    case "RoyalTrash":
                    case CredentialElement:
                        continue;
                }

                string id = Text(element, "ID");
                if (id.Length == 0)
                    continue;

                ConnectionInfo node = CreateNode(element);
                if (node == null)
                {
                    _skippedObjectTypes[element.Name] = _skippedObjectTypes.GetValueOrDefault(element.Name) + 1;
                    continue;
                }

                if (Text(element, "CredentialPassword").Length > 0)
                    PasswordsNotImported++;

                nodes[id] = node;
                parentIds[id] = Text(element, "ParentID");
                order.Add((id, int.TryParse(Text(element, "PositionNr"), out int position) ? position : 0));
            }

            // A node whose parent is not one of the imported nodes belongs at the top, unless
            // that parent is the trash. Anything inside a deleted folder hangs off a node that
            // is never attached, so it is left out as well.
            HashSet<string> trashIds = new(objects.Where(element => element.Name == "RoyalTrash")
                                                  .Select(element => Text(element, "ID")),
                                           StringComparer.Ordinal);
            foreach ((string id, _) in order.OrderBy(entry => entry.Position)
                                                .ThenBy(entry => entry.Id, StringComparer.Ordinal))
            {
                string parentId = parentIds[id];
                if (nodes.TryGetValue(parentId, out ConnectionInfo parent) && parent is ContainerInfo folder)
                    folder.AddChild(nodes[id]);
                else if (!trashIds.Contains(parentId))
                    root.AddChild(nodes[id]);
            }

            return connectionTreeModel;
        }

        private ConnectionInfo CreateNode(XmlElement element)
        {
            string name = Text(element, "Name");

            if (element.Name == FolderElement)
            {
                ContainerInfo folder = new()
                {
                    Name = name,
                    IsExpanded = Text(element, "IsExpanded") != "False"
                };
                ApplyDescription(folder, element);
                ApplyCredentials(folder, element);
                return folder;
            }

            string uri = Text(element, "URI");
            string portElement = "Port";
            ProtocolType protocol;
            switch (element.Name)
            {
                case "RoyalRDSConnection":
                    protocol = ProtocolType.RDP;
                    portElement = "RDPPort";
                    break;
                case "RoyalSSHConnection":
                    // Royal has no Telnet type of its own: it is an SSH connection with a flag.
                    protocol = Text(element, "IsTelnetConnection") == "True" ? ProtocolType.Telnet : ProtocolType.SSH2;
                    break;
                case "RoyalWebConnection":
                    protocol = uri.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ? ProtocolType.HTTP : ProtocolType.HTTPS;
                    break;
                case "RoyalVNCConnection":
                    protocol = ProtocolType.VNC;
                    break;
                default:
                    return null;
            }

            bool isWeb = protocol is ProtocolType.HTTP or ProtocolType.HTTPS;
            ConnectionInfo connection = new()
            {
                Name = name.Length > 0 ? name : uri,
                Protocol = protocol,
                // A web connection keeps the whole URL, which the HTTP protocols open as it is.
                Hostname = isWeb ? uri : HostFromUri(uri)
            };
            // The URL of a web connection already carries any port; a non-default Port would be appended to it.
            connection.Port = !isWeb && int.TryParse(Text(element, portElement), out int port) && port > 0
                ? port
                : connection.GetDefaultPort();
            ApplyDescription(connection, element);
            ApplyCredentials(connection, element);
            return connection;
        }

        // Only set when Royal has one, so the default description from the settings stays otherwise.
        private static void ApplyDescription(ConnectionInfo node, XmlElement element)
        {
            string description = Text(element, "Description");
            if (description.Length > 0)
                node.Description = description;
        }

        private void ApplyCredentials(ConnectionInfo node, XmlElement element)
        {
            // Royal's "use the credentials of the parent folder" is what mRemoteNG's inheritance does.
            if (Text(element, "CredentialFromParent") == "True")
            {
                node.Inheritance.Username = true;
                node.Inheritance.Domain = true;
                node.Inheritance.Password = true;
                return;
            }

            string username = Text(element, "CredentialUsername");
            if (Text(element, "CredentialMode") != CredentialModeSpecified &&
                (_sharedUsernamesById.TryGetValue(Text(element, "CredentialId"), out string shared) ||
                 _sharedUsernamesByName.TryGetValue(Text(element, "CredentialName"), out shared)))
            {
                username = shared;
            }

            // Royal keeps DOMAIN\user in one field; mRemoteNG has the two separately.
            int separator = username.IndexOf('\\');
            if (separator > 0)
            {
                node.Domain = username[..separator];
                username = username[(separator + 1)..];
            }

            node.Username = username;
        }

        private static void AddSharedUsername(Dictionary<string, string> usernames, string key, string username)
        {
            if (key.Length > 0)
                usernames.TryAdd(key, username);
        }

        private static string HostFromUri(string uri)
        {
            int schemeEnd = uri.IndexOf("://", StringComparison.Ordinal);
            if (schemeEnd < 0)
                return uri;

            string rest = uri[(schemeEnd + 3)..];
            int pathStart = rest.IndexOf('/');
            return pathStart < 0 ? rest : rest[..pathStart];
        }

        private static string Text(XmlElement element, string childName)
        {
            return element[childName]?.InnerText ?? string.Empty;
        }
    }
}
