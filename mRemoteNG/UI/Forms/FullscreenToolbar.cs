using System;
using System.Drawing;
using System.Windows.Forms;
using mRemoteNG.UI;

namespace mRemoteNG.UI.Forms
{
    public class FullscreenToolbar : Form
    {
        private Button _btnPrevScreen = null!;
        private Button _btnNextScreen = null!;
        private Button _btnMinimize = null!;
        private Button _btnRestore = null!;
        private Button _btnClose = null!;
        private readonly Form _targetForm;
        private readonly FullscreenHandler _fullscreenHandler;

        // Drag-to-exit-fullscreen state
        private Point _dragStartScreen;
        private bool _dragActive;
        private const int DragDownThreshold = 30;

        public FullscreenToolbar(Form targetForm, FullscreenHandler fullscreenHandler)
        {
            _targetForm = targetForm;
            _fullscreenHandler = fullscreenHandler;
            InitializeComponent();
            HookDragToExit(this);
            HookDragToExit(_btnPrevScreen);
            HookDragToExit(_btnNextScreen);
            HookDragToExit(_btnMinimize);
            HookDragToExit(_btnRestore);
            HookDragToExit(_btnClose);
        }

        private void InitializeComponent()
        {
            _btnPrevScreen = new Button();
            _btnNextScreen = new Button();
            _btnMinimize = new Button();
            _btnRestore = new Button();
            _btnClose = new Button();

            SuspendLayout();

            bool hasMultipleScreens = Screen.AllScreens.Length > 1;
            int xOffset = 2;

            //
            // FullscreenToolbar properties
            //
            FormBorderStyle = FormBorderStyle.None;
            TopMost = true;
            ShowInTaskbar = false;
            BackColor = Color.FromArgb(45, 45, 48); // Dark VS-like background
            Opacity = 0.9;
            Padding = new Padding(2);
            StartPosition = FormStartPosition.Manual;

            //
            // btnPrevScreen (◄) — move fullscreen to previous monitor
            //
            _btnPrevScreen.FlatStyle = FlatStyle.Flat;
            _btnPrevScreen.FlatAppearance.BorderSize = 0;
            _btnPrevScreen.FlatAppearance.MouseOverBackColor = Color.FromArgb(62, 62, 64);
            _btnPrevScreen.ForeColor = Color.White;
            _btnPrevScreen.Location = new Point(xOffset, 2);
            _btnPrevScreen.Name = "btnPrevScreen";
            _btnPrevScreen.Size = new Size(28, 22);
            _btnPrevScreen.Text = "3"; // Marlett Left arrow
            _btnPrevScreen.Font = new Font("Marlett", 8.5F, FontStyle.Regular, GraphicsUnit.Point);
            _btnPrevScreen.UseVisualStyleBackColor = true;
            _btnPrevScreen.Visible = hasMultipleScreens;
            _btnPrevScreen.Click += (s, e) => _fullscreenHandler.MoveToPreviousScreen();
            if (hasMultipleScreens) xOffset += 30;

            //
            // btnNextScreen (►) — move fullscreen to next monitor
            //
            _btnNextScreen.FlatStyle = FlatStyle.Flat;
            _btnNextScreen.FlatAppearance.BorderSize = 0;
            _btnNextScreen.FlatAppearance.MouseOverBackColor = Color.FromArgb(62, 62, 64);
            _btnNextScreen.ForeColor = Color.White;
            _btnNextScreen.Location = new Point(xOffset, 2);
            _btnNextScreen.Name = "btnNextScreen";
            _btnNextScreen.Size = new Size(28, 22);
            _btnNextScreen.Text = "4"; // Marlett Right arrow
            _btnNextScreen.Font = new Font("Marlett", 8.5F, FontStyle.Regular, GraphicsUnit.Point);
            _btnNextScreen.UseVisualStyleBackColor = true;
            _btnNextScreen.Visible = hasMultipleScreens;
            _btnNextScreen.Click += (s, e) => _fullscreenHandler.MoveToNextScreen();
            if (hasMultipleScreens) xOffset += 30;

            //
            // btnMinimize
            //
            _btnMinimize.FlatStyle = FlatStyle.Flat;
            _btnMinimize.FlatAppearance.BorderSize = 0;
            _btnMinimize.FlatAppearance.MouseOverBackColor = Color.FromArgb(62, 62, 64);
            _btnMinimize.ForeColor = Color.White;
            _btnMinimize.Location = new Point(xOffset, 2);
            _btnMinimize.Name = "btnMinimize";
            _btnMinimize.Size = new Size(38, 22);
            _btnMinimize.Text = "0"; // Marlett Minimize
            _btnMinimize.Font = new Font("Marlett", 8.5F, FontStyle.Regular, GraphicsUnit.Point);
            _btnMinimize.UseVisualStyleBackColor = true;
            _btnMinimize.Click += (s, e) => _targetForm.WindowState = FormWindowState.Minimized;
            xOffset += 40;

            //
            // btnRestore
            //
            _btnRestore.FlatStyle = FlatStyle.Flat;
            _btnRestore.FlatAppearance.BorderSize = 0;
            _btnRestore.FlatAppearance.MouseOverBackColor = Color.FromArgb(62, 62, 64);
            _btnRestore.ForeColor = Color.White;
            _btnRestore.Location = new Point(xOffset, 2);
            _btnRestore.Name = "btnRestore";
            _btnRestore.Size = new Size(38, 22);
            _btnRestore.Text = "2"; // Marlett Restore
            _btnRestore.Font = new Font("Marlett", 8.5F, FontStyle.Regular, GraphicsUnit.Point);
            _btnRestore.UseVisualStyleBackColor = true;
            _btnRestore.Click += (s, e) => _fullscreenHandler.Value = false; // Exit fullscreen
            xOffset += 40;

            //
            // btnClose
            //
            _btnClose.FlatStyle = FlatStyle.Flat;
            _btnClose.FlatAppearance.BorderSize = 0;
            _btnClose.FlatAppearance.MouseOverBackColor = Color.FromArgb(232, 17, 35); // Red for close
            _btnClose.ForeColor = Color.White;
            _btnClose.Location = new Point(xOffset, 2);
            _btnClose.Name = "btnClose";
            _btnClose.Size = new Size(38, 22);
            _btnClose.Text = "r"; // Marlett Close
            _btnClose.Font = new Font("Marlett", 8.5F, FontStyle.Regular, GraphicsUnit.Point);
            _btnClose.UseVisualStyleBackColor = true;
            _btnClose.Click += (s, e) => _targetForm.Close();
            xOffset += 40;

            Size = new Size(xOffset, 26);

            Controls.Add(_btnPrevScreen);
            Controls.Add(_btnNextScreen);
            Controls.Add(_btnMinimize);
            Controls.Add(_btnRestore);
            Controls.Add(_btnClose);

            ResumeLayout(false);
        }

        protected override bool ShowWithoutActivation => true; // Prevent stealing focus

        // Attach drag detection to the toolbar form and all its buttons.
        // Dragging down more than DragDownThreshold pixels exits fullscreen mode,
        // mirroring the behavior of Microsoft's RDP client title bar (#2223).
        private void HookDragToExit(Control control)
        {
            control.MouseDown += OnDragMouseDown;
            control.MouseMove += OnDragMouseMove;
            control.MouseUp += OnDragMouseUp;
        }

        private void OnDragMouseDown(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && sender is Control c)
            {
                _dragStartScreen = c.PointToScreen(e.Location);
                _dragActive = true;
            }
        }

        private void OnDragMouseMove(object? sender, MouseEventArgs e)
        {
            if (!_dragActive || e.Button != MouseButtons.Left || sender is not Control c)
                return;

            var pos = c.PointToScreen(e.Location);
            if (pos.Y - _dragStartScreen.Y > DragDownThreshold)
            {
                _dragActive = false;
                // Defer to let the current mouse event complete before closing the toolbar
                BeginInvoke(new Action(() => _fullscreenHandler.Value = false));
            }
        }

        private void OnDragMouseUp(object? sender, MouseEventArgs e)
        {
            _dragActive = false;
        }
    }
}
