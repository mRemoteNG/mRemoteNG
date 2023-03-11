using System;
using System.Runtime.Versioning;
using System.Text;
using System.Windows.Forms;
using mRemoteNG.App;

namespace mRemoteNG.UI
{
    [SupportedOSPlatform("windows")]
    public static class TextBoxExtensions
    {
        public static bool SetCueBannerText(this TextBox textBox, string cueText, bool showCueWhenFocused = false)
        {
            if (!textBox.IsHandleCreated || cueText == null) return false;
            var result = NativeMethods.SendMessage(textBox.Handle, NativeMethods.EM_SETCUEBANNER,
                                                   (IntPtr)Convert.ToInt32(showCueWhenFocused), cueText);
            return result.ToInt64() == NativeMethods.TRUE;
        }

        public static string GetCueBannerText(this TextBox textBox)
        {
            var cueBannerText = new StringBuilder(256);
            var result = NativeMethods.SendMessage(textBox.Handle, NativeMethods.EM_GETCUEBANNER, cueBannerText,
                                                   new IntPtr(cueBannerText.Capacity));
            return result.ToInt64() != 0 ? cueBannerText.ToString() : null;
        }
    }
}