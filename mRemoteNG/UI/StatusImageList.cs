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

        public Image? GetImage(ConnectionInfo connectionInfo)
        {
            string key = GetKey(connectionInfo);
            return ImageList.Images.ContainsKey(key)
                ? ImageList.Images[key]
                : null;
        }

        public string GetKey(ConnectionInfo? connectionInfo)
        {
            if (connectionInfo == null) return "";
            if (connectionInfo is RootPuttySessionsNodeInfo) return "PuttySessions";
            if (connectionInfo is RootNodeInfo) return "Root";
            if (connectionInfo is ContainerInfo) return "Folder";

            return GetConnectionIcon(connectionInfo);
        }

        private static string BuildConnectionIconName(string icon, bool connected, bool isTemplate = false)
        {
            string status = connected ? "Play" : isTemplate ? "Template" : "Default";
            return $"Connection_{icon}_{status}";
        }

        private const string DefaultConnectionIcon = "";

        private string GetConnectionIcon(ConnectionInfo connection)
        {
            if (string.IsNullOrEmpty(connection.Icon))
            {
                return DefaultConnectionIcon;
            }

            bool connected = connection.HasActiveSessions;
            bool isTemplate = connection.IsTemplate;
            bool replaceIcon = connected && Properties.OptionsAppearancePage.Default.ReplaceIconOnConnect;
            string name = isTemplate
                ? BuildConnectionIconName(connection.Icon, false, true)
                : replaceIcon
                    ? BuildConnectionIconNameReplace(connection.Icon)
                    : BuildConnectionIconName(connection.Icon, connected);
            if (ImageList.Images.ContainsKey(name)) return name;
            Icon? image = ConnectionIcon.FromString(connection.Icon);
            if (image == null)
            {
                return DefaultConnectionIcon;
            }

            ImageList.Images.Add(BuildConnectionIconName(connection.Icon, false), image);
            ImageList.Images.Add(BuildConnectionIconName(connection.Icon, true), Overlay(image, Properties.Resources.ConnectedOverlay));
            ImageList.Images.Add(BuildConnectionIconName(connection.Icon, false, true), CreateTemplateIcon(image));
            ImageList.Images.Add(BuildConnectionIconNameReplace(connection.Icon), CreateReplaceIcon());
            return name;
        }

        private static string BuildConnectionIconNameReplace(string icon)
        {
            return $"Connection_{icon}_Replace";
        }

        private static Bitmap CreateReplaceIcon()
        {
            return new Bitmap(Properties.Resources.Run_16x, new Size(16, 16));
        }

        private static Bitmap CreateTemplateIcon(Icon baseIcon)
        {
            Bitmap result = new(baseIcon.ToBitmap(), new Size(16, 16));
            using (Graphics gr = Graphics.FromImage(result))
            {
                // Draw a small "T" badge in the bottom-right corner
                using Font font = new("Arial", 7, FontStyle.Bold, GraphicsUnit.Pixel);
                using SolidBrush bgBrush = new(Color.FromArgb(200, 70, 130, 180));
                using SolidBrush fgBrush = new(Color.White);
                gr.FillRectangle(bgBrush, 9, 9, 7, 7);
                gr.DrawString("T", font, fgBrush, 9, 8);
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