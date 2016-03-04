using System.Collections.Generic;
using System;
using AxWFICALib;
using System.Drawing;
using System.Diagnostics;
using System.Data;
using AxMSTSCLib;
using Microsoft.VisualBasic;
using System.Collections;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Runtime.InteropServices;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.App;


namespace mRemoteNG.Config.Import
{
	public class RemoteDesktopConnectionManager
	{
		public static void Import(string fileName, TreeNode parentTreeNode)
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(fileName);
				
			XmlNode rdcManNode = xmlDocument.SelectSingleNode("/RDCMan");
			int schemaVersion = (int) (rdcManNode.Attributes["schemaVersion"].Value);
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
				
			Container.Info containerInfo = new Container.Info();
			containerInfo.TreeNode = treeNode;
			containerInfo.Name = name;
				
			Connection.Info connectionInfo = ConnectionInfoFromXml(propertiesNode);
			connectionInfo.Parent = containerInfo;
			connectionInfo.IsContainer = true;
			containerInfo.ConnectionInfo = connectionInfo;
				
			// We can only inherit from a container node, not the root node or connection nodes
			if (Tree.Node.GetNodeType(parentTreeNode) == Tree.Node.Type.Container)
			{
				containerInfo.Parent = parentTreeNode.Tag;
			}
			else
			{
				connectionInfo.Inherit.TurnOffInheritanceCompletely();
			}
				
			treeNode.Name = name;
			treeNode.Tag = containerInfo;
			treeNode.ImageIndex = Images.Enums.TreeImage.Container;
			treeNode.SelectedImageIndex = Images.Enums.TreeImage.Container;
				
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
				
			Connection.Info connectionInfo = ConnectionInfoFromXml(serverNode);
			connectionInfo.TreeNode = treeNode;
			connectionInfo.Parent = parentTreeNode.Tag;
				
			treeNode.Name = name;
			treeNode.Tag = connectionInfo;
			treeNode.ImageIndex = Images.Enums.TreeImage.ConnectionClosed;
			treeNode.SelectedImageIndex = Images.Enums.TreeImage.ConnectionClosed;
				
			Runtime.ConnectionList.Add(connectionInfo);
		}
			
		private static Protocol.Info ConnectionInfoFromXml(XmlNode xmlNode)
		{
			Connection.Info connectionInfo = new Connection.Info();
			connectionInfo.Inherit = new Connection.Info.Inheritance(connectionInfo);
				
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
				connectionInfo.Inherit.Username = true;
				connectionInfo.Inherit.Password = true;
				connectionInfo.Inherit.Domain = true;
			}
				
			XmlNode connectionSettingsNode = xmlNode.SelectSingleNode("./connectionSettings");
			if (connectionSettingsNode.Attributes["inherit"].Value == "None")
			{
				connectionInfo.UseConsoleSession = bool.Parse(connectionSettingsNode.SelectSingleNode("./connectToConsole").InnerText);
				// ./startProgram
				// ./workingDir
				connectionInfo.Port = (int) (connectionSettingsNode.SelectSingleNode("./port").InnerText);
			}
			else
			{
				connectionInfo.Inherit.UseConsoleSession = true;
				connectionInfo.Inherit.Port = true;
			}
				
			XmlNode gatewaySettingsNode = xmlNode.SelectSingleNode("./gatewaySettings");
			if (gatewaySettingsNode.Attributes["inherit"].Value == "None")
			{
				if (gatewaySettingsNode.SelectSingleNode("./enabled").InnerText == "True")
				{
					connectionInfo.RDGatewayUsageMethod = RDP.RDGatewayUsageMethod.Always;
				}
				else
				{
					connectionInfo.RDGatewayUsageMethod = RDP.RDGatewayUsageMethod.Never;
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
				connectionInfo.Inherit.RDGatewayUsageMethod = true;
				connectionInfo.Inherit.RDGatewayHostname = true;
				connectionInfo.Inherit.RDGatewayUsername = true;
				connectionInfo.Inherit.RDGatewayPassword = true;
				connectionInfo.Inherit.RDGatewayDomain = true;
			}
				
			XmlNode remoteDesktopNode = xmlNode.SelectSingleNode("./remoteDesktop");
			if (remoteDesktopNode.Attributes["inherit"].Value == "None")
			{
				string resolutionString = System.Convert.ToString(remoteDesktopNode.SelectSingleNode("./size").InnerText.Replace(" ", ""));
				try
				{
					connectionInfo.Resolution = "Res" + System.Convert.ToString(Tools.Misc.StringToEnum(typeof(Connection.Protocol.RDP.RDPResolutions), resolutionString));
				}
				catch (ArgumentException)
				{
					connectionInfo.Resolution = RDP.RDPResolutions.FitToWindow;
				}
					
				if (remoteDesktopNode.SelectSingleNode("./sameSizeAsClientArea").InnerText == "True")
				{
					connectionInfo.Resolution = RDP.RDPResolutions.FitToWindow;
				}
					
				if (remoteDesktopNode.SelectSingleNode("./fullScreen").InnerText == "True")
				{
					connectionInfo.Resolution = RDP.RDPResolutions.Fullscreen;
				}
					
				connectionInfo.Colors = remoteDesktopNode.SelectSingleNode("./colorDepth").InnerText;
			}
			else
			{
				connectionInfo.Inherit.Resolution = true;
				connectionInfo.Inherit.Colors = true;
			}
				
			XmlNode localResourcesNode = xmlNode.SelectSingleNode("./localResources");
			if (localResourcesNode.Attributes["inherit"].Value == "None")
			{
				switch (localResourcesNode.SelectSingleNode("./audioRedirection").InnerText)
				{
					case 0: // Bring to this computer
						connectionInfo.RedirectSound = RDP.RDPSounds.BringToThisComputer;
						break;
					case 1: // Leave at remote computer
						connectionInfo.RedirectSound = RDP.RDPSounds.LeaveAtRemoteComputer;
						break;
					case 2: // Do not play
						connectionInfo.RedirectSound = RDP.RDPSounds.DoNotPlay;
						break;
				}
					
				// ./audioRedirectionQuality
				// ./audioCaptureRedirection
					
				switch (localResourcesNode.SelectSingleNode("./keyboardHook").InnerText)
				{
					case 0: // On the local computer
						connectionInfo.RedirectKeys = false;
						break;
					case 1: // On the remote computer
						connectionInfo.RedirectKeys = true;
						break;
					case 2: // In full screen mode only
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
				connectionInfo.Inherit.RedirectSound = true;
				connectionInfo.Inherit.RedirectKeys = true;
				connectionInfo.Inherit.RedirectDiskDrives = true;
				connectionInfo.Inherit.RedirectPorts = true;
				connectionInfo.Inherit.RedirectPrinters = true;
				connectionInfo.Inherit.RedirectSmartCards = true;
			}
				
			XmlNode securitySettingsNode = xmlNode.SelectSingleNode("./securitySettings");
			if (securitySettingsNode.Attributes["inherit"].Value == "None")
			{
				switch (securitySettingsNode.SelectSingleNode("./authentication").InnerText)
				{
					case 0: // No authentication
						connectionInfo.RDPAuthenticationLevel = RDP.AuthenticationLevel.NoAuth;
						break;
					case 1: // Do not connect if authentication fails
						connectionInfo.RDPAuthenticationLevel = RDP.AuthenticationLevel.AuthRequired;
						break;
					case 2: // Warn if authentication fails
						connectionInfo.RDPAuthenticationLevel = RDP.AuthenticationLevel.WarnOnFailedAuth;
						break;
				}
			}
			else
			{
				connectionInfo.Inherit.RDPAuthenticationLevel = true;
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
			Win32.DATA_BLOB plaintextData = new Win32.DATA_BLOB();
			try
			{
				byte[] ciphertextArray = Convert.FromBase64String(ciphertext);
				gcHandle = System.Runtime.InteropServices.GCHandle.Alloc(ciphertextArray, GCHandleType.Pinned);
					
				Win32.DATA_BLOB ciphertextData = new Win32.DATA_BLOB();
				ciphertextData.cbData = ciphertextArray.Length;
				ciphertextData.pbData = gcHandle.AddrOfPinnedObject();
					
				Win32.DATA_BLOB temp_optionalEntropy = null;
				IntPtr temp_promptStruct = null;
				if (!Win32.CryptUnprotectData(ref ciphertextData, null, ref temp_optionalEntropy, null, ref temp_promptStruct, 0, ref plaintextData))
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
					Win32.LocalFree(plaintextData.pbData);
				}
			}
		}
			
		// ReSharper disable once ClassNeverInstantiated.Local
		private class Win32
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
