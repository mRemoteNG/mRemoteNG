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
            DisplayProperties display = new();

            ImageList = new ImageList
            {
                ColorDepth = ColorDepth.Depth32Bit,
                ImageSize = new Size((int)Math.Round(16 * display.ResolutionScalingFactor.Width), (int)Math.Round(16 * display.ResolutionScalingFactor.Height)),
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
            string key = GetKey(connectionInfo);
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
            string status = connected ? "Play" : "Default";
            return $"Connection_{icon}_{status}";
        }

        private static string BuildStatusIconName(string baseKey, HostStatus hostStatus)
        {
            return $"{baseKey}_{hostStatus}";
        }

        private const string DefaultConnectionIcon = "";

        private string GetConnectionIcon(ConnectionInfo connection)
        {
            if (string.IsNullOrEmpty(connection.Icon))
            {
                return DefaultConnectionIcon;
            }

            bool connected = connection.OpenConnections.Count > 0;
            string baseKey = BuildConnectionIconName(connection.Icon, connected);

            bool showStatusIndicator = Properties.OptionsAppearancePage.Default.ShowStatusIndicatorInTree;
            string name = showStatusIndicator
                ? BuildStatusIconName(baseKey, connection.HostStatus)
                : baseKey;

            if (ImageList.Images.ContainsKey(name)) return name;

            Icon image = ConnectionIcon.FromString(connection.Icon);
            if (image == null)
            {
                return DefaultConnectionIcon;
            }

            Bitmap defaultBitmap = image.ToBitmap();
            Bitmap playBitmap = Overlay(image, Properties.Resources.ConnectedOverlay);

            if (showStatusIndicator)
            {
                foreach (HostStatus status in Enum.GetValues<HostStatus>())
                {
                    Color barColor = GetStatusColor(status);
                    string defaultStatusKey = BuildStatusIconName(BuildConnectionIconName(connection.Icon, false), status);
                    string playStatusKey = BuildStatusIconName(BuildConnectionIconName(connection.Icon, true), status);

                    if (!ImageList.Images.ContainsKey(defaultStatusKey))
                        ImageList.Images.Add(defaultStatusKey, AddStatusBar(defaultBitmap, barColor));
                    if (!ImageList.Images.ContainsKey(playStatusKey))
                        ImageList.Images.Add(playStatusKey, AddStatusBar(playBitmap, barColor));
                }

                defaultBitmap.Dispose();
                playBitmap.Dispose();
            }
            else
            {
                if (!ImageList.Images.ContainsKey(BuildConnectionIconName(connection.Icon, false)))
                    ImageList.Images.Add(BuildConnectionIconName(connection.Icon, false), defaultBitmap);
                else
                    defaultBitmap.Dispose();

                if (!ImageList.Images.ContainsKey(BuildConnectionIconName(connection.Icon, true)))
                    ImageList.Images.Add(BuildConnectionIconName(connection.Icon, true), playBitmap);
                else
                    playBitmap.Dispose();
            }

            return name;
        }

        private static Color GetStatusColor(HostStatus status)
        {
            return status switch
            {
                HostStatus.Online => Color.FromArgb(0, 180, 0),
                HostStatus.Offline => Color.FromArgb(200, 0, 0),
                _ => Color.FromArgb(160, 160, 160)
            };
        }

        private static Bitmap AddStatusBar(Bitmap source, Color barColor)
        {
            Bitmap result = new(source.Width, source.Height);
            using (Graphics gr = Graphics.FromImage(result))
            {
                gr.DrawImage(source, new Rectangle(0, 0, source.Width, source.Height));
                using SolidBrush brush = new(barColor);
                gr.FillRectangle(brush, 0, 0, 3, source.Height);
            }

            return result;
        }

        private static Bitmap Overlay(Icon background, Image foreground)
        {
            Bitmap result = new(background.ToBitmap(), new Size(16, 16));
            using (Graphics gr = Graphics.FromImage(result))
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
                Runtime.MessageCollector.AddExceptionStackTrace($"Unable to fill the image list of type {nameof(StatusImageList)}", ex);
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