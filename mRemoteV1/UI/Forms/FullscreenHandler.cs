using System.Drawing;
using System.Windows.Forms;

namespace mRemoteNG.UI.Forms
{
    public class FullscreenHandler
    {
        private readonly Form _handledForm;
        private FormWindowState _savedWindowState;
        private FormBorderStyle _savedBorderStyle;
        private Rectangle _savedBounds;
        private bool _value;

        public bool Value
        {
            get
            {
                return _value;
            }
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

        public FullscreenHandler(Form handledForm)
        {
            _handledForm = handledForm;
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
        }
        private void ExitFullscreen()
        {
            _handledForm.FormBorderStyle = _savedBorderStyle;
            _handledForm.WindowState = _savedWindowState;
            _handledForm.Bounds = _savedBounds;
        }
    }
}