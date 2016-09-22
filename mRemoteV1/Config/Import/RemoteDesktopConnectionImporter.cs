using System;
using System.Windows.Forms;
using System.IO;
using mRemoteNG.App;
using mRemoteNG.Connection.Protocol.RDP;
using mRemoteNG.Container;
using mRemoteNG.Connection;
using mRemoteNG.Tree;

namespace mRemoteNG.Config.Import
{
	public class RemoteDesktopConnectionImporter
	{
		public static void Import(string fileName, TreeNode parentTreeNode)
		{
			string[] lines = File.ReadAllLines(fileName);
				
			string name = Path.GetFileNameWithoutExtension(fileName);
			TreeNode treeNode = new TreeNode(name);
			parentTreeNode.Nodes.Add(treeNode);
				
			ConnectionInfo connectionInfo = new ConnectionInfo();
			connectionInfo.Inheritance = new ConnectionInfoInheritance(connectionInfo);
			connectionInfo.Name = name;
			connectionInfo.TreeNode = treeNode;
				
			if (treeNode.Parent.Tag is ContainerInfo)
			{
                connectionInfo.Parent = (ContainerInfo)treeNode.Parent.Tag;
			}
				
			treeNode.Name = name;
			treeNode.Tag = connectionInfo;
			treeNode.ImageIndex = (int)TreeImageType.ConnectionClosed;
			treeNode.SelectedImageIndex = (int)TreeImageType.ConnectionClosed;
				
			foreach (string line in lines)
			{
				string[] parts = line.Split(new char[] {':'}, 3);
				if (parts.Length < 3)
				{
					continue;
				}
					
				string key = parts[0];
				string value = parts[2];
					
				SetConnectionInfoParameter(connectionInfo, key, value);
			}
				
			Runtime.ConnectionList.Add(connectionInfo);
		}
			
		private static void SetConnectionInfoParameter(ConnectionInfo connectionInfo, string key, string value)
		{
			switch (key.ToLower())
			{
				case "full address":
					Uri uri = new Uri("dummyscheme" + Uri.SchemeDelimiter + value);
					if (!string.IsNullOrEmpty(uri.Host))
					{
						connectionInfo.Hostname = uri.Host;
					}
					if (!(uri.Port == -1))
					{
						connectionInfo.Port = uri.Port;
					}
					break;
				case "server port":
					connectionInfo.Port = Convert.ToInt32(value);
					break;
				case "username":
					connectionInfo.Username = value;
					break;
				case "domain":
					connectionInfo.Domain = value;
					break;
				case "session bpp":
					switch (value)
					{
						case "8":
							connectionInfo.Colors = ProtocolRDP.RDPColors.Colors256;
							break;
						case "15":
							connectionInfo.Colors = ProtocolRDP.RDPColors.Colors15Bit;
							break;
						case "16":
							connectionInfo.Colors = ProtocolRDP.RDPColors.Colors16Bit;
							break;
						case "24":
							connectionInfo.Colors = ProtocolRDP.RDPColors.Colors24Bit;
							break;
						case "32":
							connectionInfo.Colors = ProtocolRDP.RDPColors.Colors32Bit;
							break;
					}
					break;
				case "bitmapcachepersistenable":
					if (value == "1")
					{
						connectionInfo.CacheBitmaps = true;
					}
					else
					{
						connectionInfo.CacheBitmaps = false;
					}
					break;
				case "screen mode id":
					if (value == "2")
					{
						connectionInfo.Resolution = ProtocolRDP.RDPResolutions.Fullscreen;
					}
					else
					{
						connectionInfo.Resolution = ProtocolRDP.RDPResolutions.FitToWindow;
					}
					break;
				case "connect to console":
					if (value == "1")
					{
						connectionInfo.UseConsoleSession = true;
					}
					break;
				case "disable wallpaper":
					if (value == "1")
					{
						connectionInfo.DisplayWallpaper = true;
					}
					else
					{
						connectionInfo.DisplayWallpaper = false;
					}
					break;
				case "disable themes":
					if (value == "1")
					{
						connectionInfo.DisplayThemes = true;
					}
					else
					{
						connectionInfo.DisplayThemes = false;
					}
					break;
				case "allow font smoothing":
					if (value == "1")
					{
						connectionInfo.EnableFontSmoothing = true;
					}
					else
					{
						connectionInfo.EnableFontSmoothing = false;
					}
					break;
				case "allow desktop composition":
					if (value == "1")
					{
						connectionInfo.EnableDesktopComposition = true;
					}
					else
					{
						connectionInfo.EnableDesktopComposition = false;
					}
					break;
				case "redirectsmartcards":
					if (value == "1")
					{
						connectionInfo.RedirectSmartCards = true;
					}
					else
					{
						connectionInfo.RedirectSmartCards = false;
					}
					break;
				case "redirectdrives":
					if (value == "1")
					{
						connectionInfo.RedirectDiskDrives = true;
					}
					else
					{
						connectionInfo.RedirectDiskDrives = false;
					}
					break;
				case "redirectcomports":
					if (value == "1")
					{
						connectionInfo.RedirectPorts = true;
					}
					else
					{
						connectionInfo.RedirectPorts = false;
					}
					break;
				case "redirectprinters":
					if (value == "1")
					{
						connectionInfo.RedirectPrinters = true;
					}
					else
					{
						connectionInfo.RedirectPrinters = false;
					}
					break;
				case "audiomode":
					switch (value)
					{
						case "0":
							connectionInfo.RedirectSound = ProtocolRDP.RDPSounds.BringToThisComputer;
							break;
						case "1":
							connectionInfo.RedirectSound = ProtocolRDP.RDPSounds.LeaveAtRemoteComputer;
							break;
						case "2":
							connectionInfo.RedirectSound = ProtocolRDP.RDPSounds.DoNotPlay;
							break;
					}
					break;
			}
		}
	}
}