using System;
using System.Drawing;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.Connection;
using mRemoteNG.Container;
using mRemoteNG.Tree.Root;


namespace mRemoteNG.UI.Controls
{
    public class StatusImageList : IDisposable
    {
        public ImageList ImageList { get; }

        public StatusImageList()
        {
            ImageList = new ImageList
            {
                ColorDepth = ColorDepth.Depth32Bit,
                ImageSize = new Size(16, 16),
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
            if (!ImageList.Images.ContainsKey(name))
            {
                var image = ConnectionIcon.FromString(connection.Icon);
                if (image == null)
                {
                    return DefaultConnectionIcon;
                }

                ImageList.Images.Add(BuildConnectionIconName(connection.Icon, false), image);
                ImageList.Images.Add(BuildConnectionIconName(connection.Icon, true), Overlay(image, Resources.ConnectedOverlay));

            }
            return name;
        }

        private static Bitmap Overlay(Icon background, Image foreground)
        {
            var result = background.ToBitmap();
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
                imageList.Images.Add("Root", Resources.Root);
                imageList.Images.Add("Folder", Resources.Folder);
                imageList.Images.Add("PuttySessions", Resources.PuttySessions);
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace($"Unable to fill the image list of type {nameof(StatusImageList)}", ex);
            }
        }

        public void Dispose()
        {
            ImageList?.Dispose();
        }
    }
}