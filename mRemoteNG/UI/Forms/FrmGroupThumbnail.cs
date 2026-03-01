using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Versioning;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.Connection;
using mRemoteNG.Container;
using mRemoteNG.Tree;

namespace mRemoteNG.UI.Forms
{
    /// <summary>
    /// Displays a thumbnail tile grid of all connections within a group (ContainerInfo).
    /// Each tile shows the connection icon, name, hostname and connected/disconnected status.
    /// Double-clicking a tile initiates the connection.
    /// </summary>
    [SupportedOSPlatform("windows")]
    public class FrmGroupThumbnail : Form
    {
        private readonly ContainerInfo _container;
        private readonly FlowLayoutPanel _flowPanel;
        private readonly Label _lblStatus;
        private readonly System.Windows.Forms.Timer _refreshTimer;
        private readonly List<ConnectionTile> _tiles = new();

        private const int TileWidth = 160;
        private const int TileHeight = 115;

        public FrmGroupThumbnail(ContainerInfo container)
        {
            _container = container;

            Text = $"Thumbnails — {container.Name}";
            Size = new Size(700, 500);
            MinimumSize = new Size(400, 300);
            StartPosition = FormStartPosition.CenterParent;
            ShowInTaskbar = false;
            Font = new Font("Segoe UI", 8.25f);

            _flowPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                Padding = new Padding(8),
                BackColor = SystemColors.Window
            };

            var pnlBottom = new Panel
            {
                Height = 36,
                Dock = DockStyle.Bottom,
                BackColor = SystemColors.Control
            };

            _lblStatus = new Label
            {
                AutoSize = false,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(8, 0, 0, 0)
            };

            var btnRefresh = new Button
            {
                Text = "Refresh",
                Width = 75,
                Height = 24,
                Top = 6,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            btnRefresh.Click += (s, e) => BuildTiles();

            var btnClose = new Button
            {
                Text = "Close",
                Width = 75,
                Height = 24,
                Top = 6,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            btnClose.Click += (s, e) => Close();

            pnlBottom.Controls.AddRange(new Control[] { _lblStatus, btnRefresh, btnClose });
            pnlBottom.Layout += (s, e) =>
            {
                btnClose.Left = pnlBottom.Width - btnClose.Width - 8;
                btnRefresh.Left = btnClose.Left - btnRefresh.Width - 4;
            };

            Controls.Add(_flowPanel);
            Controls.Add(pnlBottom);

            _refreshTimer = new System.Windows.Forms.Timer { Interval = 5000 };
            _refreshTimer.Tick += (s, e) => RefreshTileStatus();
            _refreshTimer.Start();

            BuildTiles();
        }

        private void BuildTiles()
        {
            _flowPanel.SuspendLayout();
            _flowPanel.Controls.Clear();
            _tiles.Clear();

            var connections = _container.GetRecursiveChildList()
                .Where(c => c.GetTreeNodeType() == TreeNodeType.Connection ||
                            c.GetTreeNodeType() == TreeNodeType.PuttySession)
                .ToList();

            foreach (var connection in connections)
            {
                var tile = new ConnectionTile(connection, TileWidth, TileHeight);
                tile.ConnectRequested += OnTileConnectRequested;
                _tiles.Add(tile);
                _flowPanel.Controls.Add(tile);
            }

            _flowPanel.ResumeLayout();
            UpdateStatusLabel(connections.Count);
        }

        private void RefreshTileStatus()
        {
            foreach (var tile in _tiles)
                tile.UpdateStatus();

            UpdateStatusLabel(_tiles.Count);
        }

        private void UpdateStatusLabel(int total)
        {
            int connected = _tiles.Count(t => t.IsConnected);
            _lblStatus.Text = $"{total} connection(s) — {connected} connected";
        }

        private void OnTileConnectRequested(ConnectionInfo connection)
        {
            Runtime.ConnectionInitiator.OpenConnection(connection, ConnectionInfo.Force.DoNotJump);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _refreshTimer.Stop();
                _refreshTimer.Dispose();
            }

            base.Dispose(disposing);
        }

        // ─── Inner class: one tile per connection ─────────────────────────────────
        private sealed class ConnectionTile : Panel
        {
            private readonly ConnectionInfo _connection;
            private readonly PictureBox _icon;
            private readonly Label _lblName;
            private readonly Label _lblHost;
            private readonly Label _lblStatus;
            private readonly Panel _statusBar;

            public bool IsConnected => _connection.OpenConnections.Count > 0;

            public event Action<ConnectionInfo>? ConnectRequested;

            public ConnectionTile(ConnectionInfo connection, int width, int height)
            {
                _connection = connection;
                Width = width;
                Height = height;
                Margin = new Padding(4);
                Cursor = Cursors.Hand;
                BackColor = SystemColors.Window;
                BorderStyle = BorderStyle.FixedSingle;

                // Coloured bar at top indicating status
                _statusBar = new Panel
                {
                    Dock = DockStyle.Top,
                    Height = 4,
                    BackColor = Color.LightGray
                };

                // Connection icon, centred horizontally
                _icon = new PictureBox
                {
                    Width = 32,
                    Height = 32,
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    Top = 12,
                    Left = (width - 2 - 32) / 2,
                    BackColor = Color.Transparent
                };

                // Connection name (bold)
                _lblName = new Label
                {
                    AutoSize = false,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Font = new Font("Segoe UI", 8.25f, FontStyle.Bold),
                    Width = width - 10,
                    Height = 32,
                    Top = _icon.Bottom + 4,
                    Left = 4,
                    BackColor = Color.Transparent
                };

                // Hostname in muted colour
                _lblHost = new Label
                {
                    AutoSize = false,
                    TextAlign = ContentAlignment.MiddleCenter,
                    ForeColor = SystemColors.GrayText,
                    Width = width - 10,
                    Height = 20,
                    Top = _lblName.Bottom,
                    Left = 4,
                    BackColor = Color.Transparent
                };

                // Status text
                _lblStatus = new Label
                {
                    AutoSize = false,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Width = width - 10,
                    Height = 18,
                    Top = _lblHost.Bottom,
                    Left = 4,
                    BackColor = Color.Transparent
                };

                Controls.AddRange(new Control[] { _statusBar, _icon, _lblName, _lblHost, _lblStatus });

                // Attach events to all child controls so hover/click works anywhere on the tile
                foreach (Control ctrl in Controls)
                {
                    ctrl.DoubleClick += OnTileDoubleClick;
                    ctrl.MouseEnter += OnChildMouseEnter;
                    ctrl.MouseLeave += OnChildMouseLeave;
                }
                DoubleClick += OnTileDoubleClick;
                MouseEnter += OnChildMouseEnter;
                MouseLeave += OnChildMouseLeave;

                LoadIcon();
                UpdateStatus();
            }

            private void LoadIcon()
            {
                if (!string.IsNullOrEmpty(_connection.Icon))
                {
                    var icon = ConnectionIcon.FromString(_connection.Icon);
                    if (icon != null)
                    {
                        _icon.Image = icon.ToBitmap();
                        return;
                    }
                }

                _icon.Image = SystemIcons.Application.ToBitmap();
            }

            public void UpdateStatus()
            {
                bool connected = IsConnected;
                _statusBar.BackColor = connected ? Color.MediumSeaGreen : Color.LightGray;
                _lblStatus.Text = connected ? "Connected" : "Disconnected";
                _lblStatus.ForeColor = connected ? Color.MediumSeaGreen : SystemColors.GrayText;
                _lblName.Text = _connection.Name;
                _lblHost.Text = _connection.Hostname;
            }

            private void OnTileDoubleClick(object? sender, EventArgs e)
            {
                ConnectRequested?.Invoke(_connection);
            }

            private void OnChildMouseEnter(object? sender, EventArgs e)
            {
                BackColor = Color.AliceBlue;
            }

            private void OnChildMouseLeave(object? sender, EventArgs e)
            {
                // Only reset when the cursor has truly left the tile bounds
                if (!ClientRectangle.Contains(PointToClient(Cursor.Position)))
                    BackColor = SystemColors.Window;
            }
        }
    }
}
