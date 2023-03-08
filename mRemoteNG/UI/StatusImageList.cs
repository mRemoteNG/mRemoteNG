using System;
using System.Drawing;
using System.Runtime.Versioning;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.Connection;
using mRemoteNG.Container;
using mRemoteNG.Tree.Root;

namespace mRemoteNG.UI
{
    [SupportedOSPlatform("windows")]
    public class StatusImageList : IDisposable
    {
        public ImageList ImageList { get; }

        public StatusImageList()
        {
            var display = new DisplayProperties();

            ImageList = new ImageList
            {
                ColorDepth = ColorDepth.Depth32Bit,
                ImageSize = new Size(
                                     (int)Math.Round(16 * display.ResolutionScalingFactor.Width),
                                     (int)Math.Round(16 * display.ResolutionScalingFactor.Height)),
                TransparentColor = Color.Transparent
            };

            FillImageList(ImageList);
        }

        public object ImageGetter(object rowObject)
        {
            return GetKey(rowObject as ConnectionInfo);
        }

        public Image GetImage(ConnectionInfo connectionInfo)
        {
            var key = GetKey(connectionInfo);
            return ImageList.Images.ContainsKey(key)
                ? ImageList.Images[key]
                : null;
        }

        public string GetKey(ConnectionInfo connectionInfo)
        {
            if (connectionInfo == null) return "";
            if (connectionInfo is RootPuttySessionsNodeInfo) return "PuttySessions";
            if (connectionInfo is RootNodeInfo) return "Root";
            if (connectionInfo is ContainerInfo) return "Folder";

            return GetConnectionIcon(connectionInfo);
        }

        private static string BuildConnectionIconName(string icon, bool connected)
        {
            var status = connected ? "Play" : "Default";
            return $"Connection_{icon}_{status}";
        }

        private const string DefaultConnectionIcon = "";

        private string GetConnectionIcon(ConnectionInfo connection)
        {
            if (string.IsNullOrEmpty(connection.Icon))
            {
                return DefaultConnectionIcon;
            }

            var connected = connection.OpenConnections.Count > 0;
            var name = BuildConnectionIconName(connection.Icon, connected);
            if (ImageList.Images.ContainsKey(name)) return name;
            var image = ConnectionIcon.FromString(connection.Icon);
            if (image == null)
            {
                return DefaultConnectionIcon;
            }

            ImageList.Images.Add(BuildConnectionIconName(connection.Icon, false), image);
            ImageList.Images.Add(BuildConnectionIconName(connection.Icon, true),
                                 Overlay(image, Properties.Resources.ConnectedOverlay));
            return name;
        }

        private static Bitmap Overlay(Icon background, Image foreground)
        {
            var result = new Bitmap(background.ToBitmap(), new Size(16, 16));
            using (var gr = Graphics.FromImage(result))
            {
                gr.DrawImage(foreground, new Rectangle(0, 0, foreground.Width, foreground.Height));
            }

            return result;
        }

        private static void FillImageList(ImageList imageList)
        {
            try
            {
                imageList.Images.Add("Root", Properties.Resources.ASPWebSite_16x);
                imageList.Images.Add("Folder", Properties.Resources.FolderClosed_16x);
                imageList.Images.Add("PuttySessions", Properties.Resources.PuttySessions);
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace(
                                                                $"Unable to fill the image list of type {nameof(StatusImageList)}",
                                                                ex);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                ImageList?.Dispose();
            }
        }
    }
}