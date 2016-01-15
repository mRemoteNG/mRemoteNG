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
    /// Class to hold native code interop declarations.
    /// </summary>
    internal static partial class VistaUnsafeNativeMethods
    {
        /// <summary>
        /// WM_USER taken from WinUser.h
        /// </summary>
        internal const uint WM_USER = 0x0400;

        /// <summary>
        /// The signature of the callback that receives messages from the Task Dialog when various events occur.
        /// </summary>
        /// <param name="hwnd">The window handle of the </param>
        /// <param name="msg">The message being passed.</param>
        /// <param name="wParam">wParam which is interpreted differently depending on the message.</param>
        /// <param name="lParam">wParam which is interpreted differently depending on the message.</param>
        /// <param name="refData">The refrence data that was set to TaskDialog.CallbackData.</param>
        /// <returns>A HRESULT value. The return value is specific to the message being processed. </returns>
        internal delegate int VistaTaskDialogCallback([In] IntPtr hwnd, [In] uint msg, [In] UIntPtr wParam, [In] IntPtr lParam, [In] IntPtr refData);

        /// <summary>
        /// TASKDIALOG_FLAGS taken from CommCtrl.h.
        /// </summary>
        [Flags]
        internal enum TASKDIALOG_FLAGS
        {
            /// <summary>
            /// Enable hyperlinks.
            /// </summary>
            TDF_ENABLE_HYPERLINKS = 0x0001,

            /// <summary>
            /// Use icon handle for main icon.
            /// </summary>
            TDF_USE_HICON_MAIN = 0x0002,

            /// <summary>
            /// Use icon handle for footer icon.
            /// </summary>
            TDF_USE_HICON_FOOTER = 0x0004,

            /// <summary>
            /// Allow dialog to be cancelled, even if there is no cancel button.
            /// </summary>
            TDF_ALLOW_DIALOG_CANCELLATION = 0x0008,

            /// <summary>
            /// Use command links rather than buttons.
            /// </summary>
            TDF_USE_COMMAND_LINKS = 0x0010,

            /// <summary>
            /// Use command links with no icons rather than buttons.
            /// </summary>
            TDF_USE_COMMAND_LINKS_NO_ICON = 0x0020,

            /// <summary>
            /// Show expanded info in the footer area.
            /// </summary>
            TDF_EXPAND_FOOTER_AREA = 0x0040,

            /// <summary>
            /// Expand by default.
            /// </summary>
            TDF_EXPANDED_BY_DEFAULT = 0x0080,

            /// <summary>
            /// Start with verification flag already checked.
            /// </summary>
            TDF_VERIFICATION_FLAG_CHECKED = 0x0100,

            /// <summary>
            /// Show a progress bar.
            /// </summary>
            TDF_SHOW_PROGRESS_BAR = 0x0200,

            /// <summary>
            /// Show a marquee progress bar.
            /// </summary>
            TDF_SHOW_MARQUEE_PROGRESS_BAR = 0x0400,

            /// <summary>
            /// Callback every 200 milliseconds.
            /// </summary>
            TDF_CALLBACK_TIMER = 0x0800,

            /// <summary>
            /// Center the dialog on the owner window rather than the monitor.
            /// </summary>
            TDF_POSITION_RELATIVE_TO_WINDOW = 0x1000,

            /// <summary>
            /// Right to Left Layout.
            /// </summary>
            TDF_RTL_LAYOUT = 0x2000,

            /// <summary>
            /// No default radio button.
            /// </summary>
            TDF_NO_DEFAULT_RADIO_BUTTON = 0x4000,

            /// <summary>
            /// Task Dialog can be minimized.
            /// </summary>
            TDF_CAN_BE_MINIMIZED = 0x8000
        }

        /// <summary>
        /// TASKDIALOG_ELEMENTS taken from CommCtrl.h
        /// </summary>
        internal enum TASKDIALOG_ELEMENTS
        {
            /// <summary>
            /// The content element.
            /// </summary>
            TDE_CONTENT,

            /// <summary>
            /// Expanded Information.
            /// </summary>
            TDE_EXPANDED_INFORMATION,

            /// <summary>
            /// Footer.
            /// </summary>
            TDE_FOOTER,

            /// <summary>
            /// Main Instructions
            /// </summary>
            TDE_MAIN_INSTRUCTION
        }

        /// <summary>
        /// TASKDIALOG_ICON_ELEMENTS taken from CommCtrl.h
        /// </summary>
        internal enum TASKDIALOG_ICON_ELEMENTS
        {
            /// <summary>
            /// Main instruction icon.
            /// </summary>
            TDIE_ICON_MAIN,

            /// <summary>
            /// Footer icon.
            /// </summary>
            TDIE_ICON_FOOTER
        }

        /// <summary>
        /// TASKDIALOG_MESSAGES taken from CommCtrl.h.
        /// </summary>
        internal enum TASKDIALOG_MESSAGES : uint
        {
            // Spec is not clear on what this is for.
            ///// <summary>
            ///// Navigate page.
            ///// </summary>
            ////TDM_NAVIGATE_PAGE = WM_USER + 101,

            /// <summary>
            /// Click button.
            /// </summary>
            TDM_CLICK_BUTTON = WM_USER + 102, // wParam = Button ID

            /// <summary>
            /// Set Progress bar to be marquee mode.
            /// </summary>
            TDM_SET_MARQUEE_PROGRESS_BAR = WM_USER + 103, // wParam = 0 (nonMarque) wParam != 0 (Marquee)

            /// <summary>
            /// Set Progress bar state.
            /// </summary>
            TDM_SET_PROGRESS_BAR_STATE = WM_USER + 104, // wParam = new progress state

            /// <summary>
            /// Set progress bar range.
            /// </summary>
            TDM_SET_PROGRESS_BAR_RANGE = WM_USER + 105, // lParam = MAKELPARAM(nMinRange, nMaxRange)

            /// <summary>
            /// Set progress bar position.
            /// </summary>
            TDM_SET_PROGRESS_BAR_POS = WM_USER + 106, // wParam = new position

            /// <summary>
            /// Set progress bar marquee (animation).
            /// </summary>
            TDM_SET_PROGRESS_BAR_MARQUEE = WM_USER + 107, // wParam = 0 (stop marquee), wParam != 0 (start marquee), lparam = speed (milliseconds between repaints)

            /// <summary>
            /// Set a text element of the Task Dialog.
            /// </summary>
            TDM_SET_ELEMENT_TEXT = WM_USER + 108, // wParam = element (TASKDIALOG_ELEMENTS), lParam = new element text (LPCWSTR)

            /// <summary>
            /// Click a radio button.
            /// </summary>
            TDM_CLICK_RADIO_BUTTON = WM_USER + 110, // wParam = Radio Button ID

            /// <summary>
            /// Enable or disable a button.
            /// </summary>
            TDM_ENABLE_BUTTON = WM_USER + 111, // lParam = 0 (disable), lParam != 0 (enable), wParam = Button ID

            /// <summary>
            /// Enable or disable a radio button.
            /// </summary>
            TDM_ENABLE_RADIO_BUTTON = WM_USER + 112, // lParam = 0 (disable), lParam != 0 (enable), wParam = Radio Button ID

            /// <summary>
            /// Check or uncheck the verfication checkbox.
            /// </summary>
            TDM_CLICK_VERIFICATION = WM_USER + 113, // wParam = 0 (unchecked), 1 (checked), lParam = 1 (set key focus)

            /// <summary>
            /// Update the text of an element (no effect if origially set as null).
            /// </summary>
            TDM_UPDATE_ELEMENT_TEXT = WM_USER + 114, // wParam = element (TASKDIALOG_ELEMENTS), lParam = new element text (LPCWSTR)

            /// <summary>
            /// Designate whether a given Task Dialog button or command link should have a User Account Control (UAC) shield icon.
            /// </summary>
            TDM_SET_BUTTON_ELEVATION_REQUIRED_STATE = WM_USER + 115, // wParam = Button ID, lParam = 0 (elevation not required), lParam != 0 (elevation required)

            /// <summary>
            /// Refreshes the icon of the task dialog.
            /// </summary>
            TDM_UPDATE_ICON = WM_USER + 116  // wParam = icon element (TASKDIALOG_ICON_ELEMENTS), lParam = new icon (hIcon if TDF_USE_HICON_* was set, PCWSTR otherwise)
        }

        ///// <summary>
        ///// TaskDialog taken from commctrl.h.
        ///// </summary>
        ///// <param name="hwndParent">Parent window.</param>
        ///// <param name="hInstance">Module instance to get resources from.</param>
        ///// <param name="pszWindowTitle">Title of the Task Dialog window.</param>
        ///// <param name="pszMainInstruction">The main instructions.</param>
        ///// <param name="dwCommonButtons">Common push buttons to show.</param>
        ///// <param name="pszIcon">The main icon.</param>
        ///// <param name="pnButton">The push button pressed.</param>
        ////[DllImport("ComCtl32", CharSet = CharSet.Unicode, PreserveSig = false)]
        ////public static extern void TaskDialog(
        ////    [In] IntPtr hwndParent,
        ////    [In] IntPtr hInstance,
        ////    [In] String pszWindowTitle,
        ////    [In] String pszMainInstruction,
        ////    [In] TaskDialogCommonButtons dwCommonButtons,
        ////    [In] IntPtr pszIcon,
        ////    [Out] out int pnButton);

        /// <summary>
        /// TaskDialogIndirect taken from commctl.h
        /// </summary>
        /// <param name="pTaskConfig">All the parameters about the Task Dialog to Show.</param>
        /// <param name="pnButton">The push button pressed.</param>
        /// <param name="pnRadioButton">The radio button that was selected.</param>
        /// <param name="pfVerificationFlagChecked">The state of the verification checkbox on dismiss of the Task Dialog.</param>
        [DllImport("ComCtl32", CharSet = CharSet.Unicode, PreserveSig = false)]
        internal static extern void TaskDialogIndirect(
            [In] ref TASKDIALOGCONFIG pTaskConfig,
            [Out] out int pnButton,
            [Out] out int pnRadioButton,
            [Out] out bool pfVerificationFlagChecked);

        /// <summary>
        /// Win32 SendMessage.
        /// </summary>
        /// <param name="hWnd">Window handle to send to.</param>
        /// <param name="Msg">The windows message to send.</param>
        /// <param name="wParam">Specifies additional message-specific information.</param>
        /// <param name="lParam">Specifies additional message-specific information.</param>
        /// <returns>The return value specifies the result of the message processing; it depends on the message sent.</returns>
        [DllImport("user32.dll")]
        internal static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        /// <summary>
        /// Win32 SendMessage.
        /// </summary>
        /// <param name="hWnd">Window handle to send to.</param>
        /// <param name="Msg">The windows message to send.</param>
        /// <param name="wParam">Specifies additional message-specific information.</param>
        /// <param name="lParam">Specifies additional message-specific information as a string.</param>
        /// <returns>The return value specifies the result of the message processing; it depends on the message sent.</returns>
        [DllImport("user32.dll", EntryPoint="SendMessage")]
        internal static extern IntPtr SendMessageWithString(IntPtr hWnd, uint Msg, IntPtr wParam, [MarshalAs(UnmanagedType.LPWStr)] string lParam);

        /// <summary>
        /// TASKDIALOGCONFIG taken from commctl.h.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
        internal struct TASKDIALOGCONFIG
        {
            /// <summary>
            /// Size of the structure in bytes.
            /// </summary>
            public uint cbSize;

            /// <summary>
            /// Parent window handle.
            /// </summary>
            [SuppressMessage("Microsoft.Reliability", "CA2006:UseSafeHandleToEncapsulateNativeResources")] // Managed code owns actual resource. Passed to native in syncronous call. No lifetime issues.
            public IntPtr hwndParent;

            /// <summary>
            /// Module instance handle for resources.
            /// </summary>
            [SuppressMessage("Microsoft.Reliability", "CA2006:UseSafeHandleToEncapsulateNativeResources")] // Managed code owns actual resource. Passed to native in syncronous call. No lifetime issues.
            public IntPtr hInstance;

            /// <summary>
            /// Flags.
            /// </summary>
            public TASKDIALOG_FLAGS dwFlags;            // TASKDIALOG_FLAGS (TDF_XXX) flags

            /// <summary>
            /// Bit flags for commonly used buttons.
            /// </summary>
            public VistaTaskDialogCommonButtons dwCommonButtons;    // TASKDIALOG_COMMON_BUTTON (TDCBF_XXX) flags

            /// <summary>
            /// Window title.
            /// </summary>
            [MarshalAs(UnmanagedType.LPWStr)]
            public string pszWindowTitle;                         // string or MAKEINTRESOURCE()

            /// <summary>
            /// The Main icon. Overloaded member. Can be string, a handle, a special value or a resource ID.
            /// </summary>
            [SuppressMessage("Microsoft.Reliability", "CA2006:UseSafeHandleToEncapsulateNativeResources")] // Managed code owns actual resource. Passed to native in syncronous call. No lifetime issues.
            public IntPtr MainIcon;

            /// <summary>
            /// Main Instruction.
            /// </summary>
            [MarshalAs(UnmanagedType.LPWStr)]
            public string pszMainInstruction;

            /// <summary>
            /// Content.
            /// </summary>
            [MarshalAs(UnmanagedType.LPWStr)]
            public string pszContent;

            /// <summary>
            /// Count of custom Buttons.
            /// </summary>
            public uint cButtons;

            /// <summary>
            /// Array of custom buttons.
            /// </summary>
            [SuppressMessage("Microsoft.Reliability", "CA2006:UseSafeHandleToEncapsulateNativeResources")] // Managed code owns actual resource. Passed to native in syncronous call. No lifetime issues.
            public IntPtr pButtons;

            /// <summary>
            /// ID of default button.
            /// </summary>
            public int nDefaultButton;

            /// <summary>
            /// Count of radio Buttons.
            /// </summary>
            public uint cRadioButtons;

            /// <summary>
            /// Array of radio buttons.
            /// </summary>
            [SuppressMessage("Microsoft.Reliability", "CA2006:UseSafeHandleToEncapsulateNativeResources")] // Managed code owns actual resource. Passed to native in syncronous call. No lifetime issues.
            public IntPtr pRadioButtons;

            /// <summary>
            /// ID of default radio button.
            /// </summary>
            public int nDefaultRadioButton;

            /// <summary>
            /// Text for verification check box. often "Don't ask be again".
            /// </summary>
            [MarshalAs(UnmanagedType.LPWStr)]
            public string pszVerificationText;

            /// <summary>
            /// Expanded Information.
            /// </summary>
            [MarshalAs(UnmanagedType.LPWStr)]
            public string pszExpandedInformation;

            /// <summary>
            /// Text for expanded control.
            /// </summary>
            [MarshalAs(UnmanagedType.LPWStr)]
            public string pszExpandedControlText;

            /// <summary>
            /// Text for expanded control.
            /// </summary>
            [MarshalAs(UnmanagedType.LPWStr)]
            public string pszCollapsedControlText;

            /// <summary>
            /// Icon for the footer. An overloaded member link MainIcon.
            /// </summary>
            [SuppressMessage("Microsoft.Reliability", "CA2006:UseSafeHandleToEncapsulateNativeResources")] // Managed code owns actual resource. Passed to native in syncronous call. No lifetime issues.
            public IntPtr FooterIcon;

            /// <summary>
            /// Footer Text.
            /// </summary>
            [MarshalAs(UnmanagedType.LPWStr)]
            public string pszFooter;

            /// <summary>
            /// Function pointer for callback.
            /// </summary>
            public VistaTaskDialogCallback pfCallback;

            /// <summary>
            /// Data that will be passed to the call back.
            /// </summary>
            [SuppressMessage("Microsoft.Reliability", "CA2006:UseSafeHandleToEncapsulateNativeResources")] // Managed code owns actual resource. Passed to native in syncronous call. No lifetime issues.
            public IntPtr lpCallbackData;

            /// <summary>
            /// Width of the Task Dialog's area in DLU's.
            /// </summary>
            public uint cxWidth;                                // width of the Task Dialog's client area in DLU's. If 0, Task Dialog will calculate the ideal width.
        }
    }
}
