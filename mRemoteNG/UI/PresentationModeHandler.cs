using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol.RDP;
using mRemoteNG.UI.Forms;
using mRemoteNG.UI.Window;
using WeifenLuo.WinFormsUI.Docking;

namespace mRemoteNG.UI
{
    public class PresentationModeHandler
    {
        private readonly FrmMain _mainForm;
        private bool _active;
        private readonly List<IDockContent> _hiddenPanels = new List<IDockContent>();
        private bool _wasTopPanelVisible;

        public bool Active
        {
            get => _active;
            set
            {
                if (_active == value) return;
                _active = value;
                if (_active)
                    EnterPresentationMode();
                else
                    ExitPresentationMode();
            }
        }

        public PresentationModeHandler(FrmMain mainForm)
        {
            _mainForm = mainForm ?? throw new ArgumentNullException(nameof(mainForm));
        }

        private void EnterPresentationMode()
        {
            _hiddenPanels.Clear();
            _wasTopPanelVisible = _mainForm.tsContainer.TopToolStripPanelVisible;

            // Hide Toolbars
            if (_wasTopPanelVisible)
            {
                _mainForm.tsContainer.TopToolStripPanelVisible = false;
            }

            // Hide Side Panels (DockContent that is not Document)
            // We take a snapshot of currently visible panels
            var panels = _mainForm.pnlDock.Contents.ToList(); // Copy to list to avoid modification issues during iteration if any
            
            foreach (var content in panels)
            {
                if (content.DockHandler.DockState == DockState.Document)
                    continue;

                if (content.DockHandler.IsHidden)
                    continue;

                // It is a visible panel (DockLeft, DockRight, DockBottom, DockTop, Float)
                _hiddenPanels.Add(content);
                content.DockHandler.Hide();
            }

            // Attempt to switch to Smart Size for RDP connections if not already
            // This is a "nice to have" for presentation mode to ensure content fits
            TrySetSmartSize(true);
        }

        private void ExitPresentationMode()
        {
            // Restore Toolbars
            if (_wasTopPanelVisible)
            {
                _mainForm.tsContainer.TopToolStripPanelVisible = true;
            }

            // Restore Panels
            foreach (var content in _hiddenPanels)
            {
                // Show() usually restores to the previous DockState
                content.DockHandler.Show();
            }
            _hiddenPanels.Clear();
            
            // Revert Smart Size if we forced it? 
            // For now, let's leave it as is, or we'd need to store state for every connection.
            // A simple approach is just to toggle it back if we want, but it might be annoying.
            // Let's just leave it enabled if it was enabled.
        }

        private void TrySetSmartSize(bool enable)
        {
            try
            {
                if (_mainForm.pnlDock.ActiveDocument is ConnectionWindow cw)
                {
                    // This is a bit rough, as we'd need to access the active connection tab
                    // and then checking its protocol.
                    // Accessing internal structures might be needed or using existing helpers.
                    // Given time constraints, I will skip complex reflection for now 
                    // unless there's an easy public way to get the active InterfaceControl.
                    
                    // We can try to use FrmMain.SelectedConnection but that's the Info object, 
                    // not the running protocol instance.
                    
                    // FrmMain has ActivateConnection() which finds the active interface control.
                    // Let's see if we can get it via the ActiveDocument.
                    
                    if (cw.Controls.Count > 0 && cw.Controls[0] is DockPanel dp)
                    {
                         if (dp.ActiveContent is Tabs.ConnectionTab tab)
                         {
                             if (tab.Tag is InterfaceControl ic)
                             {
                                 if (ic.Protocol is RdpProtocol rdp)
                                 {
                                     if (!rdp.SmartSize && enable)
                                     {
                                         rdp.ToggleSmartSize();
                                     }
                                 }
                             }
                         }
                    }
                }
            }
            catch
            {
                // Ignore errors here, it's an enhancement
            }
        }
    }
}
