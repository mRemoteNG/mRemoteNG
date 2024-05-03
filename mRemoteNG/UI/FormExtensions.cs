using System.Drawing;
using System.Windows.Forms;


namespace mRemoteNG.UI
{
    public static class FormExtensions
    {
        public static void CenterOnTarget(this Form formToMove, Form formToCenterOn)
        {
            int targetFormCenterX = formToCenterOn.Location.X + formToCenterOn.Width / 2;
            int targetFormCenterY = formToCenterOn.Location.Y + formToCenterOn.Height / 2;

            int thisFormCenterX = formToMove.Location.X + formToMove.Width / 2;
            int thisFormCenterY = formToMove.Location.Y + formToMove.Height / 2;

            formToMove.Location = new Point(targetFormCenterX - thisFormCenterX, targetFormCenterY - thisFormCenterY);
        }
    }
}