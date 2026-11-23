using System;
using System.Drawing;
using System.Runtime.Versioning;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.Connection;
using mRemoteNG.Container;
using mRemoteNG.Tree.Root;
using mRemoteNG.Themes;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace mRemoteNG.UI
{
    [SupportedOSPlatform("windows")]
    public class StatusImageList : IDisposable
    {
        public ImageList ImageList { get; }
        private readonly HashSet<string> _loadedIcons = new(StringComparer.Ordinal);

        public void ApplyTheme(int dpi = 96)
        {
            ImageList.ImageSize = new Size(Math.Max(1, (int)Math.Round(16 * dpi / 96d)), Math.Max(1, (int)Math.Round(16 * dpi / 96d)));
            _ = ImageList.Handle;
            ImageList.Images.Clear();
            ThemeManager manager = ThemeManager.getInstance();
            FillImageList(ImageList, manager.ActiveAndExtended ? manager.ActiveTheme : null);
            foreach (string icon in _loadedIcons) AddConnectionIcon(icon);
        }

        public StatusImageList()
        {
            DisplayProperties display = new();

            ImageList = new ImageList
            {
                ColorDepth = ColorDepth.Depth32Bit,
                ImageSize = new Size((int)Math.Round(16 * display.ResolutionScalingFactor.Width), (int)Math.Round(16 * display.ResolutionScalingFactor.Height)),
                TransparentColor = Color.Transparent
            };
            // Images are copied immediately when the native list exists, so temporary bitmaps can be disposed.
            _ = ImageList.Handle;

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
            if (connectionInfo is ContainerInfo { IsRootGroup: true }) return "Root";
            if (connectionInfo is ContainerInfo) return "Folder";

            return GetConnectionIcon(connectionInfo);
        }

        private static string BuildConnectionIconName(string icon, bool connected)
        {
            string status = connected ? "Play" : "Default";
            return $"Connection_{icon}_{status}";
        }

        private const string DefaultConnectionIcon = "";

        private string GetConnectionIcon(ConnectionInfo connection)
        {
            if (string.IsNullOrEmpty(connection.Icon))
            {
                return DefaultConnectionIcon;
            }

            bool connected = connection.OpenConnections.Count > 0;
            string name = BuildConnectionIconName(connection.Icon, connected);
            if (ImageList.Images.ContainsKey(name)) return name;
            if (!AddConnectionIcon(connection.Icon)) return DefaultConnectionIcon;
            _loadedIcons.Add(connection.Icon);
            return name;
        }

        private bool AddConnectionIcon(string name)
        {
            using Icon? icon = ConnectionIcon.FromString(name);
            if (icon == null) return false;
            ThemeManager manager = ThemeManager.getInstance();
            using Bitmap? fluent = manager.ActiveAndExtended && IsUnmodifiedDefaultIcon(name)
                ? ThemeIcons.Create("DefaultConnection", manager.ActiveTheme, true, ImageList.ImageSize.Width)
                : null;
            using Bitmap original = icon.ToBitmap();
            Image image = fluent ?? original;
            ImageList.Images.Add(BuildConnectionIconName(name, false), image);
            using Bitmap connected = Overlay(image, Properties.Resources.ConnectedOverlay);
            ImageList.Images.Add(BuildConnectionIconName(name, true), connected);
            return true;
        }

        // A user can replace even the built-in filename. Only replace the exact stock bytes.
        internal static bool IsUnmodifiedDefaultIcon(string name)
        {
            string path = Path.Combine(App.Info.GeneralAppInfo.HomePath, "Icons", "mRemoteNG.ico");
            return IsUnmodifiedDefaultIcon(name, path);
        }

        internal static bool IsUnmodifiedDefaultIcon(string name, string path)
        {
            if (!string.Equals(name, "mRemoteNG", StringComparison.Ordinal)) return false;
            try
            {
                if (!File.Exists(path)) return false;
                using Stream? stock = typeof(StatusImageList).Assembly.GetManifestResourceStream("mRemoteNG.Themes.FluentIcons.StockDefault.ico");
                if (stock == null) return false;
                using MemoryStream reference = new(); stock.CopyTo(reference);
                return File.ReadAllBytes(path).SequenceEqual(reference.ToArray());
            }
            catch (IOException) { return false; }
            catch (UnauthorizedAccessException) { return false; }
        }

        private Bitmap Overlay(Image background, Image foreground)
        {
            Bitmap result = new(background, ImageList.ImageSize);
            using (Graphics gr = Graphics.FromImage(result))
            {
                gr.DrawImage(foreground, new Rectangle(Point.Empty, ImageList.ImageSize));
            }

            return result;
        }

        private static void FillImageList(ImageList imageList, ThemeInfo? theme = null)
        {
            try
            {
                using Bitmap? root = theme != null ? ThemeIcons.Create("ASPWebSite_16x", theme, true, imageList.ImageSize.Width) : null;
                using Bitmap? folder = theme != null ? ThemeIcons.Create("FolderClosed_16x", theme, true, imageList.ImageSize.Width) : null;
                imageList.Images.Add("Root", root ?? Properties.Resources.ASPWebSite_16x);
                imageList.Images.Add("Folder", folder ?? Properties.Resources.FolderClosed_16x);
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
