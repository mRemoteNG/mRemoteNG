using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using mRemoteNG.App;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Container;
using mRemoteNG.Messages;
using System.Runtime.Versioning;

namespace mRemoteNG.Config.Import
{
    [SupportedOSPlatform("windows")]
    public class BookmarksHtmlImporter : IConnectionImporter<string>
    {
        public void Import(string fileName, ContainerInfo destinationContainer)
        {
            if (string.IsNullOrEmpty(fileName))
                return;

            if (!File.Exists(fileName))
            {
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, $"Import file not found: {fileName}");
                return;
            }

            try
            {
                var content = File.ReadAllText(fileName);
                ImportContent(content, destinationContainer);
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("BookmarksHtmlImporter.Import failed", ex);
            }
        }

        private static void ImportContent(string content, ContainerInfo rootContainer)
        {
            var stack = new Stack<ContainerInfo>();
            stack.Push(rootContainer);

            // Regex to parse Netscape Bookmarks HTML format
            // Captures Folders (H3), Links (A), and DL structure
            var regex = new Regex(@"(<DT><H3.*?>(?<folder>.*?)</H3>)|(<DT><A\s+HREF=""(?<url>.*?)"".*?>(?<title>.*?)</A>)|(?<dlstart><DL.*?>)|(?<dlend></DL>)", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            
            ContainerInfo? pendingFolder = null;
            
            var matches = regex.Matches(content);
            foreach (Match match in matches)
            {
                if (match.Groups["folder"].Success)
                {
                    var folderName = match.Groups["folder"].Value;
                    var newFolder = new ContainerInfo { Name = System.Net.WebUtility.HtmlDecode(folderName) };
                    
                    // Add to current parent (top of stack)
                    if (stack.Count > 0)
                    {
                        stack.Peek().AddChild(newFolder);
                    }
                    
                    pendingFolder = newFolder;
                }
                else if (match.Groups["dlstart"].Success)
                {
                    // Enter the pending folder
                    if (pendingFolder != null)
                    {
                        stack.Push(pendingFolder);
                        pendingFolder = null;
                    }
                }
                else if (match.Groups["dlend"].Success)
                {
                    // Leave current folder
                    if (stack.Count > 1) // Don't pop root
                    {
                        stack.Pop();
                    }
                }
                else if (match.Groups["url"].Success)
                {
                    var url = match.Groups["url"].Value;
                    var title = match.Groups["title"].Value;
                    
                    var connection = new ConnectionInfo
                    {
                        Name = System.Net.WebUtility.HtmlDecode(title),
                        Hostname = url,
                        Protocol = GetProtocolFromUrl(url)
                    };
                    
                    if (stack.Count > 0)
                    {
                        stack.Peek().AddChild(connection);
                    }
                }
            }
        }

        private static ProtocolType GetProtocolFromUrl(string url)
        {
            if (url.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                return ProtocolType.HTTPS;
            if (url.StartsWith("http://", StringComparison.OrdinalIgnoreCase))
                return ProtocolType.HTTP;
            return ProtocolType.HTTP; // Default to HTTP for web bookmarks
        }
    }
}
