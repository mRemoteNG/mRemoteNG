using System.Drawing;
using BrightIdeasSoftware;

namespace mRemoteNG.UI.Controls
{
    public class TintedRowDecoration : AbstractDecoration
    {
        private Color _tint;
        private SolidBrush _tintBrush;
        private OLVListItem _row;

        /// <summary>
        /// Gets or sets the color that will be 'tinted' over the selected column
        /// </summary>
        public Color Tint
        {
            get => _tint;
            set
            {
                if (_tint == value)
                    return;

                if (_tintBrush != null)
                {
                    _tintBrush.Dispose();
                    _tintBrush = null;
                }

                _tint = value;
                _tintBrush = new SolidBrush(_tint);
            }
        }

        /// <summary>
        /// Create a TintedRowDecoration
        /// </summary>
        public TintedRowDecoration(OLVListItem row)
        {
            _row = row;
            Tint = Color.FromArgb(15, Color.Blue);
        }

        public override void Draw(ObjectListView olv, Graphics g, Rectangle r)
        {
            g.FillRectangle(_tintBrush, _row.Bounds);
        }
    }
}
