using System;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.Versioning;
using mRemoteNG.Connection.Protocol.SSH_DotNet;

namespace mRemoteNG.UI.Controls
{
    [SupportedOSPlatform("windows")]
    public partial class SshTerminalControl : UserControl
    {
        #region Private Fields

        private const int DEFAULT_COLUMNS = 80;
        private const int DEFAULT_ROWS = 24;
        private const int DEFAULT_SCROLLBACK = 1000;

        private int _columns = DEFAULT_COLUMNS;
        private int _rows = DEFAULT_ROWS;
        private int _scrollbackLines = DEFAULT_SCROLLBACK;

        private Font _terminalFont;
        private Color _backgroundColor = Color.Black;
        private Color _foregroundColor = Color.White;

        private bool _isInitialized = false;
        private Panel _diagnosticOverlay;
        private Label _diagnosticLabel;

        #endregion

        #region Public Properties

        public int Columns
        {
            get => _columns;
            set
            {
                if (value < 1 || value > 500)
                {
                    SSHDotNetDiagnostics.LogWarning($"Terminal: Invalid column count {value}, using default {DEFAULT_COLUMNS}");
                    _columns = DEFAULT_COLUMNS;
                }
                else
                {
                    _columns = value;
                    SSHDotNetDiagnostics.LogDebug($"Terminal: Columns set to {_columns}");
                }
            }
        }

        public int Rows
        {
            get => _rows;
            set
            {
                if (value < 1 || value > 200)
                {
                    SSHDotNetDiagnostics.LogWarning($"Terminal: Invalid row count {value}, using default {DEFAULT_ROWS}");
                    _rows = DEFAULT_ROWS;
                }
                else
                {
                    _rows = value;
                    SSHDotNetDiagnostics.LogDebug($"Terminal: Rows set to {_rows}");
                }
            }
        }

        public int ScrollbackLines
        {
            get => _scrollbackLines;
            set
            {
                _scrollbackLines = Math.Max(0, Math.Min(value, 10000));
                SSHDotNetDiagnostics.LogDebug($"Terminal: Scrollback lines set to {_scrollbackLines}");
            }
        }

        public Font TerminalFont
        {
            get => _terminalFont ?? new Font("Consolas", 10);
            set
            {
                _terminalFont = value;
                SSHDotNetDiagnostics.LogDebug($"Terminal: Font set to {_terminalFont.Name} {_terminalFont.Size}pt");
                RecalculateDimensions();
            }
        }

        public Color TerminalBackColor
        {
            get => _backgroundColor;
            set
            {
                _backgroundColor = value;
                this.BackColor = value;
                SSHDotNetDiagnostics.LogDebug($"Terminal: Background color set to {value}");
            }
        }

        public Color TerminalForeColor
        {
            get => _foregroundColor;
            set
            {
                _foregroundColor = value;
                this.ForeColor = value;
                SSHDotNetDiagnostics.LogDebug($"Terminal: Foreground color set to {value}");
            }
        }

        public bool DiagnosticMode
        {
            get => _diagnosticOverlay?.Visible ?? false;
            set
            {
                if (_diagnosticOverlay != null)
                {
                    _diagnosticOverlay.Visible = value;
                    SSHDotNetDiagnostics.LogInfo($"Terminal: Diagnostic mode {(value ? "enabled" : "disabled")}");
                }
            }
        }

        #endregion

        #region Constructor

        public SshTerminalControl()
        {
            SSHDotNetDiagnostics.LogDebug("Terminal: Creating SshTerminalControl");

            InitializeComponent();
            InitializeDiagnosticOverlay();

            this.BackColor = _backgroundColor;
            this.ForeColor = _foregroundColor;
            this.DoubleBuffered = true;

            SSHDotNetDiagnostics.LogInfo($"Terminal: Initialized {_columns}x{_rows} terminal control");
            _isInitialized = true;
        }

        #endregion

        #region Initialization

        private void InitializeDiagnosticOverlay()
        {
            _diagnosticOverlay = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = Color.FromArgb(200, 50, 50, 50),
                Visible = false
            };

            _diagnosticLabel = new Label
            {
                Dock = DockStyle.Fill,
                ForeColor = Color.Yellow,
                Font = new Font("Consolas", 8),
                Text = "Diagnostic Mode: No data yet",
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(5)
            };

            _diagnosticOverlay.Controls.Add(_diagnosticLabel);
            this.Controls.Add(_diagnosticOverlay);
            _diagnosticOverlay.BringToFront();
        }

        public void Initialize()
        {
            if (_isInitialized)
            {
                SSHDotNetDiagnostics.LogWarning("Terminal: Already initialized");
                return;
            }

            SSHDotNetDiagnostics.LogDebug("Terminal: Performing initialization");

            try
            {
                // VtNetCore initialization will go here in Phase 3
                RecalculateDimensions();

                SSHDotNetDiagnostics.LogInfo("Terminal: Initialization complete");
                _isInitialized = true;
            }
            catch (Exception ex)
            {
                SSHDotNetDiagnostics.LogException("Terminal initialization failed", ex);
                throw;
            }
        }

        private void RecalculateDimensions()
        {
            // Calculate character dimensions based on font
            using (Graphics g = this.CreateGraphics())
            {
                SizeF charSize = g.MeasureString("W", TerminalFont);
                int charWidth = (int)Math.Ceiling(charSize.Width);
                int charHeight = (int)Math.Ceiling(charSize.Height);

                SSHDotNetDiagnostics.LogDebug($"Terminal: Character size calculated as {charWidth}x{charHeight}px");

                // Update dimensions based on control size
                if (this.Width > 0 && this.Height > 0)
                {
                    int newCols = Math.Max(1, this.Width / charWidth);
                    int newRows = Math.Max(1, this.Height / charHeight);

                    if (newCols != _columns || newRows != _rows)
                    {
                        _columns = newCols;
                        _rows = newRows;
                        SSHDotNetDiagnostics.LogInfo($"Terminal: Dimensions recalculated to {_columns}x{_rows}");
                    }
                }
            }
        }

        #endregion

        #region Placeholder Methods (To be implemented in Phase 3)

        public void AttachSshStream(System.IO.Stream sshStream)
        {
            SSHDotNetDiagnostics.LogInfo("Terminal: Attaching SSH stream (placeholder)");
            // Implementation in Phase 3
        }

        public void DetachSshStream()
        {
            SSHDotNetDiagnostics.LogInfo("Terminal: Detaching SSH stream (placeholder)");
            // Implementation in Phase 3
        }

        public void WriteOutput(string data)
        {
            SSHDotNetDiagnostics.LogDebug($"Terminal: WriteOutput called with {data?.Length ?? 0} characters (placeholder)");

            if (DiagnosticMode && _diagnosticLabel != null)
            {
                _diagnosticLabel.Text = $"Diagnostic Mode: {data?.Length ?? 0} chars received";
            }
            // Implementation in Phase 3
        }

        public void ReadInput(out byte[] inputData)
        {
            SSHDotNetDiagnostics.LogDebug("Terminal: ReadInput called (placeholder)");
            inputData = Array.Empty<byte>();
            // Implementation in Phase 3
        }

        #endregion

        #region Designer Support

        private void InitializeComponent()
        {
            this.SuspendLayout();

            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.Name = "SshTerminalControl";
            this.Size = new Size(800, 600);

            this.ResumeLayout(false);
        }

        #endregion
    }
}
