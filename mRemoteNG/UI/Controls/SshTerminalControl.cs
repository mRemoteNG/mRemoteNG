// Design Note: Generic catch clauses (catch Exception) are used intentionally throughout this file.
// This is a UI control that renders terminal output and handles user input — an unhandled exception
// in painting or input processing would crash the application. All exceptions are logged via
// SSHDotNetDiagnostics. The foreach loops with null/empty checks use inline filtering (continue)
// rather than .Where() because they accumulate mutable state (e.g., x-position) across iterations,
// making LINQ-style filtering less readable.
using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.Versioning;
using System.Collections.Generic;
using System.Linq;
using mRemoteNG.Connection.Protocol.SSH_DotNet;
using VtNetCore.VirtualTerminal;
using VtNetCore.XTermParser;

namespace mRemoteNG.UI.Controls
{
    /// <summary>
    /// Event args for terminal resize events
    /// </summary>
    public class TerminalResizeEventArgs : EventArgs
    {
        public int Columns { get; }
        public int Rows { get; }
        public int WidthPixels { get; }
        public int HeightPixels { get; }

        public TerminalResizeEventArgs(int columns, int rows, int widthPixels, int heightPixels)
        {
            Columns = columns;
            Rows = rows;
            WidthPixels = widthPixels;
            HeightPixels = heightPixels;
        }
    }

    [SupportedOSPlatform("windows")]
    public partial class SshTerminalControl : UserControl
    {
        #region Events

        /// <summary>
        /// Raised when the terminal dimensions change, so the SSH pty can be resized
        /// </summary>
        public event EventHandler<TerminalResizeEventArgs> TerminalResized;

        #endregion

        #region Private Fields

        private const int DEFAULT_COLUMNS = 80;
        private const int DEFAULT_ROWS = 24;
        private const int DEFAULT_SCROLLBACK = 1000;
        private const int DEFAULT_CHAR_WIDTH = 8;
        private const int DEFAULT_CHAR_HEIGHT = 16;

        private int _columns = DEFAULT_COLUMNS;
        private int _rows = DEFAULT_ROWS;
        private int _scrollbackLines = DEFAULT_SCROLLBACK;
        private int _charWidth = DEFAULT_CHAR_WIDTH;
        private int _charHeight = DEFAULT_CHAR_HEIGHT;

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
        private readonly System.Threading.SemaphoreSlim _inputAvailableSemaphore = new System.Threading.SemaphoreSlim(0);

        // Selection/Copy-Paste state
        private Point _selectionStart = Point.Empty;  // Pixel coordinates
        private Point _selectionEnd = Point.Empty;    // Pixel coordinates
        private Point _selectionStartChar = new Point(-1, -1);  // Character coordinates (column, row), (-1,-1) = no selection
        private Point _selectionEndChar = new Point(-1, -1);    // Character coordinates (column, row), (-1,-1) = no selection
        private bool _isSelecting = false;
        private ContextMenuStrip _contextMenu;

        // Cursor rendering
        private System.Windows.Forms.Timer _cursorBlinkTimer;
        private bool _cursorVisible = true;
        private readonly Color _cursorColor = Color.Blue;

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

        public int CharWidth
        {
            get => _charWidth;
        }

        public int CharHeight
        {
            get => _charHeight;
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

        public int CharacterWidth
        {
            get => _charWidth;
        }

        public int CharacterHeight
        {
            get => _charHeight;
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
            this.TabStop = true;  // Allow the control to receive focus via Tab key

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

                // Configure scrollback buffer size (default is 2001)
                _vtController.MaximumHistoryLines = DEFAULT_SCROLLBACK;
                SSHDotNetDiagnostics.LogDebug($"Terminal: VtNetCore scrollback buffer set to {DEFAULT_SCROLLBACK} lines");

                _dataConsumer = new DataConsumer(_vtController);

                SSHDotNetDiagnostics.LogDebug($"Terminal: Created VtNetCore controller and DataConsumer ({_columns}x{_rows})");

                // Initialize scrollback buffer
                _scrollbackBuffer = new List<string>();
                _inputBuffer = new StringBuilder();

                // Event handlers
                this.KeyDown += SshTerminalControl_KeyDown;
                this.KeyPress += SshTerminalControl_KeyPress;
                this.MouseDown += SshTerminalControl_MouseDown;
                this.MouseMove += SshTerminalControl_MouseMove;
                this.MouseUp += SshTerminalControl_MouseUp;
                this.MouseWheel += SshTerminalControl_MouseWheel;
                this.Resize += SshTerminalControl_Resize;
                this.VisibleChanged += SshTerminalControl_VisibleChanged;

                // Terminal font
                _terminalFont = new Font("Courier New", 10f, FontStyle.Regular);

                // Initialize diagnostic overlay if needed
                if (DiagnosticMode)
                {
                    InitializeDiagnosticOverlay();
                }

                // Initialize context menu
                InitializeContextMenu();

                // Initialize cursor blink timer
                _cursorBlinkTimer = new System.Windows.Forms.Timer();
                _cursorBlinkTimer.Interval = 500; // Blink every 500ms
                _cursorBlinkTimer.Tick += CursorBlinkTimer_Tick;
                _cursorBlinkTimer.Start();
                SSHDotNetDiagnostics.LogDebug("Terminal: Cursor blink timer initialized");

                RecalculateDimensions();

                SSHDotNetDiagnostics.LogDebug("Terminal: Initialization complete");
                _isInitialized = true;

                // Automatically focus the terminal so user can start typing immediately
                this.Focus();
                SSHDotNetDiagnostics.LogDebug("Terminal: Auto-focus set on initialization");
            }
            catch (Exception ex)
            {
                SSHDotNetDiagnostics.LogException("Terminal initialization failed", ex);
                throw;
            }
        }

        private void RecalculateDimensions()
        {
            // Skip if control has no size yet (not yet laid out)
            if (this.Width <= 0 || this.Height <= 0)
            {
                SSHDotNetDiagnostics.LogDebug($"Terminal: Skipping dimension calculation - control not yet sized (W={this.Width}, H={this.Height})");
                return;
            }

            // Calculate character dimensions based on font using TextRenderer for consistency
            // Use TextRenderer.MeasureText to match our rendering method
            using (Graphics g = this.CreateGraphics())
            {
                TextFormatFlags flags = TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix;
                Size charSize = TextRenderer.MeasureText(g, "W", TerminalFont, Size.Empty, flags);
                _charWidth = charSize.Width;
                _charHeight = charSize.Height;

                SSHDotNetDiagnostics.LogDebug($"Terminal: Character size calculated as {_charWidth}x{_charHeight}px (using TextRenderer)");

                int newCols = Math.Max(1, this.Width / _charWidth);
                int newRows = Math.Max(1, this.Height / _charHeight);

                if (newCols != _columns || newRows != _rows)
                {
                    _columns = newCols;
                    _rows = newRows;

                    // Update VtNetCore viewport to match new dimensions!
                    if (_vtController != null)
                    {
                        _vtController.ResizeView(_columns, _rows);
                        SSHDotNetDiagnostics.LogDebug($"Terminal: VtNetCore viewport resized to {_columns}x{_rows}");
                    }

                    SSHDotNetDiagnostics.LogDebug($"Terminal: Dimensions recalculated to {_columns}x{_rows} (from control size {this.Width}x{this.Height})");

                    // Calculate pixel dimensions for SSH pty
                    int widthPixels = _columns * _charWidth;
                    int heightPixels = _rows * _charHeight;
                    SSHDotNetDiagnostics.LogDebug($"Terminal: Pixel dimensions {widthPixels}x{heightPixels} (char {_charWidth}x{_charHeight})");

                    // Notify protocol layer that terminal dimensions changed so SSH pty can be resized
                    TerminalResized?.Invoke(this, new TerminalResizeEventArgs(_columns, _rows, widthPixels, heightPixels));
                }
            }
        }

        /// <summary>
        /// Forces recalculation of terminal dimensions and notification to SSH server.
        /// Call this after the control is fully sized and visible.
        /// </summary>
        public void ForceResizeNotification()
        {
            try
            {
                SSHDotNetDiagnostics.LogDebug($"Terminal: Force resize notification requested (current size: {this.Width}x{this.Height})");

                // Recalculate dimensions
                RecalculateDimensions();

                // Always fire resize event even if dimensions didn't change
                // This ensures SSH pty gets the correct size
                if (_columns > 0 && _rows > 0)
                {
                    // Calculate pixel dimensions
                    int widthPixels = _columns * _charWidth;
                    int heightPixels = _rows * _charHeight;

                    SSHDotNetDiagnostics.LogDebug($"Terminal: Forcing TerminalResized event with {_columns}x{_rows} ({widthPixels}x{heightPixels} px)");
                    TerminalResized?.Invoke(this, new TerminalResizeEventArgs(_columns, _rows, widthPixels, heightPixels));
                }
            }
            catch (Exception ex)
            {
                SSHDotNetDiagnostics.LogException("Terminal: Error in ForceResizeNotification", ex);
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
                    SSHDotNetDiagnostics.LogTrace($"Terminal: Pushed {data.Length} bytes to VtNetCore DataConsumer");

                    // Reset scroll position to bottom when new output arrives
                    // This ensures we automatically show new content
                    if (_scrollbackPosition > 0)
                    {
                        SSHDotNetDiagnostics.LogTrace($"Terminal: Resetting scroll position from {_scrollbackPosition} to 0 (new output)");
                        _scrollbackPosition = 0;
                    }
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

                // Trigger immediate repaint (Update forces synchronous paint, no message queue delay)
                this.Invoke((Action)(() =>
                {
                    this.Invalidate();
                    this.Update();  // Forces immediate paint - PuTTY-style responsiveness!
                }));

                SSHDotNetDiagnostics.LogTrace($"Terminal: Processed {data?.Length ?? 0} characters of output");
            }
            catch (Exception ex)
            {
                SSHDotNetDiagnostics.LogException("Terminal: Error writing output", ex);
            }
        }

        #endregion

        #region Connection Status Messages

        public void DisplayConnectionClosed()
        {
            try
            {
                string message = "\r\n\r\n[Connection closed]\r\n";

                // Display in terminal via VtNetCore
                if (_dataConsumer != null)
                {
                    _dataConsumer.Push(Encoding.UTF8.GetBytes(message));
                }

                // Add to scrollback
                AddToScrollback(message);

                // Stop cursor blinking when disconnected
                if (_cursorBlinkTimer != null)
                {
                    _cursorBlinkTimer.Stop();
                    _cursorVisible = false;
                }

                // Trigger immediate repaint (Update forces synchronous paint, no message queue delay)
                this.Invoke((Action)(() =>
                {
                    this.Invalidate();
                    this.Update();  // Forces immediate paint
                }));

                SSHDotNetDiagnostics.LogInfo("Terminal: Displayed connection closed message");
            }
            catch (Exception ex)
            {
                SSHDotNetDiagnostics.LogException("Terminal: Error displaying connection closed message", ex);
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
                    SSHDotNetDiagnostics.LogTrace($"Terminal: Read {inputData.Length} bytes of input");
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

        public async Task<byte[]> WaitForInputAsync(CancellationToken cancellationToken)
        {
            try
            {
                // Wait for input to be available (event-driven, no polling!)
                await _inputAvailableSemaphore.WaitAsync(cancellationToken);

                if (_inputBuffer.Length > 0)
                {
                    byte[] inputData = Encoding.UTF8.GetBytes(_inputBuffer.ToString());
                    _inputBuffer.Clear();
                    SSHDotNetDiagnostics.LogTrace($"Terminal: Read {inputData.Length} bytes of input (event-driven)");
                    return inputData;
                }

                return Array.Empty<byte>();
            }
            catch (OperationCanceledException)
            {
                return Array.Empty<byte>();
            }
            catch (Exception ex)
            {
                SSHDotNetDiagnostics.LogException("Terminal: Error waiting for input", ex);
                return Array.Empty<byte>();
            }
        }

        private void SignalInputAvailable()
        {
            try
            {
                // Signal that input is available (release semaphore)
                if (_inputAvailableSemaphore.CurrentCount == 0)
                {
                    _inputAvailableSemaphore.Release();
                }
            }
            catch (Exception ex)
            {
                SSHDotNetDiagnostics.LogException("Terminal: Error signaling input available", ex);
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

                SSHDotNetDiagnostics.LogTrace($"Terminal: Added {lines.Length} lines to scrollback (total: {_scrollbackBuffer.Count})");
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

        #region Input Key Handling Override

        protected override bool IsInputKey(Keys keyData)
        {
            // Tell Windows Forms that we want to handle these keys ourselves
            // By default, Windows Forms uses Tab for focus navigation and arrows for control navigation
            // We need to override this to capture these keys for the terminal

            switch (keyData & Keys.KeyCode)
            {
                case Keys.Tab:          // Tab for bash completion
                case Keys.Up:           // Arrow keys for command history and navigation
                case Keys.Down:
                case Keys.Left:
                case Keys.Right:
                case Keys.Home:         // Home/End for line navigation
                case Keys.End:
                case Keys.PageUp:       // Page up/down for scrolling
                case Keys.PageDown:
                case Keys.Delete:       // Delete key
                case Keys.Insert:       // Insert key
                    return true;        // We want to handle these keys

                default:
                    return base.IsInputKey(keyData);
            }
        }

        protected override bool ProcessDialogKey(Keys keyData)
        {
            // Override dialog key processing to prevent Tab from changing focus
            // This is necessary for Tab completion in bash

            Keys keyCode = keyData & Keys.KeyCode;

            if (keyCode == Keys.Tab || keyCode == Keys.Up || keyCode == Keys.Down ||
                keyCode == Keys.Left || keyCode == Keys.Right)
            {
                // Let our KeyDown handler deal with these keys
                return false;
            }

            return base.ProcessDialogKey(keyData);
        }

        #endregion

        #region Event Handlers - Keyboard Input (Task 3.2)

        private void SshTerminalControl_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                // Handle Ctrl+C and Ctrl+V specially for copy/paste (optional - can be disabled for full terminal mode)
                if (e.KeyCode == Keys.C && e.Control && !e.Shift && !e.Alt
                    && _selectionStart != Point.Empty && _selectionEnd != Point.Empty)
                {
                    // If there's a selection, copy it; otherwise send Ctrl+C to terminal
                    CopySelectionToClipboard();
                    e.Handled = true;
                    return;
                }

                if (e.KeyCode == Keys.V && e.Control && !e.Shift && !e.Alt)
                {
                    // Paste from clipboard
                    PasteFromClipboard();
                    e.Handled = true;
                    return;
                }

                // Handle Ctrl+letter combinations (ASCII control characters)
                if (e.Control && !e.Alt)
                {
                    string ctrlSequence = GetCtrlSequence(e);
                    if (!string.IsNullOrEmpty(ctrlSequence))
                    {
                        _inputBuffer.Append(ctrlSequence);
                        SignalInputAvailable();
                        e.Handled = true;
                        e.SuppressKeyPress = true;  // Prevent beep
                        SSHDotNetDiagnostics.LogTrace($"Terminal: Ctrl sequence added: {GetPrintableSequence(ctrlSequence)}");
                        return;
                    }
                }

                // Handle Alt+key combinations (Meta key)
                if (e.Alt && !e.Control)
                {
                    string altSequence = GetAltSequence(e);
                    if (!string.IsNullOrEmpty(altSequence))
                    {
                        _inputBuffer.Append(altSequence);
                        SignalInputAvailable();
                        e.Handled = true;
                        SSHDotNetDiagnostics.LogTrace($"Terminal: Alt sequence added: {GetPrintableSequence(altSequence)}");
                        return;
                    }
                }

                // Handle function keys and navigation keys with modifiers
                string keySequence = GetKeySequence(e);
                if (!string.IsNullOrEmpty(keySequence))
                {
                    _inputBuffer.Append(keySequence);
                    SignalInputAvailable();  // Signal immediately - no polling delay!
                    e.Handled = true;
                    e.SuppressKeyPress = true;  // Prevent beep
                    SSHDotNetDiagnostics.LogTrace($"Terminal: Key sequence added: {GetPrintableSequence(keySequence)}");
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
                    SignalInputAvailable();  // Signal immediately - no polling delay!
                    SSHDotNetDiagnostics.LogTrace($"Terminal: Character '{e.KeyChar}' added to input buffer");
                    e.Handled = true;
                }
                else if (e.KeyChar == (char)Keys.Return)
                {
                    _inputBuffer.Append('\n');
                    SignalInputAvailable();  // Signal immediately - no polling delay!
                    e.Handled = true;
                }
                else if (e.KeyChar == (char)Keys.Back)
                {
                    // Send backspace character to SSH server (it will handle the deletion)
                    _inputBuffer.Append('\b');
                    SignalInputAvailable();  // Signal immediately - no polling delay!
                    SSHDotNetDiagnostics.LogTrace("Terminal: Backspace character added to input buffer");
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                SSHDotNetDiagnostics.LogException("Terminal: Error in KeyPress handler", ex);
            }
        }

        private string GetCtrlSequence(KeyEventArgs e)
        {
            // Map Ctrl+letter to ASCII control characters (Ctrl+A = 0x01, Ctrl+Z = 0x1A)
            // Ctrl+@ = 0x00 (NUL), Ctrl+[ = 0x1B (ESC), Ctrl+\ = 0x1C, Ctrl+] = 0x1D, Ctrl+^ = 0x1E, Ctrl+_ = 0x1F

            if (e.KeyCode >= Keys.A && e.KeyCode <= Keys.Z)
            {
                // Ctrl+A through Ctrl+Z = ASCII 1-26
                char ctrlChar = (char)(e.KeyCode - Keys.A + 1);
                return ctrlChar.ToString();
            }

            return e.KeyCode switch
            {
                Keys.D2 when e.Shift => "\x00",  // Ctrl+@ (NUL)
                Keys.Space => "\x00",             // Ctrl+Space (NUL)
                Keys.OemOpenBrackets => "\x1b",   // Ctrl+[ (ESC)
                Keys.Oem5 => "\x1c",              // Ctrl+\ (FS)
                Keys.OemCloseBrackets => "\x1d",  // Ctrl+] (GS)
                Keys.D6 when e.Shift => "\x1e",   // Ctrl+^ (RS)
                Keys.OemMinus => "\x1f",          // Ctrl+_ (US)
                Keys.OemQuestion when e.Shift => "\x7f",  // Ctrl+? (DEL)
                Keys.Back => "\x7f",              // Ctrl+Backspace (DEL)
                _ => null
            };
        }

        private string GetAltSequence(KeyEventArgs e)
        {
            // Alt+key sends ESC followed by the key
            // This is the standard "Meta" key behavior in terminals

            if (e.KeyCode >= Keys.A && e.KeyCode <= Keys.Z)
            {
                // Alt+letter = ESC + lowercase letter
                char letter = (char)('a' + (e.KeyCode - Keys.A));
                return "\x1b" + letter;
            }

            if (e.KeyCode >= Keys.D0 && e.KeyCode <= Keys.D9)
            {
                // Alt+number = ESC + number
                char digit = (char)('0' + (e.KeyCode - Keys.D0));
                return "\x1b" + digit;
            }

            // Alt+arrow keys, etc. are handled in GetKeySequence with modifiers
            return null;
        }

        private string GetKeySequence(KeyEventArgs e)
        {
            // Map special keys to terminal sequences with modifier support
            // Modifier encoding: 1=none, 2=Shift, 3=Alt, 4=Alt+Shift, 5=Ctrl, 6=Ctrl+Shift, 7=Ctrl+Alt, 8=Ctrl+Alt+Shift
            int modifier = 1;
            if (e.Shift && e.Alt && e.Control) modifier = 8;
            else if (e.Alt && e.Control) modifier = 7;
            else if (e.Shift && e.Control) modifier = 6;
            else if (e.Control) modifier = 5;
            else if (e.Shift && e.Alt) modifier = 4;
            else if (e.Alt) modifier = 3;
            else if (e.Shift) modifier = 2;

            // Arrow keys with modifiers
            if (modifier > 1)
            {
                string modArrow = e.KeyCode switch
                {
                    Keys.Up => $"\x1b[1;{modifier}A",
                    Keys.Down => $"\x1b[1;{modifier}B",
                    Keys.Right => $"\x1b[1;{modifier}C",
                    Keys.Left => $"\x1b[1;{modifier}D",
                    Keys.Home => $"\x1b[1;{modifier}H",
                    Keys.End => $"\x1b[1;{modifier}F",
                    _ => null
                };
                if (modArrow != null) return modArrow;
            }

            // Standard keys (no modifiers or unhandled modifiers)
            return e.KeyCode switch
            {
                // Arrow keys use application mode (ESC O X) for compatibility with full-screen apps like htop
                // This matches PuTTY's default behavior - see xterm DECCKM cursor key mode
                Keys.Up => "\x1bOA",       // Cursor up (application mode)
                Keys.Down => "\x1bOB",     // Cursor down (application mode)
                Keys.Right => "\x1bOC",    // Cursor right (application mode)
                Keys.Left => "\x1bOD",     // Cursor left (application mode)

                // Other navigation keys
                Keys.Home => "\x1b[H",     // Home
                Keys.End => "\x1b[F",      // End
                Keys.PageUp => "\x1b[5~",  // Page up
                Keys.PageDown => "\x1b[6~",// Page down
                Keys.Delete => "\x1b[3~",  // Delete
                Keys.Insert => "\x1b[2~",  // Insert

                // Function keys (F1-F4 use SS3, F5+ use CSI)
                Keys.F1 => "\x1bOP",       // F1
                Keys.F2 => "\x1bOQ",       // F2
                Keys.F3 => "\x1bOR",       // F3
                Keys.F4 => "\x1bOS",       // F4
                Keys.F5 => "\x1b[15~",     // F5
                Keys.F6 => "\x1b[17~",     // F6
                Keys.F7 => "\x1b[18~",     // F7
                Keys.F8 => "\x1b[19~",     // F8
                Keys.F9 => "\x1b[20~",     // F9
                Keys.F10 => "\x1b[21~",    // F10
                Keys.F11 => "\x1b[23~",    // F11
                Keys.F12 => "\x1b[24~",    // F12
                Keys.Tab => "\t",           // Tab
                _ => null
            };
        }

        private string GetPrintableSequence(string sequence)
        {
            // Convert control characters to printable form for logging
            if (string.IsNullOrEmpty(sequence)) return "";

            var sb = new StringBuilder();
            foreach (char c in sequence)
            {
                if (c == '\x1b') sb.Append("<ESC>");
                else if (c == '\t') sb.Append("<TAB>");
                else if (c == '\n') sb.Append("<LF>");
                else if (c == '\r') sb.Append("<CR>");
                else if (c < 32) sb.Append($"<^{(char)(c + 64)}>");
                else if (c == 127) sb.Append("<DEL>");
                else sb.Append(c);
            }
            return sb.ToString();
        }

        #endregion

        #region Helper Methods

        private Color ParseColor(string colorString, Color defaultColor)
        {
            // Parse color from VtNetCore hex format (#RRGGBB) to System.Drawing.Color
            if (string.IsNullOrEmpty(colorString))
                return defaultColor;

            try
            {
                // VtNetCore uses hex format: #RRGGBB
                if (colorString.StartsWith("#") && colorString.Length == 7)
                {
                    return ColorTranslator.FromHtml(colorString);
                }

                // Fallback: try parsing as named color
                return Color.FromName(colorString);
            }
            catch (Exception ex)
            {
                SSHDotNetDiagnostics.LogWarning($"Terminal: Failed to parse color '{colorString}', using default. Error: {ex.Message}");
                return defaultColor;
            }
        }

        private Point PixelToCharCoordinates(Point pixelPoint)
        {
            int row = Math.Max(0, pixelPoint.Y / _charHeight);

            // Find column by measuring actual text width character-by-character
            // This ensures accurate mapping from pixel position to character column
            int column = 0;
            try
            {
                if (_vtController != null)
                {
                    string screenText = _vtController.GetScreenText();
                    if (!string.IsNullOrEmpty(screenText))
                    {
                        string[] lines = screenText.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
                        if (row < lines.Length)
                        {
                            string line = lines[row];

                            // Calculate column using FIXED character grid (no need to measure text)
                            // This ensures click position matches the fixed-width rendering
                            column = pixelPoint.X / _charWidth;

                            // Clamp to line length
                            column = Math.Max(0, Math.Min(column, line.Length));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SSHDotNetDiagnostics.LogException("Terminal: Error in PixelToCharCoordinates", ex);
                // Fallback to simple division
                column = Math.Max(0, pixelPoint.X / _charWidth);
            }

            return new Point(column, row);
        }

        private Point CharToPixelCoordinates(Point charPoint)
        {
            int x = charPoint.X * _charWidth;
            int y = charPoint.Y * _charHeight;
            return new Point(x, y);
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
                    _selectionStartChar = PixelToCharCoordinates(e.Location);
                    _selectionEndChar = _selectionStartChar;
                    SSHDotNetDiagnostics.LogDebug($"Terminal: Selection started at pixel {e.Location}, char ({_selectionStartChar.X},{_selectionStartChar.Y})");
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
                    _selectionEndChar = PixelToCharCoordinates(e.Location);
                    this.Invalidate();
                    SSHDotNetDiagnostics.LogDebug($"Terminal: Selection extended to pixel {e.Location}, char ({_selectionEndChar.X},{_selectionEndChar.Y})");
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

                    // Update final selection end position (in case MouseMove didn't fire)
                    _selectionEnd = e.Location;
                    _selectionEndChar = PixelToCharCoordinates(e.Location);

                    // Log selection details for debugging
                    SSHDotNetDiagnostics.LogDebug($"Terminal: Selection completed - Start: pixel{_selectionStart}, char({_selectionStartChar.X},{_selectionStartChar.Y}) | End: pixel{_selectionEnd}, char({_selectionEndChar.X},{_selectionEndChar.Y})");

                    // PuTTY-style: selection = automatic copy
                    CopySelectionToClipboard();

                    // Clear selection visual after a short delay (PuTTY behavior)
                    Task.Delay(500).ContinueWith(_ =>
                    {
                        if (!this.IsDisposed)
                        {
                            this.Invoke((Action)(() =>
                            {
                                _selectionStart = Point.Empty;
                                _selectionEnd = Point.Empty;
                                _selectionStartChar = new Point(-1, -1);
                                _selectionEndChar = new Point(-1, -1);
                                this.Invalidate();
                            }));
                        }
                    });

                    SSHDotNetDiagnostics.LogDebug("Terminal: Selection completed and copied to clipboard");
                }
            }
            catch (Exception ex)
            {
                SSHDotNetDiagnostics.LogException("Terminal: Error in MouseUp handler", ex);
            }
        }

        private void SshTerminalControl_MouseWheel(object sender, MouseEventArgs e)
        {
            try
            {
                // Calculate scroll amount (typically 3 lines per wheel notch)
                int scrollLines = (e.Delta / 120) * 3;

                // Update scroll position
                // Positive delta (scroll up) = increase scrollback position (view older content)
                // Negative delta (scroll down) = decrease scrollback position (view newer content)
                int oldPosition = _scrollbackPosition;
                _scrollbackPosition += scrollLines;

                // Clamp to valid range
                // Maximum scrollback is DEFAULT_SCROLLBACK (1000 lines)
                // Minimum is 0 (current view, no scrollback)
                _scrollbackPosition = Math.Max(0, Math.Min(_scrollbackPosition, DEFAULT_SCROLLBACK));

                // Only invalidate if position actually changed
                if (_scrollbackPosition != oldPosition)
                {
                    SSHDotNetDiagnostics.LogTrace($"Terminal: Scrolled from {oldPosition} to {_scrollbackPosition} (delta={e.Delta}, lines={scrollLines})");
                    this.Invalidate();
                }
            }
            catch (Exception ex)
            {
                SSHDotNetDiagnostics.LogException("Terminal: Error in MouseWheel handler", ex);
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

        private void SshTerminalControl_VisibleChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.Visible)
                {
                    // When the control becomes visible, it has been properly sized in its parent
                    // Recalculate dimensions to ensure SSH pty size matches actual viewport
                    SSHDotNetDiagnostics.LogDebug($"Terminal: Control became visible with size {this.Size}");
                    RecalculateDimensions();
                    this.Invalidate();
                }
            }
            catch (Exception ex)
            {
                SSHDotNetDiagnostics.LogException("Terminal: Error in VisibleChanged handler", ex);
            }
        }

        #endregion

        #region Copy/Paste Operations (Task 3.4)

        private void CopySelectionToClipboard()
        {
            try
            {
                if (_vtController == null || _selectionStartChar.X < 0 || _selectionEndChar.X < 0)
                {
                    SSHDotNetDiagnostics.LogDebug($"Terminal: CopySelectionToClipboard skipped - no selection (_selectionStartChar={_selectionStartChar}, _selectionEndChar={_selectionEndChar})");
                    return;
                }

                // Get character coordinates (column, row)
                int startCol = Math.Min(_selectionStartChar.X, _selectionEndChar.X);
                int startRow = Math.Min(_selectionStartChar.Y, _selectionEndChar.Y);
                int endCol = Math.Max(_selectionStartChar.X, _selectionEndChar.X);
                int endRow = Math.Max(_selectionStartChar.Y, _selectionEndChar.Y);

                // Get clean screen text from VtNetCore (no ANSI codes)
                string screenText = _vtController.GetScreenText();
                if (string.IsNullOrEmpty(screenText))
                    return;

                // Split into lines
                string[] lines = screenText.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

                StringBuilder selectedText = new StringBuilder();

                for (int row = startRow; row <= endRow && row < lines.Length; row++)
                {
                    string line = lines[row];

                    if (row == startRow && row == endRow)
                    {
                        // Single line selection
                        int length = Math.Min(endCol - startCol + 1, line.Length - startCol);
                        if (startCol < line.Length && length > 0)
                        {
                            selectedText.Append(line.Substring(startCol, length));
                        }
                    }
                    else if (row == startRow)
                    {
                        // First line of multi-line selection
                        if (startCol < line.Length)
                        {
                            selectedText.AppendLine(line.Substring(startCol));
                        }
                    }
                    else if (row == endRow)
                    {
                        // Last line of multi-line selection
                        int length = Math.Min(endCol + 1, line.Length);
                        if (length > 0)
                        {
                            selectedText.Append(line.Substring(0, length));
                        }
                    }
                    else
                    {
                        // Middle lines - take entire line
                        selectedText.AppendLine(line);
                    }
                }

                string textToCopy = selectedText.ToString();
                if (!string.IsNullOrEmpty(textToCopy))
                {
                    // Ensure clipboard operation happens on UI thread
                    if (this.InvokeRequired)
                    {
                        this.Invoke((Action)(() => Clipboard.SetText(textToCopy)));
                    }
                    else
                    {
                        Clipboard.SetText(textToCopy);
                    }

                    SSHDotNetDiagnostics.LogInfo($"Terminal: Copied {textToCopy.Length} characters to clipboard (rows {startRow}-{endRow}, cols {startCol}-{endCol})");
                    SSHDotNetDiagnostics.LogTrace($"Terminal: Copied text: [{textToCopy}]");
                }
                else
                {
                    SSHDotNetDiagnostics.LogDebug($"Terminal: No text to copy - selection was empty (rows {startRow}-{endRow}, cols {startCol}-{endCol})");
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
                    SignalInputAvailable();  // Signal immediately - no polling delay!
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
                _selectionStartChar = new Point(0, 0);
                _selectionEndChar = new Point(_columns - 1, _rows - 1);
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

        #region Cursor Rendering

        private void CursorBlinkTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                _cursorVisible = !_cursorVisible;
                this.Invalidate(); // Trigger repaint to show/hide cursor
            }
            catch (Exception ex)
            {
                SSHDotNetDiagnostics.LogException("Terminal: Error in cursor blink timer", ex);
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
                    // Calculate starting line based on scroll position
                    // ViewPort.TopRow is the current viewport position in the history buffer
                    // When _scrollbackPosition = 0, show current view (ViewPort.TopRow)
                    // When _scrollbackPosition > 0, show earlier content (ViewPort.TopRow - scrollback)
                    int currentTopRow = _vtController.ViewPort.TopRow;
                    int startLine = Math.Max(0, currentTopRow - _scrollbackPosition);

                    SSHDotNetDiagnostics.LogTrace($"Terminal: Rendering from line {startLine} (ViewPort.TopRow={currentTopRow}, scrollback={_scrollbackPosition})");

                    // Use GetPageSpans for colored rendering with attributes
                    var pageSpans = _vtController.GetPageSpans(startLine, _rows, _columns, null);

                    if (pageSpans != null && pageSpans.Count > 0)
                    {
                        // Log first time we get screen text for debugging
                        if (!_hasLoggedFirstRender)
                        {
                            SSHDotNetDiagnostics.LogDebug($"Terminal: First render - {pageSpans.Count} rows with spans");
                            _hasLoggedFirstRender = true;
                        }

                        // Render terminal text with colors using spans
                        Font font = TerminalFont;

                        for (int rowIndex = 0; rowIndex < pageSpans.Count && rowIndex < _rows; rowIndex++)
                        {
                            var row = pageSpans[rowIndex];
                            if (row == null || row.Spans == null)
                                continue;

                            int y = rowIndex * _charHeight;
                            int x = 0;

                            foreach (var span in row.Spans)
                            {
                                if (span == null || string.IsNullOrEmpty(span.Text))
                                    continue;

                                // Parse colors from hex strings (VtNetCore format: #RRGGBB)
                                Color foreColor = ParseColor(span.ForgroundColor, _foregroundColor);
                                Color backColor = ParseColor(span.BackgroundColor, _backgroundColor);

                                // Apply bold attribute by using a bold font
                                Font renderFont = span.Bold ? new Font(font, FontStyle.Bold) : font;

                                try
                                {
                                    // Calculate span width using FIXED character grid (not variable measurement)
                                    // This is critical for terminal apps like htop that expect exact column alignment
                                    int spanWidth = span.Text.Length * _charWidth;

                                    // Draw background if different from default
                                    if (backColor != _backgroundColor)
                                    {
                                        using (Brush backBrush = new SolidBrush(backColor))
                                        {
                                            e.Graphics.FillRectangle(backBrush, x, y, spanWidth, _charHeight);
                                        }
                                    }

                                    // Draw text with foreground color using TextRenderer for true monospace rendering
                                    // TextRenderer ensures exact fixed-width character placement (no kerning/subpixel adjustments)
                                    Rectangle textRect = new Rectangle(x, y, spanWidth, _charHeight);
                                    TextFormatFlags textFlags = TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix | TextFormatFlags.Left | TextFormatFlags.Top;
                                    TextRenderer.DrawText(e.Graphics, span.Text, renderFont, textRect, foreColor, textFlags);

                                    // Draw underline if attribute is set
                                    if (span.Underline)
                                    {
                                        using (Pen underlinePen = new Pen(foreColor, 1))
                                        {
                                            int underlineY = y + _charHeight - 2;
                                            e.Graphics.DrawLine(underlinePen, x, underlineY, x + spanWidth, underlineY);
                                        }
                                    }

                                    // Move x position for next span using FIXED character grid
                                    x += spanWidth;
                                }
                                finally
                                {
                                    // Dispose bold font if created
                                    if (span.Bold && renderFont != font)
                                    {
                                        renderFont.Dispose();
                                    }
                                }
                            }
                        }

                        // Get screen text for selection and cursor positioning
                        string screenText = _vtController.GetScreenText();

                        // Draw selection highlight if active
                        if (!string.IsNullOrEmpty(screenText) && (_isSelecting || (_selectionStartChar.X >= 0 && _selectionEndChar.X >= 0)))
                        {
                            try
                            {
                                // Get character coordinates (column, row)
                                int startCol = Math.Min(_selectionStartChar.X, _selectionEndChar.X);
                                int startRow = Math.Min(_selectionStartChar.Y, _selectionEndChar.Y);
                                int endCol = Math.Max(_selectionStartChar.X, _selectionEndChar.X);
                                int endRow = Math.Max(_selectionStartChar.Y, _selectionEndChar.Y);

                                // CRITICAL: Measure actual text width, don't use column * _charWidth
                                // This prevents "exponential" growth of selection box
                                int startX = 0, endX = 0;

                                // Calculate start position using FIXED character grid
                                startX = startCol * _charWidth;

                                // Calculate end position using FIXED character grid
                                endX = (endCol + 1) * _charWidth;

                                int startY = startRow * _charHeight;

                                // Handle single-line vs multi-line selection
                                if (startRow == endRow)
                                {
                                    // Single line selection
                                    int width = endX - startX;
                                    int height = _charHeight;

                                    if (width > 0)
                                    {
                                        using (Brush highlightBrush = new SolidBrush(Color.FromArgb(100, 100, 150, 255)))
                                        {
                                            e.Graphics.FillRectangle(highlightBrush, startX, startY, width, height);
                                        }
                                    }
                                }
                                else
                                {
                                    // Multi-line selection - draw three rectangles
                                    using (Brush highlightBrush = new SolidBrush(Color.FromArgb(100, 100, 150, 255)))
                                    {
                                        // First line: from startX to end of line
                                        int firstLineWidth = this.Width - startX;
                                        if (firstLineWidth > 0)
                                        {
                                            e.Graphics.FillRectangle(highlightBrush, startX, startY, firstLineWidth, _charHeight);
                                        }

                                        // Middle lines: full width
                                        if (endRow - startRow > 1)
                                        {
                                            int middleY = startY + _charHeight;
                                            int middleHeight = (endRow - startRow - 1) * _charHeight;
                                            e.Graphics.FillRectangle(highlightBrush, 0, middleY, this.Width, middleHeight);
                                        }

                                        // Last line: from start to endX
                                        int lastLineY = endRow * _charHeight;
                                        if (endX > 0)
                                        {
                                            e.Graphics.FillRectangle(highlightBrush, 0, lastLineY, endX, _charHeight);
                                        }
                                    }
                                }
                            }
                            catch (Exception selEx)
                            {
                                SSHDotNetDiagnostics.LogException("Terminal: Error rendering selection highlight", selEx);
                            }
                        }

                        // Draw cursor if visible
                        // Respect VtNetCore's ShowCursor property - applications can hide cursor via DECTCEM escape sequences
                        if (!string.IsNullOrEmpty(screenText) && _cursorVisible && _vtController.CursorState != null && _vtController.CursorState.ShowCursor)
                        {
                            try
                            {
                                int cursorColumn = _vtController.CursorState.CurrentColumn;
                                int cursorRow = _vtController.CursorState.CurrentRow;

                                // Calculate cursor position using FIXED character grid
                                // Cursor is at column N, which is N * charWidth pixels from left edge
                                int cursorX = cursorColumn * _charWidth;

                                int cursorY = cursorRow * _charHeight;
                                int cursorHeight = _charHeight;

                                // Draw blue cursor box (one character wide)
                                using (Brush cursorBrush = new SolidBrush(_cursorColor))
                                {
                                    e.Graphics.FillRectangle(cursorBrush, cursorX, cursorY, _charWidth, cursorHeight);
                                }
                            }
                            catch (Exception cursorEx)
                            {
                                SSHDotNetDiagnostics.LogException("Terminal: Error rendering cursor", cursorEx);
                            }
                        }
                    }
                    else
                    {
                        // Log if no spans returned
                        if (!_hasLoggedEmptyScreen)
                        {
                            SSHDotNetDiagnostics.LogDebug("Terminal: OnPaint called but GetPageSpans() returned empty");
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

        #region Cleanup and Disposal

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                try
                {
                    // Stop and dispose cursor blink timer
                    if (_cursorBlinkTimer != null)
                    {
                        _cursorBlinkTimer.Stop();
                        _cursorBlinkTimer.Tick -= CursorBlinkTimer_Tick;
                        _cursorBlinkTimer.Dispose();
                        _cursorBlinkTimer = null;
                        SSHDotNetDiagnostics.LogDebug("Terminal: Cursor blink timer disposed");
                    }

                    // Dispose terminal font if created
                    if (_terminalFont != null)
                    {
                        _terminalFont.Dispose();
                        _terminalFont = null;
                    }

                    // Dispose context menu
                    if (_contextMenu != null)
                    {
                        _contextMenu.Dispose();
                        _contextMenu = null;
                    }

                    // Dispose diagnostic overlay
                    if (_diagnosticOverlay != null)
                    {
                        _diagnosticOverlay.Dispose();
                        _diagnosticOverlay = null;
                    }

                    // Dispose input semaphore
                    if (_inputAvailableSemaphore != null)
                    {
                        _inputAvailableSemaphore.Dispose();
                        SSHDotNetDiagnostics.LogDebug("Terminal: Input semaphore disposed");
                    }
                }
                catch (Exception ex)
                {
                    SSHDotNetDiagnostics.LogException("Terminal: Error during disposal", ex);
                }
            }

            base.Dispose(disposing);
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
