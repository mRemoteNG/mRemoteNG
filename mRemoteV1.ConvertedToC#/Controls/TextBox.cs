using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using System.ComponentModel;

// Adapted from http://stackoverflow.com/a/3678888/2101395

namespace mRemoteNG.Controls
{
	public class TextBox : Windows.Forms.TextBox
	{

		#region "Public Properties"
		[Category("Behavior"), DefaultValue(false)]
		public bool SelectAllOnFocus { get; set; }
		#endregion

		#region "Protected Methods"
		protected override void OnEnter(EventArgs e)
		{
			base.OnEnter(e);

			if (MouseButtons == MouseButtons.None) {
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

			if (!_focusHandled) {
				if (SelectionLength == 0)
					SelectAll();
				_focusHandled = true;
			}
		}
		#endregion

		#region "Private Fields"
			#endregion
		private bool _focusHandled = false;
	}
}
