using System;
using System.Windows.Forms;
using mRemoteNG.App;

namespace mRemoteNG.UI.Controls
{
    public static class TextBoxExtensions
    {
        public static void CueBanner(this TextBox textBox, bool showCueWhenFocused, string cueText)
        {
            if (textBox.IsHandleCreated && cueText != null)
                NativeMethods.SendMessage(textBox.Handle, NativeMethods.EM_SETCUEBANNER, (IntPtr)Convert.ToInt32(showCueWhenFocused), cueText);
        }
    }
}