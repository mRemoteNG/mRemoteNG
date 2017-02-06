using System.Drawing;
using System.Windows.Forms;


namespace mRemoteNG.UI
{
    public static class FormExtensions
    {
        public static void CenterOnTarget(this Form formToMove, Form formToCenterOn)
        {
            var targetFormCenterX = formToCenterOn.Location.X + formToCenterOn.Width/2;
            var targetFormCenterY = formToCenterOn.Location.Y + formToCenterOn.Height/2;

            var thisFormCenterX = formToMove.Location.X + formToMove.Width/2;
            var thisFormCenterY = formToMove.Location.Y + formToMove.Height/2;

            formToMove.Location = new Point(targetFormCenterX - thisFormCenterX, targetFormCenterY - thisFormCenterY);
        }
    }
}