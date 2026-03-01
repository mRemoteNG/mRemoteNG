using System;
using System.Drawing;
using System.Windows.Forms;
using mRemoteNG.UI.Forms;

namespace mRemoteNG.UI
{
    public class FullscreenHandler(Form handledForm)
    {
        private readonly Form _handledForm = handledForm;
        private FullscreenToolbar? _toolbar;
        private readonly Timer _mouseCheckTimer = new() { Interval = 200 };
        private FormWindowState _savedWindowState;
        private FormBorderStyle _savedBorderStyle;
        private Rectangle _savedBounds;
        private bool _value;

        public bool Value
        {
            get => _value;
            set
            {
                if (_value == value) return;
                if (!_value)
                    EnterFullscreen();
                else
                    ExitFullscreen();
                _value = value;
            }
        }

        private void EnterFullscreen()
        {
            _savedBorderStyle = _handledForm.FormBorderStyle;
            _savedWindowState = _handledForm.WindowState;
            _savedBounds = _handledForm.Bounds;

            _handledForm.FormBorderStyle = FormBorderStyle.None;
            if (_handledForm.WindowState == FormWindowState.Maximized)
            {
                _handledForm.WindowState = FormWindowState.Normal;
            }

            _handledForm.WindowState = FormWindowState.Maximized;

            _mouseCheckTimer.Tick += CheckMousePosition;
            _mouseCheckTimer.Start();
        }

        private void ExitFullscreen()
        {
            _mouseCheckTimer.Stop();
            _mouseCheckTimer.Tick -= CheckMousePosition;

            if (_toolbar != null)
            {
                if (!_toolbar.IsDisposed)
                {
                    _toolbar.Close();
                    _toolbar.Dispose();
                }
                _toolbar = null;
            }

            _handledForm.FormBorderStyle = _savedBorderStyle;
            _handledForm.WindowState = _savedWindowState;

            // Clamp restored bounds to working area so the title bar is always reachable (#1804)
            var bounds = _savedBounds;
            var workingArea = Screen.GetWorkingArea(bounds);
            if (bounds.Top < workingArea.Top)
                bounds.Y = workingArea.Top;
            _handledForm.Bounds = bounds;
        }

        /// <summary>
        /// Moves the fullscreen window to the next monitor (wraps around).
        /// </summary>
        public void MoveToNextScreen()
        {
            if (!_value) return;
            var screens = Screen.AllScreens;
            if (screens.Length < 2) return;
            var current = Screen.FromControl(_handledForm);
            int idx = Array.IndexOf(screens, current);
            int next = (idx + 1) % screens.Length;
            MoveToScreen(screens[next]);
        }

        /// <summary>
        /// Moves the fullscreen window to the previous monitor (wraps around).
        /// </summary>
        public void MoveToPreviousScreen()
        {
            if (!_value) return;
            var screens = Screen.AllScreens;
            if (screens.Length < 2) return;
            var current = Screen.FromControl(_handledForm);
            int idx = Array.IndexOf(screens, current);
            int prev = (idx - 1 + screens.Length) % screens.Length;
            MoveToScreen(screens[prev]);
        }

        private void MoveToScreen(Screen target)
        {
            // Must drop Maximized state before repositioning, then re-maximize
            _handledForm.WindowState = FormWindowState.Normal;
            _handledForm.Bounds = target.Bounds;
            _handledForm.WindowState = FormWindowState.Maximized;

            // Reposition toolbar to new screen center
            if (_toolbar != null && !_toolbar.IsDisposed)
            {
                _toolbar.Location = new Point(
                    target.Bounds.Left + (target.Bounds.Width - _toolbar.Width) / 2,
                    target.Bounds.Top
                );
            }
        }

        private void CheckMousePosition(object? sender, EventArgs e)
        {
            if (!_value) return;

            try
            {
                var cursor = Cursor.Position;
                var screen = Screen.FromControl(_handledForm);
                
                // Show if mouse is near top edge (< 5px) or over the toolbar
                // Also check if cursor is within the screen bounds horizontally to avoid showing on adjacent screens
                bool isOnScreen = screen.Bounds.Contains(cursor);
                bool isNearTop = isOnScreen && (cursor.Y < screen.Bounds.Top + 5);
                bool isOverToolbar = _toolbar != null && !_toolbar.IsDisposed && _toolbar.Visible && _toolbar.Bounds.Contains(cursor);

                if (isNearTop || isOverToolbar)
                {
                    if (_toolbar == null || _toolbar.IsDisposed)
                    {
                         _toolbar = new FullscreenToolbar(_handledForm, this);
                         // Center horizontally at the top of the current screen
                         _toolbar.Location = new Point(
                             screen.Bounds.Left + (screen.Bounds.Width - _toolbar.Width) / 2, 
                             screen.Bounds.Top
                         );
                    }
                    
                    if (!_toolbar.Visible)
                    {
                        _toolbar.Show();
                        _toolbar.BringToFront();
                    }
                }
                else
                {
                     if (_toolbar != null && !_toolbar.IsDisposed && _toolbar.Visible)
                     {
                         // Hide if cursor moved away
                         // We give a small buffer (e.g. 50px) to allow moving mouse down without immediate hide, 
                         // but for "auto-hide" usually we hide as soon as it leaves the toolbar area.
                         // Let's use 50px buffer from top.
                         if (cursor.Y > screen.Bounds.Top + 50 || !isOnScreen)
                            _toolbar.Hide();
                     }
                }
            }
            catch (Exception)
            {
                _ = 0; // Ignore errors during mouse check (e.g. during screen changes)
            }
        }
    }
}
