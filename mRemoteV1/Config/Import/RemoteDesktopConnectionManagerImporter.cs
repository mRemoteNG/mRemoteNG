using System;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Runtime.InteropServices;
using mRemoteNG.App;
using mRemoteNG.Connection.Protocol.RDP;
using mRemoteNG.Connection;
using mRemoteNG.Container;
using mRemoteNG.Tree;

namespace mRemoteNG.Config.Import
{
	public class RemoteDesktopConnectionManagerImporter
	{
        public static void Import(string fileName, TreeNode parentTreeNode)
		{
			var xmlDocument = new XmlDocument();
			xmlDocument.Load(fileName);

            var rdcManNode = xmlDocument.SelectSingleNode("/RDCMan");
            var schemaVersion = Convert.ToInt32(rdcManNode?.Attributes?["schemaVersion"].Value);
			if (schemaVersion != 1)
			{
				throw (new FileFormatException($"Unsupported schema version ({schemaVersion})."));
			}
				
			var versionNode = rdcManNode.SelectSingleNode("./version");
            var version = new Version(versionNode.InnerText);
			if (!(version == new Version(2, 2)))
			{
				throw (new FileFormatException($"Unsupported file version ({version})."));
			}

            var fileNode = rdcManNode.SelectSingleNode("./file");
			ImportFileOrGroup(fileNode, parentTreeNode);
		}
			
		private static void ImportFileOrGroup(XmlNode xmlNode, TreeNode parentTreeNode)
		{
            var propertiesNode = xmlNode.SelectSingleNode("./properties");
            var name = propertiesNode?.SelectSingleNode("./name")?.InnerText;

            var treeNode = new TreeNode(name);
			parentTreeNode.Nodes.Add(treeNode);

		    var containerInfo = new ContainerInfo
		    {
		        TreeNode = treeNode,
		        Name = name
		    };

		    var connectionInfo = ConnectionInfoFromXml(propertiesNode);
			connectionInfo.Parent = containerInfo;
			connectionInfo.IsContainer = true;
			containerInfo.CopyFrom(connectionInfo);
				
			// We can only inherit from a container node, not the root node or connection nodes
			if (ConnectionTreeNode.GetNodeType(parentTreeNode) == TreeNodeType.Container)
			{
				containerInfo.Parent = (ContainerInfo)parentTreeNode.Tag;
			}
			else
			{
				connectionInfo.Inheritance.DisableInheritance();
			}
				
			treeNode.Name = name;
			treeNode.Tag = containerInfo;
			treeNode.ImageIndex = (int)TreeImageType.Container;
			treeNode.SelectedImageIndex = (int)TreeImageType.Container;
				
			foreach (XmlNode childNode in xmlNode.SelectNodes("./group|./server"))
			{
				switch (childNode.Name)
				{
					case "group":
						ImportFileOrGroup(childNode, treeNode);
						break;
					case "server":
						ImportServer(childNode, treeNode);
						break;
				}
			}
				
			containerInfo.IsExpanded = bool.Parse(propertiesNode.SelectSingleNode("./expanded")?.InnerText);
			if (containerInfo.IsExpanded)
			{
				treeNode.Expand();
			}

            Runtime.ContainerList.Add(containerInfo);
		}
			
		private static void ImportServer(XmlNode serverNode, TreeNode parentTreeNode)
		{
            var name = serverNode.SelectSingleNode("./displayName")?.InnerText ?? "";
            var treeNode = new TreeNode(name);
			parentTreeNode.Nodes.Add(treeNode);

            var connectionInfo = ConnectionInfoFromXml(serverNode);
			connectionInfo.TreeNode = treeNode;
			connectionInfo.Parent = (ContainerInfo)parentTreeNode.Tag;
				
			treeNode.Name = name;
			treeNode.Tag = connectionInfo;
			treeNode.ImageIndex = (int)TreeImageType.ConnectionClosed;
			treeNode.SelectedImageIndex = (int)TreeImageType.ConnectionClosed;
				
			Runtime.ConnectionList.Add(connectionInfo);
		}

        private static ConnectionInfo ConnectionInfoFromXml(XmlNode xmlNode)
		{
            var connectionInfo = new ConnectionInfo();
			connectionInfo.Inheritance = new ConnectionInfoInheritance(connectionInfo);

            var name = xmlNode.SelectSingleNode("./name")?.InnerText;

            var displayName = "";
            var displayNameNode = xmlNode.SelectSingleNode("./displayName");
			displayName = displayNameNode?.InnerText ?? name;
				
			connectionInfo.Name = displayName;
			connectionInfo.Description = xmlNode.SelectSingleNode("./comment")?.InnerText;
			connectionInfo.Hostname = name;

            var logonCredentialsNode = xmlNode.SelectSingleNode("./logonCredentials");
			if (logonCredentialsNode?.Attributes?["inherit"].Value == "None")
			{
				connectionInfo.Username = logonCredentialsNode.SelectSingleNode("userName")?.InnerText;

                var passwordNode = logonCredentialsNode.SelectSingleNode("./password");
				connectionInfo.Password = passwordNode?.Attributes?["storeAsClearText"].Value == "True" ? passwordNode.InnerText : DecryptPassword(passwordNode?.InnerText);
					
				connectionInfo.Domain = logonCredentialsNode.SelectSingleNode("./domain")?.InnerText;
			}
			else
			{
				connectionInfo.Inheritance.Username = true;
				connectionInfo.Inheritance.Password = true;
				connectionInfo.Inheritance.Domain = true;
			}

            var connectionSettingsNode = xmlNode.SelectSingleNode("./connectionSettings");
			if (connectionSettingsNode?.Attributes?["inherit"].Value == "None")
			{
				connectionInfo.UseConsoleSession = bool.Parse(connectionSettingsNode.SelectSingleNode("./connectToConsole")?.InnerText);
				// ./startProgram
				// ./workingDir
				connectionInfo.Port = Convert.ToInt32(connectionSettingsNode.SelectSingleNode("./port")?.InnerText);
			}
			else
			{
				connectionInfo.Inheritance.UseConsoleSession = true;
				connectionInfo.Inheritance.Port = true;
			}

            var gatewaySettingsNode = xmlNode.SelectSingleNode("./gatewaySettings");
			if (gatewaySettingsNode?.Attributes?["inherit"].Value == "None")
			{
				connectionInfo.RDGatewayUsageMethod = gatewaySettingsNode.SelectSingleNode("./enabled")?.InnerText == "True" ? ProtocolRDP.RDGatewayUsageMethod.Always : ProtocolRDP.RDGatewayUsageMethod.Never;
					
				connectionInfo.RDGatewayHostname = gatewaySettingsNode.SelectSingleNode("./hostName")?.InnerText;
				connectionInfo.RDGatewayUsername = gatewaySettingsNode.SelectSingleNode("./userName")?.InnerText;

                var passwordNode = logonCredentialsNode.SelectSingleNode("./password");
				if (passwordNode?.Attributes?["storeAsClearText"].Value == "True")
				{
					connectionInfo.RDGatewayPassword = passwordNode.InnerText;
				}
				else
				{
					connectionInfo.Password = DecryptPassword(passwordNode.InnerText);
				}
					
				connectionInfo.RDGatewayDomain = gatewaySettingsNode.SelectSingleNode("./domain")?.InnerText;
				// ./logonMethod
				// ./localBypass
				// ./credSharing
			}
			else
			{
				connectionInfo.Inheritance.RDGatewayUsageMethod = true;
				connectionInfo.Inheritance.RDGatewayHostname = true;
				connectionInfo.Inheritance.RDGatewayUsername = true;
				connectionInfo.Inheritance.RDGatewayPassword = true;
				connectionInfo.Inheritance.RDGatewayDomain = true;
			}

            var remoteDesktopNode = xmlNode.SelectSingleNode("./remoteDesktop");
			if (remoteDesktopNode?.Attributes?["inherit"].Value == "None")
			{
                var resolutionString = Convert.ToString(remoteDesktopNode.SelectSingleNode("./size")?.InnerText.Replace(" ", ""));
				try
				{
                    connectionInfo.Resolution = (ProtocolRDP.RDPResolutions)Enum.Parse(typeof(ProtocolRDP.RDPResolutions), "Res" + resolutionString);
				}
				catch (ArgumentException)
				{
					connectionInfo.Resolution = ProtocolRDP.RDPResolutions.FitToWindow;
				}
					
				if (remoteDesktopNode.SelectSingleNode("./sameSizeAsClientArea")?.InnerText == "True")
				{
					connectionInfo.Resolution = ProtocolRDP.RDPResolutions.FitToWindow;
				}
					
				if (remoteDesktopNode.SelectSingleNode("./fullScreen")?.InnerText == "True")
				{
					connectionInfo.Resolution = ProtocolRDP.RDPResolutions.Fullscreen;
				}


                connectionInfo.Colors = (ProtocolRDP.RDPColors)Enum.Parse(typeof(ProtocolRDP.RDPColors), remoteDesktopNode.SelectSingleNode("./colorDepth").InnerText);
			}
			else
			{
				connectionInfo.Inheritance.Resolution = true;
				connectionInfo.Inheritance.Colors = true;
			}

            var localResourcesNode = xmlNode.SelectSingleNode("./localResources");
			if (localResourcesNode?.Attributes?["inherit"].Value == "None")
			{
				switch (localResourcesNode.SelectSingleNode("./audioRedirection")?.InnerText)
				{
					case "0": // Bring to this computer
						connectionInfo.RedirectSound = ProtocolRDP.RDPSounds.BringToThisComputer;
						break;
					case "1": // Leave at remote computer
						connectionInfo.RedirectSound = ProtocolRDP.RDPSounds.LeaveAtRemoteComputer;
						break;
					case "2": // Do not play
						connectionInfo.RedirectSound = ProtocolRDP.RDPSounds.DoNotPlay;
						break;
				}
					
				// ./audioRedirectionQuality
				// ./audioCaptureRedirection
					
				switch (localResourcesNode.SelectSingleNode("./keyboardHook")?.InnerText)
				{
					case "0": // On the local computer
						connectionInfo.RedirectKeys = false;
						break;
					case "1": // On the remote computer
						connectionInfo.RedirectKeys = true;
						break;
					case "2": // In full screen mode only
						connectionInfo.RedirectKeys = false;
						break;
				}
					
				// ./redirectClipboard
				connectionInfo.RedirectDiskDrives = bool.Parse(localResourcesNode.SelectSingleNode("./redirectDrives").InnerText);
				connectionInfo.RedirectPorts = bool.Parse(localResourcesNode.SelectSingleNode("./redirectPorts").InnerText);
				connectionInfo.RedirectPrinters = bool.Parse(localResourcesNode.SelectSingleNode("./redirectPrinters").InnerText);
				connectionInfo.RedirectSmartCards = bool.Parse(localResourcesNode.SelectSingleNode("./redirectSmartCards").InnerText);
			}
			else
			{
				connectionInfo.Inheritance.RedirectSound = true;
				connectionInfo.Inheritance.RedirectKeys = true;
				connectionInfo.Inheritance.RedirectDiskDrives = true;
				connectionInfo.Inheritance.RedirectPorts = true;
				connectionInfo.Inheritance.RedirectPrinters = true;
				connectionInfo.Inheritance.RedirectSmartCards = true;
			}

            var securitySettingsNode = xmlNode.SelectSingleNode("./securitySettings");
			if (securitySettingsNode?.Attributes?["inherit"].Value == "None")
			{
				switch (securitySettingsNode.SelectSingleNode("./authentication")?.InnerText)
				{
					case "0": // No authentication
						connectionInfo.RDPAuthenticationLevel = ProtocolRDP.AuthenticationLevel.NoAuth;
						break;
					case "1": // Do not connect if authentication fails
						connectionInfo.RDPAuthenticationLevel = ProtocolRDP.AuthenticationLevel.AuthRequired;
						break;
					case "2": // Warn if authentication fails
						connectionInfo.RDPAuthenticationLevel = ProtocolRDP.AuthenticationLevel.WarnOnFailedAuth;
						break;
				}
			}
			else
			{
				connectionInfo.Inheritance.RDPAuthenticationLevel = true;
			}
				
			// ./displaySettings/thumbnailScale
			// ./displaySettings/liveThumbnailUpdates
			// ./displaySettings/showDisconnectedThumbnails
				
			return connectionInfo;
		}
			
		private static string DecryptPassword(string ciphertext)
		{
			if (string.IsNullOrEmpty(ciphertext))
			{
				return null;
			}

            var gcHandle = new GCHandle();
			var plaintextData = new NativeMethods.DATA_BLOB();
			try
			{
				var ciphertextArray = Convert.FromBase64String(ciphertext);
				gcHandle = GCHandle.Alloc(ciphertextArray, GCHandleType.Pinned);

			    var ciphertextData = new NativeMethods.DATA_BLOB
			    {
			        cbData = ciphertextArray.Length,
			        pbData = gcHandle.AddrOfPinnedObject()
			    };

			    var tempOptionalEntropy = new NativeMethods.DATA_BLOB();
                var tempPromptStruct = IntPtr.Zero;
				if (!NativeMethods.CryptUnprotectData(ref ciphertextData, null, ref tempOptionalEntropy, IntPtr.Zero, ref tempPromptStruct, 0, ref plaintextData))
				{
					return null;
				}

                var plaintextLength = (int) ((double) plaintextData.cbData / 2); // Char = 2 bytes
				var plaintextArray = new char[plaintextLength - 1 + 1];
				Marshal.Copy(plaintextData.pbData, plaintextArray, 0, plaintextLength);
					
				return new string(plaintextArray);
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddExceptionMessage("RemoteDesktopConnectionManager.DecryptPassword() failed.", ex, logOnly: true);
				return null;
			}
			finally
			{
				if (gcHandle.IsAllocated)
				{
					gcHandle.Free();
				}
				if (!(plaintextData.pbData == IntPtr.Zero))
				{
					NativeMethods.LocalFree(plaintextData.pbData);
				}
			}
		}
			
		// ReSharper disable once ClassNeverInstantiated.Local
		private class NativeMethods
		{
			// ReSharper disable InconsistentNaming
			// ReSharper disable IdentifierTypo
			// ReSharper disable StringLiteralTypo
			[DllImport("crypt32.dll", CharSet = CharSet.Unicode)]public static  extern bool CryptUnprotectData(ref DATA_BLOB dataIn, string description, ref DATA_BLOB optionalEntropy, IntPtr reserved, ref IntPtr promptStruct, int flags, ref DATA_BLOB dataOut);
				
			[DllImport("kernel32.dll", CharSet = CharSet.Unicode)]public static  extern void LocalFree(IntPtr ptr);
				
			public struct DATA_BLOB
			{
				public int cbData;
				public IntPtr pbData;
			}
			// ReSharper restore StringLiteralTypo
			// ReSharper restore IdentifierTypo
			// ReSharper restore InconsistentNaming
		}
	}
}