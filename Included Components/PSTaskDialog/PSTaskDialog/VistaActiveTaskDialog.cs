//------------------------------------------------------------------
// <summary>
// A P/Invoke wrapper for TaskDialog. Usability was given preference to perf and size.
// </summary>
//
// <remarks/>
//------------------------------------------------------------------

namespace PSTaskDialog
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;
    using System.Runtime.InteropServices;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// The active Task Dialog window. Provides several methods for acting on the active TaskDialog.
    /// You should not use this object after the TaskDialog Destroy notification callback. Doing so
    /// will result in undefined behavior and likely crash.
    /// </summary>
    public class VistaActiveTaskDialog : IWin32Window
    {
        /// <summary>
        /// The Task Dialog's window handle.
        /// </summary>
        [SuppressMessage("Microsoft.Reliability", "CA2006:UseSafeHandleToEncapsulateNativeResources")] // We don't own the window.
        private IntPtr handle;

        /// <summary>
        /// Creates a ActiveTaskDialog.
        /// </summary>
        /// <param name="handle">The Task Dialog's window handle.</param>
        internal VistaActiveTaskDialog(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                throw new ArgumentNullException("handle");
            }

            this.handle = handle;
        }

        /// <summary>
        /// The Task Dialog's window handle.
        /// </summary>
        public IntPtr Handle
        {
            get { return this.handle; }
        }

        //// Not supported. Task Dialog Spec does not indicate what this is for.
        ////public void NavigatePage()
        ////{
        ////    // TDM_NAVIGATE_PAGE                   = WM_USER+101,
        ////    UnsafeNativeMethods.SendMessage(
        ////        this.windowHandle,
        ////        (uint)UnsafeNativeMethods.TASKDIALOG_MESSAGES.TDM_NAVIGATE_PAGE,
        ////        IntPtr.Zero,
        ////        //a UnsafeNativeMethods.TASKDIALOGCONFIG value);
        ////}

        /// <summary>
        /// Simulate the action of a button click in the TaskDialog. This can be a DialogResult value 
        /// or the ButtonID set on a TasDialogButton set on TaskDialog.Buttons.
        /// </summary>
        /// <param name="buttonId">Indicates the button ID to be selected.</param>
        /// <returns>If the function succeeds the return value is true.</returns>
        public bool ClickButton(int buttonId)
        {
            // TDM_CLICK_BUTTON                    = WM_USER+102, // wParam = Button ID
            return VistaUnsafeNativeMethods.SendMessage(
                this.handle,
                (uint)VistaUnsafeNativeMethods.TASKDIALOG_MESSAGES.TDM_CLICK_BUTTON,
                (IntPtr)buttonId,
                IntPtr.Zero) != IntPtr.Zero;
        }

        /// <summary>
        /// Used to indicate whether the hosted progress bar should be displayed in marquee mode or not.
        /// </summary>
        /// <param name="marquee">Specifies whether the progress bar sbould be shown in Marquee mode.
        /// A value of true turns on Marquee mode.</param>
        /// <returns>If the function succeeds the return value is true.</returns>
        public bool SetMarqueeProgressBar(bool marquee)
        {
            // TDM_SET_MARQUEE_PROGRESS_BAR        = WM_USER+103, // wParam = 0 (nonMarque) wParam != 0 (Marquee)
            return VistaUnsafeNativeMethods.SendMessage(
                this.handle,
                (uint)VistaUnsafeNativeMethods.TASKDIALOG_MESSAGES.TDM_SET_MARQUEE_PROGRESS_BAR,
                (marquee ? (IntPtr)1 : IntPtr.Zero),
                IntPtr.Zero) != IntPtr.Zero;

            // Future: get more detailed error from and throw.
        }

        /// <summary>
        /// Sets the state of the progress bar.
        /// </summary>
        /// <param name="newState">The state to set the progress bar.</param>
        /// <returns>If the function succeeds the return value is true.</returns>
        public bool SetProgressBarState(VistaProgressBarState newState)
        {
            // TDM_SET_PROGRESS_BAR_STATE          = WM_USER+104, // wParam = new progress state
            return VistaUnsafeNativeMethods.SendMessage(
                this.handle,
                (uint)VistaUnsafeNativeMethods.TASKDIALOG_MESSAGES.TDM_SET_PROGRESS_BAR_STATE,
                (IntPtr)newState,
                IntPtr.Zero) != IntPtr.Zero;

            // Future: get more detailed error from and throw.
        }

        /// <summary>
        /// Set the minimum and maximum values for the hosted progress bar.
        /// </summary>
        /// <param name="minRange">Minimum range value. By default, the minimum value is zero.</param>
        /// <param name="maxRange">Maximum range value.  By default, the maximum value is 100.</param>
        /// <returns>If the function succeeds the return value is true.</returns>
        public bool SetProgressBarRange(Int16 minRange, Int16 maxRange)
        {
            // TDM_SET_PROGRESS_BAR_RANGE          = WM_USER+105, // lParam = MAKELPARAM(nMinRange, nMaxRange)
            // #define MAKELPARAM(l, h)      ((LPARAM)(DWORD)MAKELONG(l, h))
            // #define MAKELONG(a, b)      ((LONG)(((WORD)(((DWORD_PTR)(a)) & 0xffff)) | ((DWORD)((WORD)(((DWORD_PTR)(b)) & 0xffff))) << 16))
            IntPtr lparam = (IntPtr)((((Int32)minRange) & 0xffff) | ((((Int32)maxRange) & 0xffff) << 16));
            return VistaUnsafeNativeMethods.SendMessage(
                this.handle,
                (uint)VistaUnsafeNativeMethods.TASKDIALOG_MESSAGES.TDM_SET_PROGRESS_BAR_RANGE,
                IntPtr.Zero,
                lparam) != IntPtr.Zero;

            // Return value is actually prior range.
        }

        /// <summary>
        /// Set the current position for a progress bar.
        /// </summary>
        /// <param name="newPosition">The new position.</param>
        /// <returns>Returns the previous value if successful, or zero otherwise.</returns>
        public int SetProgressBarPosition(int newPosition)
        {
            // TDM_SET_PROGRESS_BAR_POS            = WM_USER+106, // wParam = new position
            return (int)VistaUnsafeNativeMethods.SendMessage(
                this.handle,
                (uint)VistaUnsafeNativeMethods.TASKDIALOG_MESSAGES.TDM_SET_PROGRESS_BAR_POS,
                (IntPtr)newPosition,
                IntPtr.Zero);
        }

        /// <summary>
        /// Sets the animation state of the Marquee Progress Bar.
        /// </summary>
        /// <param name="startMarquee">true starts the marquee animation and false stops it.</param>
        /// <param name="speed">The time in milliseconds between refreshes.</param>
        public void SetProgressBarMarquee(bool startMarquee, uint speed)
        {
            // TDM_SET_PROGRESS_BAR_MARQUEE        = WM_USER+107, // wParam = 0 (stop marquee), wParam != 0 (start marquee), lparam = speed (milliseconds between repaints)
            VistaUnsafeNativeMethods.SendMessage(
                this.handle,
                (uint)VistaUnsafeNativeMethods.TASKDIALOG_MESSAGES.TDM_SET_PROGRESS_BAR_MARQUEE,
                (startMarquee ? new IntPtr(1) : IntPtr.Zero),
                (IntPtr)speed);
        }

        /// <summary>
        /// Updates the content text.
        /// </summary>
        /// <param name="content">The new value.</param>
        /// <returns>If the function succeeds the return value is true.</returns>
        public bool SetContent(string content)
        {
            // TDE_CONTENT,
            // TDM_SET_ELEMENT_TEXT                = WM_USER+108  // wParam = element (TASKDIALOG_ELEMENTS), lParam = new element text (LPCWSTR)
            return VistaUnsafeNativeMethods.SendMessageWithString(
                this.handle,
                (uint)VistaUnsafeNativeMethods.TASKDIALOG_MESSAGES.TDM_SET_ELEMENT_TEXT,
                (IntPtr)VistaUnsafeNativeMethods.TASKDIALOG_ELEMENTS.TDE_CONTENT,
                content) != IntPtr.Zero;
        }

        /// <summary>
        /// Updates the Expanded Information text.
        /// </summary>
        /// <param name="expandedInformation">The new value.</param>
        /// <returns>If the function succeeds the return value is true.</returns>
        public bool SetExpandedInformation(string expandedInformation)
        {
            // TDE_EXPANDED_INFORMATION,
            // TDM_SET_ELEMENT_TEXT                = WM_USER+108  // wParam = element (TASKDIALOG_ELEMENTS), lParam = new element text (LPCWSTR)
            return VistaUnsafeNativeMethods.SendMessageWithString(
                this.handle,
                (uint)VistaUnsafeNativeMethods.TASKDIALOG_MESSAGES.TDM_SET_ELEMENT_TEXT,
                (IntPtr)VistaUnsafeNativeMethods.TASKDIALOG_ELEMENTS.TDE_EXPANDED_INFORMATION,
                expandedInformation) != IntPtr.Zero;
        }

        /// <summary>
        /// Updates the Footer text.
        /// </summary>
        /// <param name="footer">The new value.</param>
        /// <returns>If the function succeeds the return value is true.</returns>
        public bool SetFooter(string footer)
        {
            // TDE_FOOTER,
            // TDM_SET_ELEMENT_TEXT                = WM_USER+108  // wParam = element (TASKDIALOG_ELEMENTS), lParam = new element text (LPCWSTR)
            return VistaUnsafeNativeMethods.SendMessageWithString(
                this.handle,
                (uint)VistaUnsafeNativeMethods.TASKDIALOG_MESSAGES.TDM_SET_ELEMENT_TEXT,
                (IntPtr)VistaUnsafeNativeMethods.TASKDIALOG_ELEMENTS.TDE_FOOTER,
                footer) != IntPtr.Zero;
        }

        /// <summary>
        /// Updates the Main Instruction.
        /// </summary>
        /// <param name="mainInstruction">The new value.</param>
        /// <returns>If the function succeeds the return value is true.</returns>
        public bool SetMainInstruction(string mainInstruction)
        {
            // TDE_MAIN_INSTRUCTION
            // TDM_SET_ELEMENT_TEXT                = WM_USER+108  // wParam = element (TASKDIALOG_ELEMENTS), lParam = new element text (LPCWSTR)
            return VistaUnsafeNativeMethods.SendMessageWithString(
                this.handle,
                (uint)VistaUnsafeNativeMethods.TASKDIALOG_MESSAGES.TDM_SET_ELEMENT_TEXT,
                (IntPtr)VistaUnsafeNativeMethods.TASKDIALOG_ELEMENTS.TDE_MAIN_INSTRUCTION,
                mainInstruction) != IntPtr.Zero;
        }

        /// <summary>
        /// Simulate the action of a radio button click in the TaskDialog. 
        /// The passed buttonID is the ButtonID set on a TaskDialogButton set on TaskDialog.RadioButtons.
        /// </summary>
        /// <param name="buttonId">Indicates the button ID to be selected.</param>
        public void ClickRadioButton(int buttonId)
        {
            // TDM_CLICK_RADIO_BUTTON = WM_USER+110, // wParam = Radio Button ID
            VistaUnsafeNativeMethods.SendMessage(
                this.handle,
                (uint)VistaUnsafeNativeMethods.TASKDIALOG_MESSAGES.TDM_CLICK_RADIO_BUTTON,
                (IntPtr)buttonId,
                IntPtr.Zero);
        }

        /// <summary>
        /// Enable or disable a button in the TaskDialog. 
        /// The passed buttonID is the ButtonID set on a TaskDialogButton set on TaskDialog.Buttons
        /// or a common button ID.
        /// </summary>
        /// <param name="buttonId">Indicates the button ID to be enabled or diabled.</param>
        /// <param name="enable">Enambe the button if true. Disable the button if false.</param>
        public void EnableButton(int buttonId, bool enable)
        {
            // TDM_ENABLE_BUTTON = WM_USER+111, // lParam = 0 (disable), lParam != 0 (enable), wParam = Button ID
            VistaUnsafeNativeMethods.SendMessage(
                this.handle,
                (uint)VistaUnsafeNativeMethods.TASKDIALOG_MESSAGES.TDM_ENABLE_BUTTON,
                (IntPtr)buttonId,
                (IntPtr)(enable ? 0 : 1 ));
        }

        /// <summary>
        /// Enable or disable a radio button in the TaskDialog. 
        /// The passed buttonID is the ButtonID set on a TaskDialogButton set on TaskDialog.RadioButtons.
        /// </summary>
        /// <param name="buttonId">Indicates the button ID to be enabled or diabled.</param>
        /// <param name="enable">Enambe the button if true. Disable the button if false.</param>
        public void EnableRadioButton(int buttonId, bool enable)
        {
            // TDM_ENABLE_RADIO_BUTTON = WM_USER+112, // lParam = 0 (disable), lParam != 0 (enable), wParam = Radio Button ID
            VistaUnsafeNativeMethods.SendMessage(
                this.handle,
                (uint)VistaUnsafeNativeMethods.TASKDIALOG_MESSAGES.TDM_ENABLE_RADIO_BUTTON,
                (IntPtr)buttonId,
                (IntPtr)(enable ? 0 : 1));
        }

        /// <summary>
        /// Check or uncheck the verification checkbox in the TaskDialog. 
        /// </summary>
        /// <param name="checkedState">The checked state to set the verification checkbox.</param>
        /// <param name="setKeyboardFocusToCheckBox">True to set the keyboard focus to the checkbox, and fasle otherwise.</param>
        public void ClickVerification(bool checkedState, bool setKeyboardFocusToCheckBox)
        {
            // TDM_CLICK_VERIFICATION = WM_USER+113, // wParam = 0 (unchecked), 1 (checked), lParam = 1 (set key focus)
            VistaUnsafeNativeMethods.SendMessage(
                this.handle,
                (uint)VistaUnsafeNativeMethods.TASKDIALOG_MESSAGES.TDM_CLICK_VERIFICATION,
                (checkedState ? new IntPtr(1) : IntPtr.Zero),
                (setKeyboardFocusToCheckBox ? new IntPtr(1) : IntPtr.Zero));
        }

        /// <summary>
        /// Updates the content text.
        /// </summary>
        /// <param name="content">The new value.</param>
        public void UpdateContent(string content)
        {
            // TDE_CONTENT,
            // TDM_UPDATE_ELEMENT_TEXT             = WM_USER+114, // wParam = element (TASKDIALOG_ELEMENTS), lParam = new element text (LPCWSTR)
            VistaUnsafeNativeMethods.SendMessageWithString(
                this.handle,
                (uint)VistaUnsafeNativeMethods.TASKDIALOG_MESSAGES.TDM_UPDATE_ELEMENT_TEXT,
                (IntPtr)VistaUnsafeNativeMethods.TASKDIALOG_ELEMENTS.TDE_CONTENT,
                content);
        }

        /// <summary>
        /// Updates the Expanded Information text. No effect if it was previously set to null.
        /// </summary>
        /// <param name="expandedInformation">The new value.</param>
        public void UpdateExpandedInformation(string expandedInformation)
        {
            // TDE_EXPANDED_INFORMATION,
            // TDM_UPDATE_ELEMENT_TEXT             = WM_USER+114, // wParam = element (TASKDIALOG_ELEMENTS), lParam = new element text (LPCWSTR)
            VistaUnsafeNativeMethods.SendMessageWithString(
                this.handle,
                (uint)VistaUnsafeNativeMethods.TASKDIALOG_MESSAGES.TDM_UPDATE_ELEMENT_TEXT,
                (IntPtr)VistaUnsafeNativeMethods.TASKDIALOG_ELEMENTS.TDE_EXPANDED_INFORMATION,
                expandedInformation);
        }

        /// <summary>
        /// Updates the Footer text. No Effect if it was perviously set to null.
        /// </summary>
        /// <param name="footer">The new value.</param>
        public void UpdateFooter(string footer)
        {
            // TDE_FOOTER,
            // TDM_UPDATE_ELEMENT_TEXT             = WM_USER+114, // wParam = element (TASKDIALOG_ELEMENTS), lParam = new element text (LPCWSTR)
            VistaUnsafeNativeMethods.SendMessageWithString(
                this.handle,
                (uint)VistaUnsafeNativeMethods.TASKDIALOG_MESSAGES.TDM_UPDATE_ELEMENT_TEXT,
                (IntPtr)VistaUnsafeNativeMethods.TASKDIALOG_ELEMENTS.TDE_FOOTER,
                footer);
        }

        /// <summary>
        /// Updates the Main Instruction.
        /// </summary>
        /// <param name="mainInstruction">The new value.</param>
        public void UpdateMainInstruction(string mainInstruction)
        {
            // TDE_MAIN_INSTRUCTION
            // TDM_UPDATE_ELEMENT_TEXT             = WM_USER+114, // wParam = element (TASKDIALOG_ELEMENTS), lParam = new element text (LPCWSTR)
            VistaUnsafeNativeMethods.SendMessageWithString(
                this.handle,
                (uint)VistaUnsafeNativeMethods.TASKDIALOG_MESSAGES.TDM_UPDATE_ELEMENT_TEXT,
                (IntPtr)VistaUnsafeNativeMethods.TASKDIALOG_ELEMENTS.TDE_MAIN_INSTRUCTION,
                mainInstruction);
        }

        /// <summary>
        /// Designate whether a given Task Dialog button or command link should have a User Account Control (UAC) shield icon.
        /// </summary>
        /// <param name="buttonId">ID of the push button or command link to be updated.</param>
        /// <param name="elevationRequired">False to designate that the action invoked by the button does not require elevation;
        /// true to designate that the action does require elevation.</param>
        public void SetButtonElevationRequiredState(int buttonId, bool elevationRequired)
        {
            // TDM_SET_BUTTON_ELEVATION_REQUIRED_STATE = WM_USER+115, // wParam = Button ID, lParam = 0 (elevation not required), lParam != 0 (elevation required)
            VistaUnsafeNativeMethods.SendMessage(
                this.handle,
                (uint)VistaUnsafeNativeMethods.TASKDIALOG_MESSAGES.TDM_SET_BUTTON_ELEVATION_REQUIRED_STATE,
                (IntPtr)buttonId,
                (IntPtr)(elevationRequired ? new IntPtr(1) : IntPtr.Zero));
        }

        /// <summary>
        /// Updates the main instruction icon. Note the type (standard via enum or
        /// custom via Icon type) must be used when upating the icon.
        /// </summary>
        /// <param name="icon">Task Dialog standard icon.</param>
        public void UpdateMainIcon(VistaTaskDialogIcon icon)
        {
            // TDM_UPDATE_ICON = WM_USER+116  // wParam = icon element (TASKDIALOG_ICON_ELEMENTS), lParam = new icon (hIcon if TDF_USE_HICON_* was set, PCWSTR otherwise)
            VistaUnsafeNativeMethods.SendMessage(
                this.handle,
                (uint)VistaUnsafeNativeMethods.TASKDIALOG_MESSAGES.TDM_UPDATE_ICON,
                (IntPtr)VistaUnsafeNativeMethods.TASKDIALOG_ICON_ELEMENTS.TDIE_ICON_MAIN,
                (IntPtr)icon);
        }

        /// <summary>
        /// Updates the main instruction icon. Note the type (standard via enum or
        /// custom via Icon type) must be used when upating the icon.
        /// </summary>
        /// <param name="icon">The icon to set.</param>
        public void UpdateMainIcon(Icon icon)
        {
            // TDM_UPDATE_ICON = WM_USER+116  // wParam = icon element (TASKDIALOG_ICON_ELEMENTS), lParam = new icon (hIcon if TDF_USE_HICON_* was set, PCWSTR otherwise)
            VistaUnsafeNativeMethods.SendMessage(
                this.handle,
                (uint)VistaUnsafeNativeMethods.TASKDIALOG_MESSAGES.TDM_UPDATE_ICON,
                (IntPtr)VistaUnsafeNativeMethods.TASKDIALOG_ICON_ELEMENTS.TDIE_ICON_MAIN,
                (icon == null ? IntPtr.Zero : icon.Handle));
        }

        /// <summary>
        /// Updates the footer icon. Note the type (standard via enum or
        /// custom via Icon type) must be used when upating the icon.
        /// </summary>
        /// <param name="icon">Task Dialog standard icon.</param>
        public void UpdateFooterIcon(VistaTaskDialogIcon icon)
        {
            // TDM_UPDATE_ICON = WM_USER+116  // wParam = icon element (TASKDIALOG_ICON_ELEMENTS), lParam = new icon (hIcon if TDF_USE_HICON_* was set, PCWSTR otherwise)
            VistaUnsafeNativeMethods.SendMessage(
                this.handle,
                (uint)VistaUnsafeNativeMethods.TASKDIALOG_MESSAGES.TDM_UPDATE_ICON,
                (IntPtr)VistaUnsafeNativeMethods.TASKDIALOG_ICON_ELEMENTS.TDIE_ICON_FOOTER,
                (IntPtr)icon);
        }

        /// <summary>
        /// Updates the footer icon. Note the type (standard via enum or
        /// custom via Icon type) must be used when upating the icon.
        /// </summary>
        /// <param name="icon">The icon to set.</param>
        public void UpdateFooterIcon(Icon icon)
        {
            // TDM_UPDATE_ICON = WM_USER+116  // wParam = icon element (TASKDIALOG_ICON_ELEMENTS), lParam = new icon (hIcon if TDF_USE_HICON_* was set, PCWSTR otherwise)
            VistaUnsafeNativeMethods.SendMessage(
                this.handle,
                (uint)VistaUnsafeNativeMethods.TASKDIALOG_MESSAGES.TDM_UPDATE_ICON,
                (IntPtr)VistaUnsafeNativeMethods.TASKDIALOG_ICON_ELEMENTS.TDIE_ICON_FOOTER,
                (icon == null ? IntPtr.Zero : icon.Handle));
        }
    }
}
