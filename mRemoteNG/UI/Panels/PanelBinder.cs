using System;
using System.Runtime.Versioning;
using mRemoteNG.App;
using mRemoteNG.Properties;
using mRemoteNG.UI.Forms;
using WeifenLuo.WinFormsUI.Docking;
using System.Windows.Forms;
using System.ComponentModel;

namespace mRemoteNG.UI.Panels
{
    /// <summary>
    /// Manages the binding between Connections and Config panels so they show/hide together when in auto-hide state
    /// </summary>
    [SupportedOSPlatform("windows")]
    public class PanelBinder
    {
        private static PanelBinder? _instance;
        private bool _isProcessing; // Prevent recursive calls
        
        // Store original auto-hide states
        private DockState _treeFormAutoHideState = DockState.Unknown;
        private DockState _configFormAutoHideState = DockState.Unknown;
        
        // Store original docked states
        private DockState _treeFormDockedState = DockState.Unknown;
        private DockState _configFormDockedState = DockState.Unknown;
        
        // Track if panels are temporarily pinned
        private bool _panelsTemporarilyPinned;
        
        // Timer to check for focus loss
        private Timer _focusCheckTimer;

        public static PanelBinder Instance => _instance ?? (_instance = new PanelBinder());

        private PanelBinder()
        {
            _focusCheckTimer = new Timer();
            _focusCheckTimer.Interval = 250; // Check every 250ms
            _focusCheckTimer.Tick += FocusCheckTimer_Tick;
            
            // Listen for binding option changes
            OptionsTabsPanelsPage.Default.PropertyChanged += OptionsPropertyChanged;
        }
        
        /// <summary>
        /// Responds to changes in the binding option
        /// </summary>
        private void OptionsPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(OptionsTabsPanelsPage.Default.BindConnectionsAndConfigPanels))
            {
                bool bindingEnabled = OptionsTabsPanelsPage.Default.BindConnectionsAndConfigPanels;
                UpdatePanelBindingState(bindingEnabled);
            }
        }
        
        /// <summary>
        /// Updates panel states based on binding setting
        /// </summary>
        private void UpdatePanelBindingState(bool bindingEnabled)
        {
            if (_isProcessing)
                return;
                
            _isProcessing = true;
            try
            {
                if (bindingEnabled)
                {
                    // Binding was enabled - set both panels to auto-hide mode
                    SetPanelsToAutoHide();
                }
                else
                {
                    // Binding was disabled - restore panels to docked mode
                    SetPanelsToDocked();
                    
                    // Stop any active timers
                    _focusCheckTimer.Stop();
                    _panelsTemporarilyPinned = false;
                }
            }
            finally
            {
                _isProcessing = false;
            }
        }
        
        /// <summary>
        /// Sets both panels to auto-hide mode
        /// </summary>
        private void SetPanelsToAutoHide()
        {
            // Save current states if they're not auto-hide
            if (AppWindows.TreeForm != null && !IsAutoHideState(AppWindows.TreeForm.DockState))
            {
                _treeFormDockedState = AppWindows.TreeForm.DockState;
                
                // Set to auto-hide equivalent
                if (AppWindows.TreeForm.DockState == DockState.DockLeft)
                    AppWindows.TreeForm.DockState = DockState.DockLeftAutoHide;
                else if (AppWindows.TreeForm.DockState == DockState.DockRight)
                    AppWindows.TreeForm.DockState = DockState.DockRightAutoHide;
                else if (AppWindows.TreeForm.DockState == DockState.DockTop)
                    AppWindows.TreeForm.DockState = DockState.DockTopAutoHide;
                else if (AppWindows.TreeForm.DockState == DockState.DockBottom)
                    AppWindows.TreeForm.DockState = DockState.DockBottomAutoHide;
                    
                // Save this auto-hide state
                _treeFormAutoHideState = AppWindows.TreeForm.DockState;
            }
            
            if (AppWindows.ConfigForm != null && !IsAutoHideState(AppWindows.ConfigForm.DockState))
            {
                _configFormDockedState = AppWindows.ConfigForm.DockState;
                
                // Set to auto-hide equivalent
                if (AppWindows.ConfigForm.DockState == DockState.DockLeft)
                    AppWindows.ConfigForm.DockState = DockState.DockLeftAutoHide;
                else if (AppWindows.ConfigForm.DockState == DockState.DockRight)
                    AppWindows.ConfigForm.DockState = DockState.DockRightAutoHide;
                else if (AppWindows.ConfigForm.DockState == DockState.DockTop)
                    AppWindows.ConfigForm.DockState = DockState.DockTopAutoHide;
                else if (AppWindows.ConfigForm.DockState == DockState.DockBottom)
                    AppWindows.ConfigForm.DockState = DockState.DockBottomAutoHide;
                    
                // Save this auto-hide state
                _configFormAutoHideState = AppWindows.ConfigForm.DockState;
            }
        }
        
        /// <summary>
        /// Sets both panels to docked (pinned) mode
        /// </summary>
        private void SetPanelsToDocked()
        {
            // Restore to docked states if available, otherwise convert from auto-hide
            if (AppWindows.TreeForm != null)
            {
                if (_treeFormDockedState != DockState.Unknown && _treeFormDockedState != DockState.Hidden)
                {
                    AppWindows.TreeForm.DockState = _treeFormDockedState;
                }
                else if (IsAutoHideState(AppWindows.TreeForm.DockState))
                {
                    // Convert auto-hide to regular docked
                    if (AppWindows.TreeForm.DockState == DockState.DockLeftAutoHide)
                        AppWindows.TreeForm.DockState = DockState.DockLeft;
                    else if (AppWindows.TreeForm.DockState == DockState.DockRightAutoHide)
                        AppWindows.TreeForm.DockState = DockState.DockRight;
                    else if (AppWindows.TreeForm.DockState == DockState.DockTopAutoHide)
                        AppWindows.TreeForm.DockState = DockState.DockTop;
                    else if (AppWindows.TreeForm.DockState == DockState.DockBottomAutoHide)
                        AppWindows.TreeForm.DockState = DockState.DockBottom;
                }
                
                // Explicitly ensure it's not in auto-hide state
                if (IsAutoHideState(AppWindows.TreeForm.DockState))
                {
                    AppWindows.TreeForm.DockState = DockState.DockLeft;
                }
            }
            
            if (AppWindows.ConfigForm != null)
            {
                if (_configFormDockedState != DockState.Unknown && _configFormDockedState != DockState.Hidden)
                {
                    AppWindows.ConfigForm.DockState = _configFormDockedState;
                }
                else if (IsAutoHideState(AppWindows.ConfigForm.DockState))
                {
                    // Convert auto-hide to regular docked
                    if (AppWindows.ConfigForm.DockState == DockState.DockLeftAutoHide)
                        AppWindows.ConfigForm.DockState = DockState.DockLeft;
                    else if (AppWindows.ConfigForm.DockState == DockState.DockRightAutoHide)
                        AppWindows.ConfigForm.DockState = DockState.DockRight;
                    else if (AppWindows.ConfigForm.DockState == DockState.DockTopAutoHide)
                        AppWindows.ConfigForm.DockState = DockState.DockTop;
                    else if (AppWindows.ConfigForm.DockState == DockState.DockBottomAutoHide)
                        AppWindows.ConfigForm.DockState = DockState.DockBottom;
                }
                
                // Explicitly ensure it's not in auto-hide state
                if (IsAutoHideState(AppWindows.ConfigForm.DockState))
                {
                    AppWindows.ConfigForm.DockState = DockState.DockLeft;
                }
            }
            
            // Reset our tracking variables
            _treeFormAutoHideState = DockState.Unknown;
            _configFormAutoHideState = DockState.Unknown;
        }

        /// <summary>
        /// Initializes event handlers for the Connections and Config panels
        /// </summary>
        public void Initialize()
        {
            if (AppWindows.TreeForm != null)
            {
                AppWindows.TreeForm.DockStateChanged += OnTreeFormDockStateChanged;
                AppWindows.TreeForm.Enter += OnPanelEnter;

                // Store initial dock state if not auto-hide
                if (!IsAutoHideState(AppWindows.TreeForm.DockState))
                    _treeFormDockedState = AppWindows.TreeForm.DockState;
            }

            if (AppWindows.ConfigForm != null)
            {
                AppWindows.ConfigForm.DockStateChanged += OnConfigFormDockStateChanged;
                AppWindows.ConfigForm.Enter += OnPanelEnter;

                // Store initial dock state if not auto-hide
                if (!IsAutoHideState(AppWindows.ConfigForm.DockState))
                    _configFormDockedState = AppWindows.ConfigForm.DockState;
            }

            // Hook into DockPanel.ActiveContentChanged — this fires reliably when
            // auto-hidden panels are activated via the auto-hide strip, unlike
            // DockContent.Enter/VisibleChanged which may not fire for overlay panels.
            if (FrmMain.IsCreated)
            {
                FrmMain.Default.pnlDock.ActiveContentChanged += OnDockPanelActiveContentChanged;
            }

            // Apply initial binding state based on option
            if (OptionsTabsPanelsPage.Default.BindConnectionsAndConfigPanels)
            {
                UpdatePanelBindingState(true);
            }
        }

        private void OnTreeFormDockStateChanged(object? sender, EventArgs e)
        {
            if (AppWindows.TreeForm == null)
                return;

            // Save auto-hide state if it's an auto-hide state
            if (IsAutoHideState(AppWindows.TreeForm.DockState))
            {
                _treeFormAutoHideState = AppWindows.TreeForm.DockState;
            }
            // Save docked state if it's a docked state
            else if (AppWindows.TreeForm.DockState != DockState.Hidden &&
                     AppWindows.TreeForm.DockState != DockState.Unknown)
            {
                _treeFormDockedState = AppWindows.TreeForm.DockState;
            }
        }

        private void OnConfigFormDockStateChanged(object? sender, EventArgs e)
        {
            if (AppWindows.ConfigForm == null)
                return;

            // Save auto-hide state if it's an auto-hide state
            if (IsAutoHideState(AppWindows.ConfigForm.DockState))
            {
                _configFormAutoHideState = AppWindows.ConfigForm.DockState;
            }
            // Save docked state if it's a docked state
            else if (AppWindows.ConfigForm.DockState != DockState.Hidden &&
                     AppWindows.ConfigForm.DockState != DockState.Unknown)
            {
                _configFormDockedState = AppWindows.ConfigForm.DockState;
            }
        }

        /// <summary>
        /// Handles when a panel is entered (gets focus)
        /// </summary>
        private void OnPanelEnter(object? sender, EventArgs e)
        {
            TryBindPanels();
        }

        /// <summary>
        /// Handles when the DockPanel's active content changes — fires reliably
        /// for auto-hidden panels activated via the auto-hide strip.
        /// </summary>
        private void OnDockPanelActiveContentChanged(object? sender, EventArgs e)
        {
            if (!FrmMain.IsCreated)
                return;

            IDockContent? active = FrmMain.Default.pnlDock.ActiveContent;
            if (active == AppWindows.TreeForm || active == AppWindows.ConfigForm)
            {
                TryBindPanels();
            }
        }

        /// <summary>
        /// Attempts to bind both panels together when both are auto-hidden
        /// </summary>
        private void TryBindPanels()
        {
            if (!OptionsTabsPanelsPage.Default.BindConnectionsAndConfigPanels || _isProcessing)
                return;

            if (AppWindows.TreeForm == null || AppWindows.ConfigForm == null)
                return;

            // Check if both panels are in auto-hide mode
            if (!IsPanelAutoHidden(AppWindows.TreeForm) || !IsPanelAutoHidden(AppWindows.ConfigForm))
                return;

            _isProcessing = true;
            try
            {
                // Store current auto-hide states if not already stored
                if (_treeFormAutoHideState == DockState.Unknown)
                    _treeFormAutoHideState = AppWindows.TreeForm.DockState;

                if (_configFormAutoHideState == DockState.Unknown)
                    _configFormAutoHideState = AppWindows.ConfigForm.DockState;

                // Pin both panels temporarily (make them normal docked)
                TemporarilyPinPanels();

                // Start checking for focus loss
                _focusCheckTimer.Start();
            }
            finally
            {
                _isProcessing = false;
            }
        }
        
        /// <summary>
        /// Timer to check if both panels have lost focus
        /// </summary>
        private void FocusCheckTimer_Tick(object? sender, EventArgs e)
        {
            if (!_panelsTemporarilyPinned || _isProcessing)
                return;
            
            // Get active form in the application
            Form? activeForm = Form.ActiveForm;
            
            // Check if neither panel has focus
            bool treeHasFocus = AppWindows.TreeForm != null && 
                               (activeForm == AppWindows.TreeForm || 
                                AppWindows.TreeForm.ContainsFocus);
                                
            bool configHasFocus = AppWindows.ConfigForm != null && 
                                 (activeForm == AppWindows.ConfigForm || 
                                  AppWindows.ConfigForm.ContainsFocus);
            
            // If neither panel has focus and panels are temporarily pinned, restore auto-hide
            if (!treeHasFocus && !configHasFocus)
            {
                _isProcessing = true;
                try
                {
                    RestoreAutoHideState();
                }
                finally
                {
                    _isProcessing = false;
                }
            }
        }

        /// <summary>
        /// Temporarily pins both panels (makes them normal docked panels)
        /// </summary>
        private void TemporarilyPinPanels()
        {
            if (_panelsTemporarilyPinned)
                return;
                
            // For TreeForm: change from auto-hide to normal docked
            if (AppWindows.TreeForm != null && IsPanelAutoHidden(AppWindows.TreeForm))
            {
                // Convert auto-hide state to regular docked state
                if (AppWindows.TreeForm.DockState == DockState.DockLeftAutoHide)
                    AppWindows.TreeForm.DockState = DockState.DockLeft;
                else if (AppWindows.TreeForm.DockState == DockState.DockRightAutoHide)
                    AppWindows.TreeForm.DockState = DockState.DockRight;
                else if (AppWindows.TreeForm.DockState == DockState.DockTopAutoHide)
                    AppWindows.TreeForm.DockState = DockState.DockTop;
                else if (AppWindows.TreeForm.DockState == DockState.DockBottomAutoHide)
                    AppWindows.TreeForm.DockState = DockState.DockBottom;
            }
            
            // For ConfigForm: change from auto-hide to normal docked
            if (AppWindows.ConfigForm != null && IsPanelAutoHidden(AppWindows.ConfigForm))
            {
                // Convert auto-hide state to regular docked state
                if (AppWindows.ConfigForm.DockState == DockState.DockLeftAutoHide)
                    AppWindows.ConfigForm.DockState = DockState.DockLeft;
                else if (AppWindows.ConfigForm.DockState == DockState.DockRightAutoHide)
                    AppWindows.ConfigForm.DockState = DockState.DockRight;
                else if (AppWindows.ConfigForm.DockState == DockState.DockTopAutoHide)
                    AppWindows.ConfigForm.DockState = DockState.DockTop;
                else if (AppWindows.ConfigForm.DockState == DockState.DockBottomAutoHide)
                    AppWindows.ConfigForm.DockState = DockState.DockBottom;
            }
            
            _panelsTemporarilyPinned = true;
            
            // Ensure both panels are visible and active
            if (AppWindows.TreeForm != null)
            {
                AppWindows.TreeForm.Show();
                AppWindows.TreeForm.Activate();
            }
            
            if (AppWindows.ConfigForm != null)
            {
                AppWindows.ConfigForm.Show();
                AppWindows.ConfigForm.Activate();
            }
        }
        
        /// <summary>
        /// Restore both panels to their original auto-hide state
        /// </summary>
        private void RestoreAutoHideState()
        {
            if (!_panelsTemporarilyPinned)
                return;
                
            _focusCheckTimer.Stop();
            
            // Restore TreeForm to its auto-hide state
            if (AppWindows.TreeForm != null && _treeFormAutoHideState != DockState.Unknown)
            {
                AppWindows.TreeForm.DockState = _treeFormAutoHideState;
            }
            
            // Restore ConfigForm to its auto-hide state
            if (AppWindows.ConfigForm != null && _configFormAutoHideState != DockState.Unknown)
            {
                AppWindows.ConfigForm.DockState = _configFormAutoHideState;
            }
            
            _panelsTemporarilyPinned = false;
        }
        
        /// <summary>
        /// Checks if a dock state is an auto-hide state
        /// </summary>
        private static bool IsAutoHideState(DockState state)
        {
            return state == DockState.DockLeftAutoHide ||
                   state == DockState.DockRightAutoHide ||
                   state == DockState.DockTopAutoHide ||
                   state == DockState.DockBottomAutoHide;
        }

        /// <summary>
        /// Checks if a panel is in auto-hide state
        /// </summary>
        private static bool IsPanelAutoHidden(DockContent panel)
        {
            if (panel == null)
                return false;

            return IsAutoHideState(panel.DockState);
        }
    }
}