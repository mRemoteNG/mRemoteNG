using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mRemoteNG.UI.Window;
using WeifenLuo.WinFormsUI.Docking;

namespace mRemoteNG.UI
{
    public static class DockPanelHelper
    {
        public enum Direction { Left = -1, Right = 1};
        /// <summary>
        /// Helper method to retrieve the array representation of the currently docked documents.
        /// </summary>
        /// <param name="dockPanel">The panel itself</param>
        /// <returns>DockPanelDocumentInfo about the docks and the selected index</returns>
        public static DockPanelDocumentInfo GetDocumentInfo(this DockPanel dockPanel)
        {
            DockPanelDocumentInfo re = new DockPanelDocumentInfo();
            re.DockContents = new IDockContent[dockPanel.DocumentsCount];
            re.SelectedDockContentIndex = -1;

            var i = 0;
            foreach (var idc in dockPanel.Documents)
            {
                if (idc == dockPanel.ActiveDocument)
                {
                    re.SelectedDockContentIndex = i;
                    re.SelectedDockContent = idc;
                }
                re.DockContents[i++] = idc;
            }

            return re;
        }

        /// <summary>
        /// Changes the active document of the panel. If reaches the border, delegates the navigation request to the parent dock, if any.
        /// </summary>
        /// <param name="dockPanel">The panel itself</param>
        /// <param name="direction">Direction of the navigation</param>
        public static void NavigateDocument(this DockPanel dockPanel, Direction direction, bool delegateDownToDockPanels = true)
        {
            DockPanelDocumentInfo info = dockPanel.GetDocumentInfo();
            if(info.SelectedDockContent != null)
            {
                if(delegateDownToDockPanels)
                {
                    // lets see if our currently selected document is a dock panel as well
                    // in that case, we delegate the request to this child

                    if ((info.SelectedDockContent is ConnectionWindow connectionWindow)&&(connectionWindow.Controls.Count > 0)&&(connectionWindow.Controls[0] is DockPanel child))
                    {
                        child.NavigateDocument(direction, true);
                        return;
                    }

                }

                var newIndex = info.SelectedDockContentIndex + (int)direction;
                if((newIndex > -1)&&(newIndex < info.DockContents.Length))
                {
                    // the desired new index belongs to the current dock panel, we can navigate internally.
                    info.DockContents[newIndex].DockHandler.Activate();
                    return;
                }
            }

            // couldnt navigate internally, lets delegate the navigation request to our parent (if it is a dockPanel)
            if(
                (dockPanel.Parent is ConnectionWindow parentConnectionWindow)&&
                (parentConnectionWindow.Parent is DockPane parentPane)&&
                (parentPane.Parent is DockWindow parentWindow) &&
                (parentWindow.Parent is DockPanel parent)
              )
            {
                parent.NavigateDocument(direction, false);
            }
        }
    }

    public struct DockPanelDocumentInfo
    {
        public int SelectedDockContentIndex;
        public IDockContent SelectedDockContent;
        public IDockContent[] DockContents;
    }
}
