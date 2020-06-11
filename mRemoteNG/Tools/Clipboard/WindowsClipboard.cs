using WinClipboard = System.Windows.Forms.Clipboard;

namespace mRemoteNG.Tools.Clipboard
{
    /// <summary>
    /// Represents the Windows system clipboard
    /// </summary>
    public class WindowsClipboard : IClipboard
    {
        public string GetText()
        {
            return WinClipboard.GetText();
        }

        public void SetText(string text)
        {
            WinClipboard.SetText(text);
        }
    }
}