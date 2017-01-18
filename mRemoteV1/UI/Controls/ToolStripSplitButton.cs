using System.Drawing;
using System.Windows.Forms;

namespace mRemoteNG.UI.Controls
{
	public class ToolStripSplitButton : System.Windows.Forms.ToolStripSplitButton
	{
        public new ToolStripDropDown DropDown
		{
			get { return base.DropDown; }
			set
			{
				if (base.DropDown != value)
				{
					base.DropDown = value;
					base.DropDown.Closing += DropDown_Closing;
				}
			}
		}
			
		private void DropDown_Closing(object sender, ToolStripDropDownClosingEventArgs e)
		{
			if (e.CloseReason != ToolStripDropDownCloseReason.AppClicked)
			{
				return;
			}
				
			Rectangle dropDownButtonBoundsClient = DropDownButtonBounds; // Relative to the ToolStripSplitButton
			dropDownButtonBoundsClient.Offset(Bounds.Location); // Relative to the parent of the ToolStripSplitButton
			Rectangle dropDownButtonBoundsScreen = GetCurrentParent().RectangleToScreen(dropDownButtonBoundsClient); // Relative to the screen
				
			if (dropDownButtonBoundsScreen.Contains(Control.MousePosition))
			{
				e.Cancel = true;
			}
		}
			
		protected override void OnMouseDown(MouseEventArgs e)
		{
			_dropDownVisibleOnMouseDown = DropDown.Visible;
			base.OnMouseDown(e);
		}
			
		protected override void OnMouseUp(MouseEventArgs e)
		{
			if (_dropDownVisibleOnMouseDown)
			{
				DropDown.Close();
			}
			else
			{
				base.OnMouseUp(e);
			}
		}
			
		private bool _dropDownVisibleOnMouseDown;
	}
}
