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
	public class RemoteDesktopConnectionManager
	{
		public static void Import(string fileName, TreeNode parentTreeNode)
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(fileName);
				
			XmlNode rdcManNode = xmlDocument.SelectSingleNode("/RDCMan");
			int schemaVersion = Convert.ToInt32(rdcManNode.Attributes["schemaVersion"].Value);
			if (!(schemaVersion == 1))
			{
				throw (new FileFormatException(string.Format("Unsupported schema version ({0}).", schemaVersion)));
			}
				
			XmlNode versionNode = rdcManNode.SelectSingleNode("./version");
			Version version = new Version(versionNode.InnerText);
			if (!(version == new Version(2, 2)))
			{
				throw (new FileFormatException(string.Format("Unsupported file version ({0}).", version)));
			}
				
			XmlNode fileNode = rdcManNode.SelectSingleNode("./file");
			ImportFileOrGroup(fileNode, parentTreeNode);
		}
			
		private static void ImportFileOrGroup(XmlNode xmlNode, TreeNode parentTreeNode)
		{
			XmlNode propertiesNode = xmlNode.SelectSingleNode("./properties");
			string name = propertiesNode.SelectSingleNode("./name").InnerText;
				
			TreeNode treeNode = new TreeNode(name);
			parentTreeNode.Nodes.Add(treeNode);

            ContainerInfo containerInfo = new ContainerInfo();
			containerInfo.TreeNode = treeNode;
			containerInfo.Name = name;

            ConnectionInfo connectionInfo = ConnectionInfoFromXml(propertiesNode);
			connectionInfo.Parent = containerInfo;
			connectionInfo.IsContainer = true;
			containerInfo.ConnectionInfo = connectionInfo;
				
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
				
			containerInfo.IsExpanded = bool.Parse(propertiesNode.SelectSingleNode("./expanded").InnerText);
			if (containerInfo.IsExpanded)
			{
				treeNode.Expand();
			}

            Runtime.ContainerList.Add(containerInfo);
		}
			
		private static void ImportServer(XmlNode serverNode, TreeNode parentTreeNode)
		{
			string name = serverNode.SelectSingleNode("./displayName").InnerText;
			TreeNode treeNode = new TreeNode(name);
			parentTreeNode.Nodes.Add(treeNode);
				
			ConnectionInfo connectionInfo = ConnectionInfoFromXml(serverNode);
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
			ConnectionInfo connectionInfo = new ConnectionInfo();
			connectionInfo.Inheritance = new ConnectionInfoInheritance(connectionInfo);
				
			string name = xmlNode.SelectSingleNode("./name").InnerText;
				
			string displayName = "";
			XmlNode displayNameNode = xmlNode.SelectSingleNode("./displayName");
			if (displayNameNode == null)
			{
				displayName = name;
			}
			else
			{
				displayName = displayNameNode.InnerText;
			}
				
			connectionInfo.Name = displayName;
			connectionInfo.Description = xmlNode.SelectSingleNode("./comment").InnerText;
			connectionInfo.Hostname = name;
				
			XmlNode logonCredentialsNode = xmlNode.SelectSingleNode("./logonCredentials");
			if (logonCredentialsNode.Attributes["inherit"].Value == "None")
			{
				connectionInfo.Username = logonCredentialsNode.SelectSingleNode("userName").InnerText;
					
				XmlNode passwordNode = logonCredentialsNode.SelectSingleNode("./password");
				if (passwordNode.Attributes["storeAsClearText"].Value == "True")
				{
					connectionInfo.Password = passwordNode.InnerText;
				}
				else
				{
					connectionInfo.Password = DecryptPassword(passwordNode.InnerText);
				}
					
				connectionInfo.Domain = logonCredentialsNode.SelectSingleNode("./domain").InnerText;
			}
			else
			{
				connectionInfo.Inheritance.Username = true;
				connectionInfo.Inheritance.Password = true;
				connectionInfo.Inheritance.Domain = true;
			}
				
			XmlNode connectionSettingsNode = xmlNode.SelectSingleNode("./connectionSettings");
			if (connectionSettingsNode.Attributes["inherit"].Value == "None")
			{
				connectionInfo.UseConsoleSession = bool.Parse(connectionSettingsNode.SelectSingleNode("./connectToConsole").InnerText);
				// ./startProgram
				// ./workingDir
				connectionInfo.Port = Convert.ToInt32(connectionSettingsNode.SelectSingleNode("./port").InnerText);
			}
			else
			{
				connectionInfo.Inheritance.UseConsoleSession = true;
				connectionInfo.Inheritance.Port = true;
			}
				
			XmlNode gatewaySettingsNode = xmlNode.SelectSingleNode("./gatewaySettings");
			if (gatewaySettingsNode.Attributes["inherit"].Value == "None")
			{
				if (gatewaySettingsNode.SelectSingleNode("./enabled").InnerText == "True")
				{
					connectionInfo.RDGatewayUsageMethod = ProtocolRDP.RDGatewayUsageMethod.Always;
				}
				else
				{
					connectionInfo.RDGatewayUsageMethod = ProtocolRDP.RDGatewayUsageMethod.Never;
				}
					
				connectionInfo.RDGatewayHostname = gatewaySettingsNode.SelectSingleNode("./hostName").InnerText;
				connectionInfo.RDGatewayUsername = gatewaySettingsNode.SelectSingleNode("./userName").InnerText;
					
				XmlNode passwordNode = logonCredentialsNode.SelectSingleNode("./password");
				if (passwordNode.Attributes["storeAsClearText"].Value == "True")
				{
					connectionInfo.RDGatewayPassword = passwordNode.InnerText;
				}
				else
				{
					connectionInfo.Password = DecryptPassword(passwordNode.InnerText);
				}
					
				connectionInfo.RDGatewayDomain = gatewaySettingsNode.SelectSingleNode("./domain").InnerText;
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
				
			XmlNode remoteDesktopNode = xmlNode.SelectSingleNode("./remoteDesktop");
			if (remoteDesktopNode.Attributes["inherit"].Value == "None")
			{
				string resolutionString = Convert.ToString(remoteDesktopNode.SelectSingleNode("./size").InnerText.Replace(" ", ""));
				try
				{
                    connectionInfo.Resolution = (ProtocolRDP.RDPResolutions)Enum.Parse(typeof(ProtocolRDP.RDPResolutions), "Res" + resolutionString);
				}
				catch (ArgumentException)
				{
					connectionInfo.Resolution = ProtocolRDP.RDPResolutions.FitToWindow;
				}
					
				if (remoteDesktopNode.SelectSingleNode("./sameSizeAsClientArea").InnerText == "True")
				{
					connectionInfo.Resolution = ProtocolRDP.RDPResolutions.FitToWindow;
				}
					
				if (remoteDesktopNode.SelectSingleNode("./fullScreen").InnerText == "True")
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
				
			XmlNode localResourcesNode = xmlNode.SelectSingleNode("./localResources");
			if (localResourcesNode.Attributes["inherit"].Value == "None")
			{
				switch (localResourcesNode.SelectSingleNode("./audioRedirection").InnerText)
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
					
				switch (localResourcesNode.SelectSingleNode("./keyboardHook").InnerText)
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
				
			XmlNode securitySettingsNode = xmlNode.SelectSingleNode("./securitySettings");
			if (securitySettingsNode.Attributes["inherit"].Value == "None")
			{
				switch (securitySettingsNode.SelectSingleNode("./authentication").InnerText)
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
				
			GCHandle gcHandle = new GCHandle();
			NativeMethods.DATA_BLOB plaintextData = new NativeMethods.DATA_BLOB();
			try
			{
				byte[] ciphertextArray = Convert.FromBase64String(ciphertext);
				gcHandle = GCHandle.Alloc(ciphertextArray, GCHandleType.Pinned);
					
				NativeMethods.DATA_BLOB ciphertextData = new NativeMethods.DATA_BLOB();
				ciphertextData.cbData = ciphertextArray.Length;
				ciphertextData.pbData = gcHandle.AddrOfPinnedObject();

                NativeMethods.DATA_BLOB temp_optionalEntropy = new NativeMethods.DATA_BLOB();
				IntPtr temp_promptStruct = IntPtr.Zero;
				if (!NativeMethods.CryptUnprotectData(ref ciphertextData, null, ref temp_optionalEntropy, IntPtr.Zero, ref temp_promptStruct, 0, ref plaintextData))
				{
					return null;
				}
					
				int plaintextLength = (int) ((double) plaintextData.cbData / 2); // Char = 2 bytes
				char[] plaintextArray = new char[plaintextLength - 1 + 1];
				Marshal.Copy(plaintextData.pbData, plaintextArray, 0, plaintextLength);
					
				return new string(plaintextArray);
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddExceptionMessage(message: "RemoteDesktopConnectionManager.DecryptPassword() failed.", ex: ex, logOnly: true);
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