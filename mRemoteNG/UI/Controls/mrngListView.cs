using System.ComponentModel;
using System.Drawing;
using System.Runtime.Versioning;
using BrightIdeasSoftware;
using mRemoteNG.Themes;

namespace mRemoteNG.UI.Controls
{
    [SupportedOSPlatform("windows")]
    //Simple coloring of ObjectListView
    //This is subclassed to avoid repeating the code in multiple places
    internal class MrngListView : ObjectListView
    {
        private CellBorderDecoration? deco;

        //Control if the gridlines are styled, must be set before the OnCreateControl is fired
        public bool DecorateLines { get; set; } = true;

        public MrngListView()
        {
            InitializeComponent();
            ThemeManager.getInstance().ThemeChanged += OnCreateControl;
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            ThemeManager _themeManager = ThemeManager.getInstance();
            if (!_themeManager.ActiveAndExtended) return;
            var palette = _themeManager.ActiveTheme.ExtendedPalette;
            if (palette == null) return;
            //List back color
            BackColor = palette.getColor("List_Background");
            ForeColor = palette.getColor("List_Item_Foreground");
            //Selected item
            SelectedBackColor = palette.getColor("List_Item_Selected_Background");
            SelectedForeColor = palette.getColor("List_Item_Selected_Foreground");

            //Header style
            HeaderUsesThemes = false;
            HeaderFormatStyle headerStylo = new()
            {
                Normal =
                {
                    BackColor = palette.getColor("List_Header_Background"),
                    ForeColor = palette.getColor("List_Header_Foreground")
                }
            };
            HeaderFormatStyle = headerStylo;
            //Border style
            if (DecorateLines)
            {
                UseCellFormatEvents = true;
                GridLines = false;
                deco = new CellBorderDecoration
                {
                    BorderPen = new Pen(palette.getColor("List_Item_Border")),
                    FillBrush = null,
                    BoundsPadding = Size.Empty,
                    CornerRounding = 0
                };
                FormatCell += NGListView_FormatCell;
            }

            if (Items != null && Items.Count != 0)
                BuildList();
            Invalidate();
        }

        private void NGListView_FormatCell(object sender, FormatCellEventArgs e)
        {
            if (e.Column.IsVisible)
            {
                e.SubItem.Decoration = deco;
            }
        }

        private void InitializeComponent()
        {
            ((ISupportInitialize)(this)).BeginInit();
            SuspendLayout();
            // 
            // NGListView
            // 
            Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ((ISupportInitialize)(this)).EndInit();
            ResumeLayout(false);
        }
    }
}