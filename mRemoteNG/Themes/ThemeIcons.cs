using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace mRemoteNG.Themes
{
    /// <summary>Optional Fluent icons. Each binding owns its rendered image; stock resources stay untouched.</summary>
    internal static class ThemeIcons
    {
        private static readonly Dictionary<string, string> Assets = new(StringComparer.Ordinal)
        {
            ["Add_16x"] = "expand", ["AddItem_16x"] = "new-connection", ["NewFile_16x"] = "new-connection",
            ["AddFolder_16x"] = "new-folder", ["FolderClosed_16x"] = "folder", ["OpenFile_16x"] = "folder-open",
            ["ASPWebSite_16x"] = "folder-open", ["Monitor_16x"] = "computer", ["Run_16x"] = "connect",
            ["Stop_16x"] = "disconnect", ["Search_16x"] = "search", ["Save_16x"] = "save",
            ["SaveAll_16x"] = "save", ["Settings_16x"] = "settings", ["Refresh_16x"] = "refresh",
            ["ExpandAll_16x"] = "expand", ["CollapseAll_16x"] = "collapse", ["Remove_16x"] = "collapse",
            ["Filter_16x"] = "filter", ["Favorite_16x"] = "favorite", ["Copy_16x"] = "copy",
            ["Close_16x"] = "dismiss", ["DefaultConnection"] = "computer",
            ["Rename_16x"] = "rename", ["SortAscending_16x"] = "sort-ascending", ["SortDescending_16x"] = "sort-descending",
            ["Schema_16x"] = "inheritance", ["FileProperties_16x"] = "properties", ["Properties_16x"] = "properties",
            ["Property_16x"] = "settings", ["ExtendedProperty_16x"] = "properties", ["ViewDownBySchema_16x"] = "inheritance",
            ["Panel_16x"] = "panel", ["InsertPanel_16x"] = "panel-add", ["FullScreen_16x"] = "fullscreen",
            ["F1Help_16x"] = "help", ["UIAboutBox_16x"] = "info", ["RunUpdate_16x"] = "update",
            ["Console_16x"] = "terminal", ["CloseSolution_16x"] = "disconnect",
            ["Tab_16x"] = "panel-add", ["LogError_16x"] = "alert",
            ["Key_16x"] = "key", ["SQLDatabase_16x"] = "database", ["Lock_16x"] = "lock_closed",
            ["AppearanceEditor_16x"] = "paint_brush", ["OptionsPlugins"] = "puzzle_piece",
            ["DocumentsFolder_16x"] = "folder"
        };
        private static readonly ConditionalWeakTable<object, Binding> Bindings = new();
        private static readonly List<WeakReference<Binding>> LiveBindings = [];
        private static readonly ConditionalWeakTable<Form, TabBinding> TabBindings = new();
        private static readonly List<WeakReference<TabBinding>> LiveTabs = [];
        private static ThemeManager? _manager;

        internal static bool IsMapped(string key) => Assets.ContainsKey(key);

        internal static Bitmap? Create(string key, ThemeInfo theme, bool enabled, int size)
        {
            if (!theme.UseFluentIcons || !Assets.TryGetValue(key, out string? asset)) return null;
            if (size < 1 || size > 256) throw new ArgumentOutOfRangeException(nameof(size));
            Color color = enabled ? theme.IconColor : theme.DisabledIconColor;
            if (color.IsEmpty)
                color = theme.IsExtended
                    ? theme.ExtendedPalette.getColor(enabled ? "Dialog_Foreground" : "TextBox_Disabled_Foreground")
                    : enabled ? SystemColors.ControlText : SystemColors.GrayText;
            using Stream archiveStream = typeof(ThemeIcons).Assembly.GetManifestResourceStream("mRemoteNG.Themes.FluentIcons.zip")
                ?? throw new InvalidOperationException("Missing Fluent icon bundle.");
            using ZipArchive archive = new(archiveStream, ZipArchiveMode.Read);
            string resource = $"{(size <= 16 ? "16" : "48")}/{asset}.png";
            using Stream stream = (archive.GetEntry(resource)
                ?? throw new InvalidOperationException($"Missing Fluent icon resource: {resource}")).Open();
            using Image mask = Image.FromStream(stream);
            Bitmap result = new(size, size, PixelFormat.Format32bppArgb);
            try
            {
                using (Graphics graphics = Graphics.FromImage(result))
                {
                    graphics.InterpolationMode = size == mask.Width ? InterpolationMode.NearestNeighbor : InterpolationMode.HighQualityBicubic;
                    graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    graphics.DrawImage(mask, new Rectangle(0, 0, size, size));
                }
                // Resize alpha coverage first, then set straight RGB. A translating color
                // matrix during bicubic sampling produces bright fringes on thin outlines.
                BitmapData pixels = result.LockBits(new Rectangle(0, 0, size, size), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
                try
                {
                    byte[] row = new byte[size * 4];
                    for (int y = 0; y < size; y++)
                    {
                        IntPtr address = IntPtr.Add(pixels.Scan0, y * pixels.Stride);
                        Marshal.Copy(address, row, 0, row.Length);
                        for (int x = 0; x < row.Length; x += 4)
                        {
                            row[x] = color.B; row[x + 1] = color.G; row[x + 2] = color.R;
                            row[x + 3] = (byte)((row[x + 3] * color.A + 127) / 255);
                        }
                        Marshal.Copy(row, 0, address, row.Length);
                    }
                }
                finally { result.UnlockBits(pixels); }
                return result;
            }
            catch { result.Dispose(); throw; }
        }

        internal static void Bind(object target, string key)
            => Bind(target, key, null);

        internal static void BindConnectionIcon(ToolStripItem target, Connection.ConnectionInfo connection)
            => Bind(target, "DefaultConnection", connection);

        internal static void Unbind(object target)
        {
            if (Bindings.TryGetValue(target, out Binding? binding)) binding.Dispose();
            Bindings.Remove(target);
        }

        private static void Bind(object target, string key, Connection.ConnectionInfo? connection)
        {
            if (!IsMapped(key)) throw new ArgumentException("Unknown themed icon.", nameof(key));
            _manager ??= ThemeManager.getInstance();
            if (Bindings.TryGetValue(target, out Binding? previous)) previous.Dispose();
            Bindings.Remove(target);
            Binding binding = new(target, key, connection);
            Bindings.Add(target, binding);
            LiveBindings.Add(new WeakReference<Binding>(binding));
            // One weak registry, rather than a singleton subscription rooting every menu item.
            _manager.ThemeChanged -= RefreshAll;
            _manager.ThemeChanged += RefreshAll;
            binding.Update();
        }

        private static void RefreshAll()
        {
            for (int i = LiveBindings.Count - 1; i >= 0; i--)
            {
                if (!LiveBindings[i].TryGetTarget(out Binding? binding) || binding.Disposed)
                    LiveBindings.RemoveAt(i);
                else binding.Update();
            }
            for (int i = LiveTabs.Count - 1; i >= 0; i--)
                if (!LiveTabs[i].TryGetTarget(out TabBinding? tab) || tab.Disposed) LiveTabs.RemoveAt(i); else tab.Update();
        }

        internal static void BindConnectionTab(Form form, Connection.ConnectionInfo connection)
        {
            _manager ??= ThemeManager.getInstance();
            if (TabBindings.TryGetValue(form, out TabBinding? old)) old.Dispose();
            TabBindings.Remove(form);
            TabBinding binding = new(form, connection);
            TabBindings.Add(form, binding); LiveTabs.Add(new WeakReference<TabBinding>(binding));
            _manager.ThemeChanged -= RefreshAll; _manager.ThemeChanged += RefreshAll;
            binding.Update();
        }

        private sealed class TabBinding : IDisposable
        {
            private readonly Form _form;
            private readonly Connection.ConnectionInfo _connection;
            private Icon? _fallback;
            private Icon? _owned;
            private string? _name;
            internal bool Disposed { get; private set; }
            internal TabBinding(Form form, Connection.ConnectionInfo connection)
            {
                _form = form; _connection = connection;
                _fallback = Connection.ConnectionIcon.FromString(connection.Icon); _name = connection.Icon;
                form.Disposed += OnDisposed; form.DpiChanged += OnChanged;
            }
            private void OnDisposed(object? sender, EventArgs e) => Dispose();
            private void OnChanged(object? sender, EventArgs e) => Update();
            internal void Update()
            {
                if (Disposed || _form.IsDisposed || _form.Disposing) return;
                if (_form.InvokeRequired)
                {
                    try { _form.BeginInvoke(new MethodInvoker(Update)); }
                    catch (InvalidOperationException) when (_form.IsDisposed || !_form.IsHandleCreated) { }
                    return;
                }
                if (_name != _connection.Icon)
                {
                    Icon? previous = _fallback;
                    _fallback = Connection.ConnectionIcon.FromString(_connection.Icon); _name = _connection.Icon;
                    if (_owned == null) _form.Icon = _fallback;
                    previous?.Dispose();
                }
                Icon? next = null;
                if (_manager!.ActiveAndExtended && UI.StatusImageList.IsUnmodifiedDefaultIcon(_connection.Icon))
                {
                    using Bitmap? image = Create("DefaultConnection", _manager.ActiveTheme, true, Math.Max(1, (int)Math.Round(16 * _form.DeviceDpi / 96d)));
                    if (image != null)
                    {
                        IntPtr handle = image.GetHicon();
                        try { next = (Icon)Icon.FromHandle(handle).Clone(); }
                        finally { DestroyIcon(handle); }
                    }
                }
                Icon? old = _owned; _owned = next; _form.Icon = next ?? _fallback; old?.Dispose();
            }
            public void Dispose()
            {
                if (Disposed) return;
                Disposed = true; _form.Disposed -= OnDisposed; _form.DpiChanged -= OnChanged;
                _owned?.Dispose(); _fallback?.Dispose(); _owned = null; _fallback = null;
            }
        }

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool DestroyIcon(IntPtr handle);

        private sealed class Binding : IDisposable
        {
            private readonly object _target;
            private readonly string _key;
            private Image? _fallback;
            private readonly Connection.ConnectionInfo? _connection;
            private string? _connectionIcon;
            private Bitmap? _owned;
            private Control? _owner;
            internal bool Disposed { get; private set; }

            internal Binding(object target, string key, Connection.ConnectionInfo? connection)
            {
                _target = target; _key = key;
                _connection = connection;
                if (connection == null) _fallback = (Image)Properties.Resources.ResourceManager.GetObject(key)!;
                else ReloadConnectionIcon();
                if (target is ToolStripItem item)
                {
                    item.Disposed += OnDisposed; item.EnabledChanged += OnChanged; item.OwnerChanged += OnOwnerChanged;
                    SetOwner(item.GetCurrentParent());
                }
                else if (target is Control control && (target is PictureBox || target is ButtonBase))
                {
                    control.Disposed += OnDisposed; control.EnabledChanged += OnChanged;
                    SetOwner(control);
                }
                else throw new ArgumentException("Expected an image-bearing control or ToolStripItem.", nameof(target));
            }

            private void SetOwner(Control? owner)
            {
                if (_owner != null) _owner.DpiChangedAfterParent -= OnChanged;
                _owner = owner;
                if (_owner != null) _owner.DpiChangedAfterParent += OnChanged;
            }
            private void OnOwnerChanged(object? sender, EventArgs e) { SetOwner(((ToolStripItem)_target).GetCurrentParent()); Update(); }
            private void OnChanged(object? sender, EventArgs e) => Update();
            private void OnDisposed(object? sender, EventArgs e) => Dispose();

            private void ReloadConnectionIcon()
            {
                using Icon? icon = Connection.ConnectionIcon.FromString(_connection!.Icon);
                Image? old = _fallback; _fallback = icon?.ToBitmap(); _connectionIcon = _connection.Icon; old?.Dispose();
            }

            internal void Update()
            {
                if (Disposed) return;
                if (_owner?.IsDisposed == true || _owner?.Disposing == true) return;
                if (_owner?.InvokeRequired == true)
                {
                    try { _owner.BeginInvoke(new MethodInvoker(Update)); }
                    catch (InvalidOperationException) when (_owner.IsDisposed || !_owner.IsHandleCreated) { }
                    return;
                }
                bool enabled = _target is ToolStripItem item ? item.Enabled : ((Control)_target).Enabled;
                if (_connection != null && _connection.Icon != _connectionIcon) ReloadConnectionIcon();
                bool replace = _connection == null || UI.StatusImageList.IsUnmodifiedDefaultIcon(_connection.Icon);
                Bitmap? next = _manager!.ActiveAndExtended && replace
                    ? Create(_key, _manager.ActiveTheme, enabled, Math.Max(1, (int)Math.Round(16 * (_owner?.DeviceDpi ?? 96) / 96d)))
                    : null;
                Bitmap? old = _owned;
                _owned = next;
                SetImage(next ?? _fallback);
                old?.Dispose();
            }

            private void SetImage(Image? image)
            {
                if (_target is ToolStripItem item) { item.Image = image; item.Invalidate(); }
                else if (_target is PictureBox picture) { picture.Image = image; picture.Invalidate(); }
                else if (_target is ButtonBase button) { button.Image = image; button.Invalidate(); }
            }

            public void Dispose()
            {
                if (Disposed) return;
                Disposed = true;
                if (_target is ToolStripItem item)
                {
                    item.Disposed -= OnDisposed; item.EnabledChanged -= OnChanged; item.OwnerChanged -= OnOwnerChanged;
                    if (!item.IsDisposed) SetImage(_connection == null ? _fallback : null);
                }
                else if (_target is Control control)
                {
                    control.Disposed -= OnDisposed; control.EnabledChanged -= OnChanged;
                    if (!control.IsDisposed) SetImage(_connection == null ? _fallback : null);
                }
                SetOwner(null);
                _owned?.Dispose(); _owned = null;
                if (_connection != null) { _fallback?.Dispose(); _fallback = null; }
            }
        }
    }
}
