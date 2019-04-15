namespace mRemoteNG.Tools.Clipboard
{
    /// <summary>
    /// An abstraction of an operating system clipboard where
    /// data can be placed on and taken off the clipboard.
    /// </summary>
    public interface IClipboard
    {
        string GetText();
        void SetText(string text);
    }
}
