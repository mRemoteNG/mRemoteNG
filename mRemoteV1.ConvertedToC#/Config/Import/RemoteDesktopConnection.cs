using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;
using mRemoteNG.App.Runtime;

namespace mRemoteNG.Config.Import
{
	public class RemoteDesktopConnection
	{
		public static void Import(string fileName, TreeNode parentTreeNode)
		{
			string[] lines = File.ReadAllLines(fileName);

			string name = Path.GetFileNameWithoutExtension(fileName);
			TreeNode treeNode = new TreeNode(name);
			parentTreeNode.Nodes.Add(treeNode);

			Connection.Info connectionInfo = new Connection.Info();
			connectionInfo.Inherit = new Connection.Info.Inheritance(connectionInfo);
			connectionInfo.Name = name;
			connectionInfo.TreeNode = treeNode;

			if (treeNode.Parent.Tag is Container.Info) {
				connectionInfo.Parent = treeNode.Parent.Tag;
			}

			treeNode.Name = name;
			treeNode.Tag = connectionInfo;
			treeNode.ImageIndex = mRemoteNG.Images.Enums.TreeImage.ConnectionClosed;
			treeNode.SelectedImageIndex = mRemoteNG.Images.Enums.TreeImage.ConnectionClosed;

			foreach (string line in lines) {
				string[] parts = line.Split(new char[] { ":" }, 3);
				if (parts.Length < 3)
					continue;

				string key = parts[0];
				string value = parts[2];

				SetConnectionInfoParameter(ref connectionInfo, key, value);
			}

			mRemoteNG.App.Runtime.ConnectionList.Add(connectionInfo);
		}

		private static void SetConnectionInfoParameter(ref Connection.Info connectionInfo, string key, string value)
		{
			switch (Strings.LCase(key)) {
				case "full address":
					Uri uri = new Uri("dummyscheme" + uri.SchemeDelimiter + value);
					if (!string.IsNullOrEmpty(uri.Host))
						connectionInfo.Hostname = uri.Host;
					if (!(uri.Port == -1))
						connectionInfo.Port = uri.Port;
					break;
				case "server port":
					connectionInfo.Port = value;
					break;
				case "username":
					connectionInfo.Username = value;
					break;
				case "domain":
					connectionInfo.Domain = value;
					break;
				case "session bpp":
					switch (value) {
						case 8:
							connectionInfo.Colors = mRemoteNG.Connection.Protocol.RDP.RDPColors.Colors256;
							break;
						case 15:
							connectionInfo.Colors = mRemoteNG.Connection.Protocol.RDP.RDPColors.Colors15Bit;
							break;
						case 16:
							connectionInfo.Colors = mRemoteNG.Connection.Protocol.RDP.RDPColors.Colors16Bit;
							break;
						case 24:
							connectionInfo.Colors = mRemoteNG.Connection.Protocol.RDP.RDPColors.Colors24Bit;
							break;
						case 32:
							connectionInfo.Colors = mRemoteNG.Connection.Protocol.RDP.RDPColors.Colors32Bit;
							break;
					}
					break;
				case "bitmapcachepersistenable":
					if (value == 1) {
						connectionInfo.CacheBitmaps = true;
					} else {
						connectionInfo.CacheBitmaps = false;
					}
					break;
				case "screen mode id":
					if (value == 2) {
						connectionInfo.Resolution = mRemoteNG.Connection.Protocol.RDP.RDPResolutions.Fullscreen;
					} else {
						connectionInfo.Resolution = mRemoteNG.Connection.Protocol.RDP.RDPResolutions.FitToWindow;
					}
					break;
				case "connect to console":
					if (value == 1) {
						connectionInfo.UseConsoleSession = true;
					}
					break;
				case "disable wallpaper":
					if (value == 1) {
						connectionInfo.DisplayWallpaper = true;
					} else {
						connectionInfo.DisplayWallpaper = false;
					}
					break;
				case "disable themes":
					if (value == 1) {
						connectionInfo.DisplayThemes = true;
					} else {
						connectionInfo.DisplayThemes = false;
					}
					break;
				case "allow font smoothing":
					if (value == 1) {
						connectionInfo.EnableFontSmoothing = true;
					} else {
						connectionInfo.EnableFontSmoothing = false;
					}
					break;
				case "allow desktop composition":
					if (value == 1) {
						connectionInfo.EnableDesktopComposition = true;
					} else {
						connectionInfo.EnableDesktopComposition = false;
					}
					break;
				case "redirectsmartcards":
					if (value == 1) {
						connectionInfo.RedirectSmartCards = true;
					} else {
						connectionInfo.RedirectSmartCards = false;
					}
					break;
				case "redirectdrives":
					if (value == 1) {
						connectionInfo.RedirectDiskDrives = true;
					} else {
						connectionInfo.RedirectDiskDrives = false;
					}
					break;
				case "redirectcomports":
					if (value == 1) {
						connectionInfo.RedirectPorts = true;
					} else {
						connectionInfo.RedirectPorts = false;
					}
					break;
				case "redirectprinters":
					if (value == 1) {
						connectionInfo.RedirectPrinters = true;
					} else {
						connectionInfo.RedirectPrinters = false;
					}
					break;
				case "audiomode":
					switch (value) {
						case 0:
							connectionInfo.RedirectSound = mRemoteNG.Connection.Protocol.RDP.RDPSounds.BringToThisComputer;
							break;
						case 1:
							connectionInfo.RedirectSound = mRemoteNG.Connection.Protocol.RDP.RDPSounds.LeaveAtRemoteComputer;
							break;
						case 2:
							connectionInfo.RedirectSound = mRemoteNG.Connection.Protocol.RDP.RDPSounds.DoNotPlay;
							break;
					}
					break;
			}
		}
	}
}
