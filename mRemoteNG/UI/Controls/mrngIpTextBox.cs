using System.Runtime.Versioning;
using System.Windows.Forms;

namespace mRemoteNG.UI.Controls
{
    /// <summary>
    /// Single-line text box for entering an IP address. Accepts a full IPv4 or IPv6 address typed
    /// (or pasted) in one field, replacing the old four-octet control that only supported IPv4.
    /// Validation is performed by the consumer (e.g. via <see cref="System.Net.IPAddress.TryParse(string, out System.Net.IPAddress)"/>).
    /// </summary>
    [SupportedOSPlatform("windows")]
    public class MrngIpTextBox : MrngTextBox
    {
        private readonly ToolTip _toolTip = new();

        /// <summary>
        /// Tooltip shown when hovering the field. Preserved for compatibility with the previous
        /// control's designer wiring.
        /// </summary>
        public string ToolTipText
        {
            get => _toolTip.GetToolTip(this);
            set => _toolTip.SetToolTip(this, value);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _toolTip.Dispose();

            base.Dispose(disposing);
        }
    }
}
