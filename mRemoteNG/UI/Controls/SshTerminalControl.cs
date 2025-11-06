using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Runtime.Versioning;
using System.Collections.Generic;
using System.Linq;
using mRemoteNG.Connection.Protocol.SSH_DotNet;
using VtNetCore.VirtualTerminal;
using VtNetCore.XTermParser;

namespace mRemoteNG.UI.Controls
{
    [SupportedOSPlatform("windows")]
    public partial class SshTerminalControl : UserControl
    {
        #region Private Fields

        private const int DEFAULT_COLUMNS = 80;
        private const int DEFAULT_ROWS = 24;
        private const int DEFAULT_SCROLLBACK = 1000;
        private const int CHAR_WIDTH = 8;
        private const int CHAR_HEIGHT = 16;

        private int _columns = DEFAULT_COLUMNS;
        private int _rows = DEFAULT_ROWS;
        private int _scrollbackLines = DEFAULT_SCROLLBACK;

        private Font _terminalFont;
        private Color _backgroundColor = Color.Black;
        private Color _foregroundColor = Color.White;

        private bool _isInitialized = false;
        private Panel _diagnosticOverlay;
        private Label _diagnosticLabel;
        private bool _hasLoggedFirstRender = false;
        private bool _hasLoggedEmptyScreen = false;

        // VtNetCore terminal emulation
        private VirtualTerminalController _vtController;
        private DataConsumer _dataConsumer;
        private List<string> _scrollbackBuffer;
        private int _scrollbackPosition = 0;

        // Input handling
        private StringBuilder _inputBuffer;
        private Stream _sshStream;
        private bool _streamAttached = false;

        // Selection/Copy-Paste state
        private Point _selectionStart = Point.Empty;
        private Point _selectionEnd = Point.Empty;
        private bool _isSelecting = false;
        private ContextMenuStrip _contextMenu;

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

            this.BackColor = _backgroundColor;
            this.ForeColor = _foregroundColor;
            this.DoubleBuffered = true;

            // Note: Do NOT set _isInitialized here - let Initialize() do the full setup
            SSHDotNetDiagnostics.LogDebug("Terminal: Constructor complete, waiting for Initialize() call");
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
                SSHDotNetDiagnostics.LogDebug("Terminal: Already initialized, skipping re-initialization");
                return;
            }

            SSHDotNetDiagnostics.LogDebug("Terminal: Performing initialization");

            try
            {
                // Initialize VtNetCore terminal emulation
                _vtController = new VirtualTerminalController();
                _vtController.ResizeView(_columns, _rows);
                _dataConsumer = new DataConsumer(_vtController);

                SSHDotNetDiagnostics.LogInfo($"Terminal: Created VtNetCore controller and DataConsumer ({_columns}x{_rows})");

                // Initialize scrollback buffer
                _scrollbackBuffer = new List<string>();
                _inputBuffer = new StringBuilder();

                // Event handlers
                this.KeyDown += SshTerminalControl_KeyDown;
                this.KeyPress += SshTerminalControl_KeyPress;
                this.MouseDown += SshTerminalControl_MouseDown;
                this.MouseMove += SshTerminalControl_MouseMove;
                this.MouseUp += SshTerminalControl_MouseUp;
                this.Resize += SshTerminalControl_Resize;

                // Terminal font
                _terminalFont = new Font("Courier New", 10f, FontStyle.Regular);

                // Initialize diagnostic overlay if needed
                if (DiagnosticMode)
                {
                    InitializeDiagnosticOverlay();
                }

                // Initialize context menu
                InitializeContextMenu();

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

        #region Stream Management

        public void AttachSshStream(System.IO.Stream sshStream)
        {
            try
            {
                _sshStream = sshStream;
                _streamAttached = true;
                SSHDotNetDiagnostics.LogInfo("Terminal: SSH stream attached");
            }
            catch (Exception ex)
            {
                SSHDotNetDiagnostics.LogException("Terminal: Failed to attach SSH stream", ex);
            }
        }

        public void DetachSshStream()
        {
            try
            {
                _sshStream = null;
                _streamAttached = false;
                SSHDotNetDiagnostics.LogInfo("Terminal: SSH stream detached");
            }
            catch (Exception ex)
            {
                SSHDotNetDiagnostics.LogException("Terminal: Failed to detach SSH stream", ex);
            }
        }

        #endregion

        #region Output Handling

        public void WriteOutput(string data)
        {
            if (string.IsNullOrEmpty(data))
                return;

            try
            {
                // Process data through VtNetCore terminal controller using DataConsumer
                if (_dataConsumer != null)
                {
                    _dataConsumer.Push(Encoding.UTF8.GetBytes(data));
                    SSHDotNetDiagnostics.LogDebug($"Terminal: Pushed {data.Length} bytes to VtNetCore DataConsumer");
                }
                else
                {
                    SSHDotNetDiagnostics.LogWarning("Terminal: DataConsumer is null, cannot process output");
                }

                // Update scrollback buffer
                AddToScrollback(data);

                // Update diagnostic mode if enabled
                if (DiagnosticMode && _diagnosticLabel != null)
                {
                    this.Invoke((Action)(() =>
                    {
                        _diagnosticLabel.Text = $"Terminal: {_columns}x{_rows}, Scrollback: {_scrollbackBuffer.Count}, Input: {data?.Length ?? 0} bytes";
                    }));
                }

                // Trigger repaint
                this.Invoke((Action)(() => this.Invalidate()));

                SSHDotNetDiagnostics.LogDebug($"Terminal: Processed {data?.Length ?? 0} characters of output");
            }
            catch (Exception ex)
            {
                SSHDotNetDiagnostics.LogException("Terminal: Error writing output", ex);
            }
        }

        #endregion

        #region Input Handling

        public void ReadInput(out byte[] inputData)
        {
            try
            {
                if (_inputBuffer.Length > 0)
                {
                    inputData = Encoding.UTF8.GetBytes(_inputBuffer.ToString());
                    _inputBuffer.Clear();
                    SSHDotNetDiagnostics.LogDebug($"Terminal: Read {inputData.Length} bytes of input");
                }
                else
                {
                    inputData = Array.Empty<byte>();
                }
            }
            catch (Exception ex)
            {
                SSHDotNetDiagnostics.LogException("Terminal: Error reading input", ex);
                inputData = Array.Empty<byte>();
            }
        }

        #endregion

        #region Scrollback Management

        private void AddToScrollback(string data)
        {
            if (string.IsNullOrEmpty(data))
                return;

            try
            {
                // Split by newlines and add to scrollback
                var lines = data.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
                foreach (var line in lines)
                {
                    _scrollbackBuffer.Add(line);

                    // Maintain maximum scrollback size
                    if (_scrollbackBuffer.Count > _scrollbackLines)
                    {
                        _scrollbackBuffer.RemoveAt(0);
                    }
                }

                SSHDotNetDiagnostics.LogDebug($"Terminal: Added {lines.Length} lines to scrollback (total: {_scrollbackBuffer.Count})");
            }
            catch (Exception ex)
            {
                SSHDotNetDiagnostics.LogException("Terminal: Error adding to scrollback", ex);
            }
        }

        public string GetScrollbackContent(int startLine, int endLine)
        {
            try
            {
                startLine = Math.Max(0, Math.Min(startLine, _scrollbackBuffer.Count - 1));
                endLine = Math.Max(0, Math.Min(endLine, _scrollbackBuffer.Count - 1));

                if (startLine > endLine)
                    return string.Empty;

                var lines = _scrollbackBuffer.Skip(startLine).Take(endLine - startLine + 1);
                return string.Join(Environment.NewLine, lines);
            }
            catch (Exception ex)
            {
                SSHDotNetDiagnostics.LogException("Terminal: Error getting scrollback content", ex);
                return string.Empty;
            }
        }

        #endregion

        #region Event Handlers - Keyboard Input (Task 3.2)

        private void SshTerminalControl_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                // Handle special keys
                if (e.KeyCode == Keys.C && e.Control)
                {
                    // Copy selected text to clipboard
                    CopySelectionToClipboard();
                    e.Handled = true;
                    return;
                }

                if (e.KeyCode == Keys.V && e.Control)
                {
                    // Paste from clipboard
                    PasteFromClipboard();
                    e.Handled = true;
                    return;
                }

                // Handle function keys and special keys
                string keySequence = GetKeySequence(e);
                if (!string.IsNullOrEmpty(keySequence))
                {
                    _inputBuffer.Append(keySequence);
                    e.Handled = true;
                    SSHDotNetDiagnostics.LogDebug($"Terminal: Key sequence added to input buffer: {keySequence}");
                }
            }
            catch (Exception ex)
            {
                SSHDotNetDiagnostics.LogException("Terminal: Error in KeyDown handler", ex);
            }
        }

        private void SshTerminalControl_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                // Only process printable characters
                if (!char.IsControl(e.KeyChar))
                {
                    _inputBuffer.Append(e.KeyChar);
                    SSHDotNetDiagnostics.LogDebug($"Terminal: Character '{e.KeyChar}' added to input buffer");
                    e.Handled = true;
                }
                else if (e.KeyChar == (char)Keys.Return)
                {
                    _inputBuffer.Append('\n');
                    e.Handled = true;
                }
                else if (e.KeyChar == (char)Keys.Back)
                {
                    if (_inputBuffer.Length > 0)
                        _inputBuffer.Length--;
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                SSHDotNetDiagnostics.LogException("Terminal: Error in KeyPress handler", ex);
            }
        }

        private string GetKeySequence(KeyEventArgs e)
        {
            // Map special keys to terminal sequences
            return e.KeyCode switch
            {
                Keys.Up => "\x1b[A",       // Cursor up
                Keys.Down => "\x1b[B",     // Cursor down
                Keys.Right => "\x1b[C",    // Cursor right
                Keys.Left => "\x1b[D",     // Cursor left
                Keys.Home => "\x1b[H",     // Home
                Keys.End => "\x1b[F",      // End
                Keys.PageUp => "\x1b[5~",  // Page up
                Keys.PageDown => "\x1b[6~",// Page down
                Keys.Delete => "\x1b[3~",  // Delete
                Keys.F1 => "\x1bOP",       // F1
                Keys.F2 => "\x1bOQ",       // F2
                Keys.F3 => "\x1bOR",       // F3
                Keys.F4 => "\x1bOS",       // F4
                Keys.F5 => "\x1b[15~",     // F5
                Keys.Tab => "\t",
                _ => null
            };
        }

        #endregion

        #region Event Handlers - Mouse (Task 3.3 & 3.4)

        private void SshTerminalControl_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Left)
                {
                    _isSelecting = true;
                    _selectionStart = e.Location;
                    _selectionEnd = e.Location;
                    SSHDotNetDiagnostics.LogDebug($"Terminal: Selection started at {e.Location}");
                }
                else if (e.Button == MouseButtons.Right)
                {
                    // PuTTY-style right-click paste
                    PasteFromClipboard();
                }
            }
            catch (Exception ex)
            {
                SSHDotNetDiagnostics.LogException("Terminal: Error in MouseDown handler", ex);
            }
        }

        private void SshTerminalControl_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (_isSelecting && e.Button == MouseButtons.Left)
                {
                    _selectionEnd = e.Location;
                    this.Invalidate();
                    SSHDotNetDiagnostics.LogDebug($"Terminal: Selection extended to {e.Location}");
                }
            }
            catch (Exception ex)
            {
                SSHDotNetDiagnostics.LogException("Terminal: Error in MouseMove handler", ex);
            }
        }

        private void SshTerminalControl_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Left && _isSelecting)
                {
                    _isSelecting = false;
                    // PuTTY-style: selection = automatic copy
                    CopySelectionToClipboard();
                    SSHDotNetDiagnostics.LogDebug("Terminal: Selection completed and copied to clipboard");
                }
            }
            catch (Exception ex)
            {
                SSHDotNetDiagnostics.LogException("Terminal: Error in MouseUp handler", ex);
            }
        }

        #endregion

        #region Event Handlers - Window Resize (Task 3.3)

        private void SshTerminalControl_Resize(object sender, EventArgs e)
        {
            try
            {
                RecalculateDimensions();
                this.Invalidate();
                SSHDotNetDiagnostics.LogDebug($"Terminal: Resized to {this.Size}");
            }
            catch (Exception ex)
            {
                SSHDotNetDiagnostics.LogException("Terminal: Error in Resize handler", ex);
            }
        }

        #endregion

        #region Copy/Paste Operations (Task 3.4)

        private void CopySelectionToClipboard()
        {
            try
            {
                // Get selected text from scrollback or current display
                // For now, get the range based on selection coordinates
                int startLine = Math.Min(_selectionStart.Y, _selectionEnd.Y) / CHAR_HEIGHT;
                int endLine = Math.Max(_selectionStart.Y, _selectionEnd.Y) / CHAR_HEIGHT;

                string selectedText = GetScrollbackContent(startLine, endLine);
                if (!string.IsNullOrEmpty(selectedText))
                {
                    Clipboard.SetText(selectedText);
                    SSHDotNetDiagnostics.LogInfo($"Terminal: Copied {selectedText.Length} characters to clipboard");
                }
            }
            catch (Exception ex)
            {
                SSHDotNetDiagnostics.LogException("Terminal: Error copying to clipboard", ex);
            }
        }

        private void PasteFromClipboard()
        {
            try
            {
                if (Clipboard.ContainsText())
                {
                    string pastedText = Clipboard.GetText();
                    _inputBuffer.Append(pastedText);
                    SSHDotNetDiagnostics.LogInfo($"Terminal: Pasted {pastedText.Length} characters from clipboard");
                }
            }
            catch (Exception ex)
            {
                SSHDotNetDiagnostics.LogException("Terminal: Error pasting from clipboard", ex);
            }
        }

        #endregion

        #region Context Menu (Task 3.7)

        private void InitializeContextMenu()
        {
            _contextMenu = new ContextMenuStrip();

            var copyItem = new ToolStripMenuItem("&Copy", null, (s, e) => CopySelectionToClipboard());
            var pasteItem = new ToolStripMenuItem("&Paste", null, (s, e) => PasteFromClipboard());
            var selectAllItem = new ToolStripMenuItem("Select &All", null, (s, e) => SelectAll());

            _contextMenu.Items.Add(copyItem);
            _contextMenu.Items.Add(pasteItem);
            _contextMenu.Items.Add(new ToolStripSeparator());
            _contextMenu.Items.Add(selectAllItem);

            this.ContextMenuStrip = _contextMenu;
            SSHDotNetDiagnostics.LogDebug("Terminal: Context menu initialized");
        }

        private void SelectAll()
        {
            try
            {
                _selectionStart = Point.Empty;
                _selectionEnd = new Point(this.Width, this.Height);
                this.Invalidate();
                SSHDotNetDiagnostics.LogDebug("Terminal: Select All executed");
            }
            catch (Exception ex)
            {
                SSHDotNetDiagnostics.LogException("Terminal: Error in SelectAll", ex);
            }
        }

        #endregion

        #region Color Support (Task 3.5)

        public void SetColorScheme(Color backgroundColor, Color foregroundColor)
        {
            try
            {
                TerminalBackColor = backgroundColor;
                TerminalForeColor = foregroundColor;
                SSHDotNetDiagnostics.LogInfo($"Terminal: Color scheme changed to BG:{backgroundColor.Name}, FG:{foregroundColor.Name}");
            }
            catch (Exception ex)
            {
                SSHDotNetDiagnostics.LogException("Terminal: Error setting color scheme", ex);
            }
        }

        #endregion

        #region Terminal Rendering

        protected override void OnPaint(PaintEventArgs e)
        {
            try
            {
                // Clear background
                e.Graphics.Clear(_backgroundColor);

                // Get terminal screen text from VtNetCore
                if (_vtController != null)
                {
                    string screenText = _vtController.GetScreenText();

                    if (!string.IsNullOrEmpty(screenText))
                    {
                        // Log first time we get screen text for debugging
                        if (!_hasLoggedFirstRender)
                        {
                            SSHDotNetDiagnostics.LogInfo($"Terminal: First render - screen text length: {screenText.Length} chars");
                            _hasLoggedFirstRender = true;
                        }

                        // Render terminal text
                        using (Brush foregroundBrush = new SolidBrush(_foregroundColor))
                        {
                            StringFormat format = new StringFormat(StringFormatFlags.NoWrap);
                            int y = 0;
                            Font font = TerminalFont;  // Use property with lazy initialization

                            foreach (var line in screenText.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None))
                            {
                                e.Graphics.DrawString(line ?? "", font, foregroundBrush, 0, y, format);
                                y += font.Height;

                                if (y > this.Height)
                                    break;
                            }
                        }
                    }
                    else
                    {
                        // Log if screen text is empty
                        if (!_hasLoggedEmptyScreen)
                        {
                            SSHDotNetDiagnostics.LogDebug("Terminal: OnPaint called but GetScreenText() returned empty");
                            _hasLoggedEmptyScreen = true;
                        }
                    }
                }
                else
                {
                    // Show placeholder before initialization
                    using (Brush brush = new SolidBrush(_foregroundColor))
                    {
                        Font font = TerminalFont;  // Use property with lazy initialization
                        e.Graphics.DrawString("Terminal not initialized", font, brush, 0, 0);
                    }
                }

                // Draw diagnostic overlay if enabled
                if (DiagnosticMode && _diagnosticOverlay != null)
                {
                    _diagnosticOverlay.Invalidate();
                }
            }
            catch (Exception ex)
            {
                SSHDotNetDiagnostics.LogException("Terminal: Error in OnPaint", ex);
            }

            base.OnPaint(e);
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
