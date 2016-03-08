using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;


namespace mRemoteNG.Controls
{
	public class ListView : System.Windows.Forms.ListView
	{
        #region Public Properties
		[Category("Appearance"), DefaultValue(typeof(Color), "HighlightText")]
        public Color HighlightForeColor {get; set;}
			
		[Category("Appearance"), DefaultValue(typeof(Color), "Highlight")]
        public Color HighlightBackColor {get; set;}
			
		[Category("Appearance"), DefaultValue(typeof(Color), "HotTrack")]
        public Color HighlightBorderColor {get; set;}
			
		[Category("Appearance"), DefaultValue(typeof(Color), "ControlText")]
        public Color InactiveHighlightForeColor {get; set;}
			
		[Category("Appearance"), DefaultValue(typeof(Color), "Control")]
        public Color InactiveHighlightBackColor {get; set;}
			
		[Category("Appearance"), DefaultValue(typeof(Color), "ControlDark")]
        public Color InactiveHighlightBorderColor {get; set;}
			
		[Category("Appearance"), DefaultValue(true)]
        private bool _ShowFocusCues = true;
        public new bool ShowFocusCues
		{
			get
			{
				return _ShowFocusCues;
			}
			set
			{
				_ShowFocusCues = value;
			}
		}
			
		[Category("Appearance")]
        private Alignment _LabelAlignment = new Alignment(VerticalAlignment.Top, HorizontalAlignment.Left);
        public Alignment LabelAlignment
		{
			get
			{
				return _LabelAlignment;
			}
			set
			{
				_LabelAlignment = value;
			}
		}
        #endregion
			
        #region Constructors
		public ListView()
		{
			// VBConversions Note: Non-static class variable initialization is below.  Class variables cannot be initially assigned non-static values in C#.
			HighlightForeColor = SystemColors.HighlightText;
			HighlightBackColor = SystemColors.Highlight;
			HighlightBorderColor = SystemColors.HotTrack;
			InactiveHighlightForeColor = SystemColors.ControlText;
			InactiveHighlightBackColor = SystemColors.Control;
			InactiveHighlightBorderColor = SystemColors.ControlDark;
				
			OwnerDraw = true;
		}
        #endregion
			
        #region Protected Methods
		protected override void OnDrawItem(DrawListViewItemEventArgs e)
		{
			if (!(View == View.Tile))
			{
				base.OnDrawItem(e);
			}
			if (e.ItemIndex < 0)
			{
				base.OnDrawItem(e);
			}
				
			Brush foreColorBrush = null;
			Brush backColorBrush = null;
			Pen borderPen = null;
			try
			{
				if (Focused)
				{
					borderPen = new Pen(HighlightBorderColor);
				}
				else
				{
					borderPen = new Pen(InactiveHighlightBorderColor);
				}
					
				if (e.Item.Selected)
				{
					if (Focused)
					{
						foreColorBrush = new SolidBrush(HighlightForeColor);
						backColorBrush = new SolidBrush(HighlightBackColor);
					}
					else
					{
						foreColorBrush = new SolidBrush(InactiveHighlightForeColor);
						backColorBrush = new SolidBrush(InactiveHighlightBackColor);
					}
				}
				else
				{
					foreColorBrush = new SolidBrush(e.Item.ForeColor);
					backColorBrush = new SolidBrush(BackColor);
				}
					
				e.Graphics.FillRectangle(backColorBrush, e.Bounds);
					
				if (Focused && ShowFocusCues)
				{
					e.DrawFocusRectangle();
				}
				else if (e.Item.Selected)
				{
					e.Graphics.DrawRectangle(borderPen, e.Bounds.X, e.Bounds.Y, e.Bounds.Width - 1, e.Bounds.Height - 1);
				}
					
				Rectangle imageBounds = new Rectangle(e.Bounds.X + 2, e.Bounds.Y + 6, 16, 16);
				Rectangle textBounds = e.Bounds;
					
				if (e.Item.ImageList != null)
				{
					Image image = null;
					if (!string.IsNullOrEmpty(e.Item.ImageKey) && e.Item.ImageList.Images.ContainsKey(e.Item.ImageKey))
					{
						image = e.Item.ImageList.Images[e.Item.ImageKey];
					}
					else if (!(e.Item.ImageIndex < 0) & e.Item.ImageList.Images.Count > e.Item.ImageIndex)
					{
						image = e.Item.ImageList.Images[e.Item.ImageIndex];
					}
					if (image != null)
					{
						e.Graphics.DrawImageUnscaledAndClipped(image, imageBounds);
						textBounds.X = textBounds.Left + 20;
						textBounds.Width = textBounds.Width - 20;
					}
				}
					
				e.Graphics.DrawString(e.Item.Text, e.Item.Font, foreColorBrush, textBounds, GetStringFormat());
			}
			finally
			{
				if (foreColorBrush != null)
				{
					foreColorBrush.Dispose();
				}
				if (backColorBrush != null)
				{
					backColorBrush.Dispose();
				}
				if (borderPen != null)
				{
					borderPen.Dispose();
				}
			}
		}
        #endregion
			
        #region Private Methods
		private StringFormat GetStringFormat()
		{
			StringFormat format = StringFormat.GenericDefault;
				
			switch (LabelAlignment.Vertical)
			{
				case VerticalAlignment.Top:
					format.LineAlignment = StringAlignment.Near;
					break;
				case VerticalAlignment.Middle:
					format.LineAlignment = StringAlignment.Center;
					break;
				case VerticalAlignment.Bottom:
					format.LineAlignment = StringAlignment.Far;
					break;
			}
				
			switch (LabelAlignment.Horizontal)
			{
				case HorizontalAlignment.Left:
					format.Alignment = StringAlignment.Near;
					break;
				case HorizontalAlignment.Center:
					format.Alignment = StringAlignment.Center;
					break;
				case HorizontalAlignment.Right:
					format.Alignment = StringAlignment.Far;
					break;
			}
            
			if (RightToLeft.ToString() != null)
			{
				format.FormatFlags = (System.Drawing.StringFormatFlags) (format.FormatFlags | StringFormatFlags.DirectionRightToLeft);
			}
				
			if (LabelWrap)
			{
				format.FormatFlags = (System.Drawing.StringFormatFlags) (format.FormatFlags 
					& ~StringFormatFlags.NoWrap);
			}
			else
			{
				format.FormatFlags = (System.Drawing.StringFormatFlags) (format.FormatFlags | StringFormatFlags.NoWrap);
			}
				
			return format;
		}
        #endregion
	}
		
	[TypeConverter(typeof(ExpandableObjectConverter))]
    public class Alignment
	{
		public Alignment()
		{
			
		}
			
		public Alignment(VerticalAlignment verticalAlignment, HorizontalAlignment horizontalAlignment)
		{
			Vertical = verticalAlignment;
			Horizontal = horizontalAlignment;
		}
			
		[DefaultValue(VerticalAlignment.Top)]
        private VerticalAlignment _Vertical = VerticalAlignment.Top;
        public VerticalAlignment Vertical
	    {
		    get
		    {
			    return _Vertical;
		    }
		    set
		    {
			    _Vertical = value;
		    }
	    }
			
		[DefaultValue(HorizontalAlignment.Left)]
        private HorizontalAlignment _Horizontal = HorizontalAlignment.Left;
        public HorizontalAlignment Horizontal
		{
			get
			{
				return _Horizontal;
			}
			set
			{
				_Horizontal = value;
			}
		}
			
		public override string ToString()
		{
			return string.Format("{0}, {1}", Vertical, Horizontal);
		}
	}
		
	public enum VerticalAlignment
	{
		Top,
		Middle,
		Bottom
	}
		
	public enum HorizontalAlignment
	{
		Left,
		Center,
		Right
	}
}
