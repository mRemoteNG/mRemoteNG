using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
namespace mRemoteNG.Controls
{
	public class ToolStripSplitButton : Windows.Forms.ToolStripSplitButton
	{

		public ToolStripDropDown DropDown {
			get { return base.DropDown; }
			set {
				if (!object.ReferenceEquals(base.DropDown, value)) {
					base.DropDown = value;
					base.DropDown.Closing += DropDown_Closing;
				}
			}
		}

		private void DropDown_Closing(object sender, ToolStripDropDownClosingEventArgs e)
		{
			if (!(e.CloseReason == ToolStripDropDownCloseReason.AppClicked))
				return;

			Rectangle dropDownButtonBoundsClient = DropDownButtonBounds;
			// Relative to the ToolStripSplitButton
			dropDownButtonBoundsClient.Offset(Bounds.Location);
			// Relative to the parent of the ToolStripSplitButton
			Rectangle dropDownButtonBoundsScreen = GetCurrentParent().RectangleToScreen(dropDownButtonBoundsClient);
			// Relative to the screen

			if (dropDownButtonBoundsScreen.Contains(Control.MousePosition))
				e.Cancel = true;
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			_dropDownVisibleOnMouseDown = DropDown.Visible;
			base.OnMouseDown(e);
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			if (_dropDownVisibleOnMouseDown) {
				DropDown.Close();
			} else {
				base.OnMouseUp(e);
			}
		}

		private bool _dropDownVisibleOnMouseDown = false;
	}
}
