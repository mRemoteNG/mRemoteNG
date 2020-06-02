using System;
using System.ComponentModel;
using System.Windows.Forms;

// Adapted from http://stackoverflow.com/a/3678888/2101395

namespace mRemoteNG.UI.Forms
{
    public class TextBox : Controls.Base.NGTextBox
    {
        #region Public Properties

        [Category("Behavior"),
         DefaultValue(false)]
        private bool _SelectAllOnFocus;

        public bool SelectAllOnFocus
        {
            get => _SelectAllOnFocus;
            set => _SelectAllOnFocus = value;
        }

        #endregion

        #region Protected Methods

        protected override void OnEnter(EventArgs e)
        {
            base.OnEnter(e);

            if (MouseButtons != MouseButtons.None) return;
            SelectAll();
            _focusHandled = true;
        }

        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);

            _focusHandled = false;
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            if (_focusHandled) return;
            if (SelectionLength == 0)
            {
                SelectAll();
            }

            _focusHandled = true;
        }

        #endregion

        #region Private Fields

        private bool _focusHandled;

        #endregion

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // TextBox
            // 
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular,
                                                System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ResumeLayout(false);
        }
    }
}