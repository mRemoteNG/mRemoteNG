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
			DefaultValue(false)]private bool _SelectAllOnFocus;
        public bool SelectAllOnFocus
		{
			get
			{
				return _SelectAllOnFocus;
			}
			set
			{
				_SelectAllOnFocus = value;
			}
		}
        #endregion
			
        #region Protected Methods
		protected override void OnEnter(EventArgs e)
		{
			base.OnEnter(e);
				
			if (MouseButtons == MouseButtons.None)
			{
				SelectAll();
				_focusHandled = true;
			}
		}
			
		protected override void OnLeave(EventArgs e)
		{
			base.OnLeave(e);
				
			_focusHandled = false;
		}
			
		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
				
			if (!_focusHandled)
			{
				if (SelectionLength == 0)
				{
					SelectAll();
				}
				_focusHandled = true;
			}
		}
        #endregion
			
        #region Private Fields
		private bool _focusHandled;
        #endregion
	}
}