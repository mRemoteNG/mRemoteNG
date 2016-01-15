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
  /// The signature of the callback that recieves notificaitons from the Task Dialog.
  /// </summary>
  /// <param name="taskDialog">The active task dialog which has methods that can be performed on an active Task Dialog.</param>
  /// <param name="args">The notification arguments including the type of notification and information for the notification.</param>
  /// <param name="callbackData">The value set on TaskDialog.CallbackData</param>
  /// <returns>Return value meaning varies depending on the Notification member of args.</returns>
  public delegate bool VistaTaskDialogCallback(VistaActiveTaskDialog taskDialog, VistaTaskDialogNotificationArgs args, object callbackData);

  /// <summary>
  /// The TaskDialog common button flags used to specify the builtin bottons to show in the TaskDialog.
  /// </summary>
  [Flags]
  public enum VistaTaskDialogCommonButtons
  {
    /// <summary>
    /// No common buttons.
    /// </summary>
    None = 0,

    /// <summary>
    /// OK common button. If selected Task Dialog will return DialogResult.OK.
    /// </summary>
    Ok = 0x0001,

    /// <summary>
    /// Yes common button. If selected Task Dialog will return DialogResult.Yes.
    /// </summary>
    Yes = 0x0002,

    /// <summary>
    /// No common button. If selected Task Dialog will return DialogResult.No.
    /// </summary>
    No = 0x0004,

    /// <summary>
    /// Cancel common button. If selected Task Dialog will return DialogResult.Cancel.
    /// If this button is specified, the dialog box will respond to typical cancel actions (Alt-F4 and Escape).
    /// </summary>
    Cancel = 0x0008,

    /// <summary>
    /// Retry common button. If selected Task Dialog will return DialogResult.Retry.
    /// </summary>
    Retry = 0x0010,

    /// <summary>
    /// Close common button. If selected Task Dialog will return this value.
    /// </summary>
    Close = 0x0020,
  }

  /// <summary>
  /// The System icons the TaskDialog supports.
  /// </summary>
  [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")] // Type comes from CommCtrl.h
  public enum VistaTaskDialogIcon : uint
  {
    /// <summary>
    /// No Icon.
    /// </summary>
    None = 0,

    /// <summary>
    /// System warning icon.
    /// </summary>
    Warning = 0xFFFF, // MAKEINTRESOURCEW(-1)

    /// <summary>
    /// System Error icon.
    /// </summary>
    Error = 0xFFFE, // MAKEINTRESOURCEW(-2)

    /// <summary>
    /// System Information icon.
    /// </summary>
    Information = 0xFFFD, // MAKEINTRESOURCEW(-3)

    /// <summary>
    /// Shield icon.
    /// </summary>
    Shield = 0xFFFC, // MAKEINTRESOURCEW(-4)
  }

  /// <summary>
  /// Task Dialog callback notifications. 
  /// </summary>
  public enum VistaTaskDialogNotification
  {
    /// <summary>
    /// Sent by the Task Dialog once the dialog has been created and before it is displayed.
    /// The value returned by the callback is ignored.
    /// </summary>
    Created = 0,

    //// Spec is not clear what this is so not supporting it.
    ///// <summary>
    ///// Sent by the Task Dialog when a navigation has occurred.
    ///// The value returned by the callback is ignored.
    ///// </summary>   
    // Navigated = 1,

    /// <summary>
    /// Sent by the Task Dialog when the user selects a button or command link in the task dialog.
    /// The button ID corresponding to the button selected will be available in the
    /// TaskDialogNotificationArgs. To prevent the Task Dialog from closing, the application must
    /// return true, otherwise the Task Dialog will be closed and the button ID returned to via
    /// the original application call.
    /// </summary>
    ButtonClicked = 2,            // wParam = Button ID

    /// <summary>
    /// Sent by the Task Dialog when the user clicks on a hyperlink in the Task Dialog’s content.
    /// The string containing the HREF of the hyperlink will be available in the
    /// TaskDialogNotificationArgs. To prevent the TaskDialog from shell executing the hyperlink,
    /// the application must return TRUE, otherwise ShellExecute will be called.
    /// </summary>
    HyperlinkClicked = 3,            // lParam = (LPCWSTR)pszHREF

    /// <summary>
    /// Sent by the Task Dialog approximately every 200 milliseconds when TaskDialog.CallbackTimer
    /// has been set to true. The number of milliseconds since the dialog was created or the
    /// notification returned true is available on the TaskDialogNotificationArgs. To reset
    /// the tickcount, the application must return true, otherwise the tickcount will continue to
    /// increment.
    /// </summary>
    Timer = 4,            // wParam = Milliseconds since dialog created or timer reset

    /// <summary>
    /// Sent by the Task Dialog when it is destroyed and its window handle no longer valid.
    /// The value returned by the callback is ignored.
    /// </summary>
    Destroyed = 5,

    /// <summary>
    /// Sent by the Task Dialog when the user selects a radio button in the task dialog.
    /// The button ID corresponding to the button selected will be available in the
    /// TaskDialogNotificationArgs.
    /// The value returned by the callback is ignored.
    /// </summary>
    RadioButtonClicked = 6,            // wParam = Radio Button ID

    /// <summary>
    /// Sent by the Task Dialog once the dialog has been constructed and before it is displayed.
    /// The value returned by the callback is ignored.
    /// </summary>
    DialogConstructed = 7,

    /// <summary>
    /// Sent by the Task Dialog when the user checks or unchecks the verification checkbox.
    /// The verificationFlagChecked value is available on the TaskDialogNotificationArgs.
    /// The value returned by the callback is ignored.
    /// </summary>
    VerificationClicked = 8,             // wParam = 1 if checkbox checked, 0 if not, lParam is unused and always 0

    /// <summary>
    /// Sent by the Task Dialog when the user presses F1 on the keyboard while the dialog has focus.
    /// The value returned by the callback is ignored.
    /// </summary>
    Help = 9,

    /// <summary>
    /// Sent by the task dialog when the user clicks on the dialog's expando button.
    /// The expanded value is available on the TaskDialogNotificationArgs.
    /// The value returned by the callback is ignored.
    /// </summary>
    ExpandoButtonClicked = 10            // wParam = 0 (dialog is now collapsed), wParam != 0 (dialog is now expanded)
  }

  /// <summary>
  /// Progress bar state.
  /// </summary>
  [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")] // Comes from CommCtrl.h PBST_* values which don't have a zero.
  public enum VistaProgressBarState
  {
    /// <summary>
    /// Normal.
    /// </summary>
    Normal = 1,

    /// <summary>
    /// Error state.
    /// </summary>
    Error = 2,

    /// <summary>
    /// Paused state.
    /// </summary>
    Paused = 3
  }

  /// <summary>
  /// A custom button for the TaskDialog.
  /// </summary>
  [SuppressMessage("Microsoft.Performance", "CA1815:OverrideEqualsAndOperatorEqualsOnValueTypes")] // Would be unused code as not required for usage.
  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
  public struct VistaTaskDialogButton
  {
    /// <summary>
    /// The ID of the button. This value is returned by TaskDialog.Show when the button is clicked.
    /// </summary>
    private int buttonId;

    /// <summary>
    /// The string that appears on the button.
    /// </summary>
    [MarshalAs(UnmanagedType.LPWStr)]
    private string buttonText;

    /// <summary>
    /// Initialize the custom button.
    /// </summary>
    /// <param name="id">The ID of the button. This value is returned by TaskDialog.Show when
    /// the button is clicked. Typically this will be a value in the DialogResult enum.</param>
    /// <param name="text">The string that appears on the button.</param>
    public VistaTaskDialogButton(int id, string text)
    {
      this.buttonId = id;
      this.buttonText = text;
    }

    /// <summary>
    /// The ID of the button. This value is returned by TaskDialog.Show when the button is clicked.
    /// </summary>
    public int ButtonId
    {
      get { return this.buttonId; }
      set { this.buttonId = value; }
    }

    /// <summary>
    /// The string that appears on the button.
    /// </summary>
    public string ButtonText
    {
      get { return this.buttonText; }
      set { this.buttonText = value; }
    }
  }

  /// <summary>
  /// A Task Dialog. This is like a MessageBox but with many more features. TaskDialog requires Windows Longhorn or later.
  /// </summary>
  public class VistaTaskDialog
  {
    /// <summary>
    /// The string to be used for the dialog box title. If this parameter is NULL, the filename of the executable program is used.
    /// </summary>
    private string windowTitle;

    /// <summary>
    /// The string to be used for the main instruction.
    /// </summary>
    private string mainInstruction;

    /// <summary>
    /// The string to be used for the dialog’s primary content. If the EnableHyperlinks member is true,
    /// then this string may contain hyperlinks in the form: <A HREF="executablestring">Hyperlink Text</A>. 
    /// WARNING: Enabling hyperlinks when using content from an unsafe source may cause security vulnerabilities.
    /// </summary>
    private string content;

    /// <summary>
    /// Specifies the push buttons displayed in the dialog box.  This parameter may be a combination of flags.
    /// If no common buttons are specified and no custom buttons are specified using the Buttons member, the
    /// dialog box will contain the OK button by default.
    /// </summary>
    private VistaTaskDialogCommonButtons commonButtons;

    /// <summary>
    /// Specifies a built in icon for the main icon in the dialog. If this is set to none
    /// and the CustomMainIcon is null then no main icon will be displayed.
    /// </summary>
    private VistaTaskDialogIcon mainIcon;

    /// <summary>
    /// Specifies a custom in icon for the main icon in the dialog. If this is set to none
    /// and the CustomMainIcon member is null then no main icon will be displayed.
    /// </summary>
    private Icon customMainIcon;

    /// <summary>
    /// Specifies a built in icon for the icon to be displayed in the footer area of the
    /// dialog box. If this is set to none and the CustomFooterIcon member is null then no
    /// footer icon will be displayed.
    /// </summary>
    private VistaTaskDialogIcon footerIcon;

    /// <summary>
    /// Specifies a custom icon for the icon to be displayed in the footer area of the
    /// dialog box. If this is set to none and the CustomFooterIcon member is null then no
    /// footer icon will be displayed.
    /// </summary>
    private Icon customFooterIcon;

    /// <summary>
    /// Specifies the custom push buttons to display in the dialog. Use CommonButtons member for
    /// common buttons; OK, Yes, No, Retry and Cancel, and Buttons when you want different text
    /// on the push buttons.
    /// </summary>
    private VistaTaskDialogButton[] buttons;

    /// <summary>
    /// Specifies the radio buttons to display in the dialog.
    /// </summary>
    private VistaTaskDialogButton[] radioButtons;

    /// <summary>
    /// The flags passed to TaskDialogIndirect.
    /// </summary>
    private VistaUnsafeNativeMethods.TASKDIALOG_FLAGS flags;

    /// <summary>
    /// Indicates the default button for the dialog. This may be any of the values specified
    /// in ButtonId members of one of the TaskDialogButton structures in the Buttons array,
    /// or one a DialogResult value that corresponds to a buttons specified in the CommonButtons Member.
    /// If this member is zero or its value does not correspond to any button ID in the dialog,
    /// then the first button in the dialog will be the default. 
    /// </summary>
    private int defaultButton;

    /// <summary>
    /// Indicates the default radio button for the dialog. This may be any of the values specified
    /// in ButtonId members of one of the TaskDialogButton structures in the RadioButtons array.
    /// If this member is zero or its value does not correspond to any radio button ID in the dialog,
    /// then the first button in RadioButtons will be the default.
    /// The property NoDefaultRadioButton can be set to have no default.
    /// </summary>
    private int defaultRadioButton;

    /// <summary>
    /// The string to be used to label the verification checkbox. If this member is null, the
    /// verification checkbox is not displayed in the dialog box.
    /// </summary>
    private string verificationText;

    /// <summary>
    /// The string to be used for displaying additional information. The additional information is
    /// displayed either immediately below the content or below the footer text depending on whether
    /// the ExpandFooterArea member is true. If the EnableHyperlinks member is true, then this string
    /// may contain hyperlinks in the form: <A HREF="executablestring">Hyperlink Text</A>.
    /// WARNING: Enabling hyperlinks when using content from an unsafe source may cause security vulnerabilities.
    /// </summary>
    private string expandedInformation;

    /// <summary>
    /// The string to be used to label the button for collapsing the expanded information. This
    /// member is ignored when the ExpandedInformation member is null. If this member is null
    /// and the CollapsedControlText is specified, then the CollapsedControlText value will be
    /// used for this member as well.
    /// </summary>
    private string expandedControlText;

    /// <summary>
    /// The string to be used to label the button for expanding the expanded information. This
    /// member is ignored when the ExpandedInformation member is null.  If this member is null
    /// and the ExpandedControlText is specified, then the ExpandedControlText value will be
    /// used for this member as well.
    /// </summary>
    private string collapsedControlText;

    /// <summary>
    /// The string to be used in the footer area of the dialog box. If the EnableHyperlinks member
    /// is true, then this string may contain hyperlinks in the form: <A HREF="executablestring">
    /// Hyperlink Text</A>.
    /// WARNING: Enabling hyperlinks when using content from an unsafe source may cause security vulnerabilities.
    /// </summary>
    private string footer;

    /// <summary>
    /// The callback that receives messages from the Task Dialog when various events occur.
    /// </summary>
    private VistaTaskDialogCallback callback;

    /// <summary>
    /// Reference that is passed to the callback.
    /// </summary>
    private object callbackData;

    /// <summary>
    /// Specifies the width of the Task Dialog’s client area in DLU’s. If 0, Task Dialog will calculate the ideal width.
    /// </summary>
    private uint width;

    /// <summary>
    /// Creates a default Task Dialog.
    /// </summary>
    public VistaTaskDialog()
    {
      this.Reset();
    }

    /// <summary>
    /// Returns true if the current operating system supports TaskDialog. If false TaskDialog.Show should not
    /// be called as the results are undefined but often results in a crash.
    /// </summary>
    public static bool IsAvailableOnThisOS
    {
      get
      {
        OperatingSystem os = Environment.OSVersion;
        if (os.Platform != PlatformID.Win32NT)
          return false;
        return (os.Version.CompareTo(VistaTaskDialog.RequiredOSVersion) >= 0);
      }
    }

    /// <summary>
    /// The minimum Windows version needed to support TaskDialog.
    /// </summary>
    public static Version RequiredOSVersion
    {
      get { return new Version(6, 0, 5243); }
    }

    /// <summary>
    /// The string to be used for the dialog box title. If this parameter is NULL, the filename of the executable program is used.
    /// </summary>
    public string WindowTitle
    {
      get { return this.windowTitle; }
      set { this.windowTitle = value; }
    }

    /// <summary>
    /// The string to be used for the main instruction.
    /// </summary>
    public string MainInstruction
    {
      get { return this.mainInstruction; }
      set { this.mainInstruction = value; }
    }

    /// <summary>
    /// The string to be used for the dialog’s primary content. If the EnableHyperlinks member is true,
    /// then this string may contain hyperlinks in the form: <A HREF="executablestring">Hyperlink Text</A>. 
    /// WARNING: Enabling hyperlinks when using content from an unsafe source may cause security vulnerabilities.
    /// </summary>
    public string Content
    {
      get { return this.content; }
      set { this.content = value; }
    }

    /// <summary>
    /// Specifies the push buttons displayed in the dialog box. This parameter may be a combination of flags.
    /// If no common buttons are specified and no custom buttons are specified using the Buttons member, the
    /// dialog box will contain the OK button by default.
    /// </summary>
    public VistaTaskDialogCommonButtons CommonButtons
    {
      get { return this.commonButtons; }
      set { this.commonButtons = value; }
    }

    /// <summary>
    /// Specifies a built in icon for the main icon in the dialog. If this is set to none
    /// and the CustomMainIcon is null then no main icon will be displayed.
    /// </summary>
    public VistaTaskDialogIcon MainIcon
    {
      get { return this.mainIcon; }
      set { this.mainIcon = value; }
    }

    /// <summary>
    /// Specifies a custom in icon for the main icon in the dialog. If this is set to none
    /// and the CustomMainIcon member is null then no main icon will be displayed.
    /// </summary>
    public Icon CustomMainIcon
    {
      get { return this.customMainIcon; }
      set { this.customMainIcon = value; }
    }

    /// <summary>
    /// Specifies a built in icon for the icon to be displayed in the footer area of the
    /// dialog box. If this is set to none and the CustomFooterIcon member is null then no
    /// footer icon will be displayed.
    /// </summary>
    public VistaTaskDialogIcon FooterIcon
    {
      get { return this.footerIcon; }
      set { this.footerIcon = value; }
    }

    /// <summary>
    /// Specifies a custom icon for the icon to be displayed in the footer area of the
    /// dialog box. If this is set to none and the CustomFooterIcon member is null then no
    /// footer icon will be displayed.
    /// </summary>
    public Icon CustomFooterIcon
    {
      get { return this.customFooterIcon; }
      set { this.customFooterIcon = value; }
    }

    /// <summary>
    /// Specifies the custom push buttons to display in the dialog. Use CommonButtons member for
    /// common buttons; OK, Yes, No, Retry and Cancel, and Buttons when you want different text
    /// on the push buttons.
    /// </summary>
    [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")] // Style of use is like single value. Array is of value types.
    [SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")] // Returns a reference, not a copy.
    public VistaTaskDialogButton[] Buttons
    {
      get
      {
        return this.buttons;
      }

      set
      {
        if (value == null)
        {
          throw new ArgumentNullException("value");
        }

        this.buttons = value;
      }
    }

    /// <summary>
    /// Specifies the radio buttons to display in the dialog.
    /// </summary>
    [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")] // Style of use is like single value. Array is of value types.
    [SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")] // Returns a reference, not a copy.
    public VistaTaskDialogButton[] RadioButtons
    {
      get
      {
        return this.radioButtons;
      }

      set
      {
        if (value == null)
        {
          throw new ArgumentNullException("value");
        }

        this.radioButtons = value;
      }
    }

    /// <summary>
    /// Enables hyperlink processing for the strings specified in the Content, ExpandedInformation
    /// and FooterText members. When enabled, these members may be strings that contain hyperlinks
    /// in the form: <A HREF="executablestring">Hyperlink Text</A>. 
    /// WARNING: Enabling hyperlinks when using content from an unsafe source may cause security vulnerabilities.
    /// Note: Task Dialog will not actually execute any hyperlinks. Hyperlink execution must be handled
    /// in the callback function specified by Callback member.
    /// </summary>
    public bool EnableHyperlinks
    {
      get { return (this.flags & VistaUnsafeNativeMethods.TASKDIALOG_FLAGS.TDF_ENABLE_HYPERLINKS) != 0; }
      set { this.SetFlag(VistaUnsafeNativeMethods.TASKDIALOG_FLAGS.TDF_ENABLE_HYPERLINKS, value); }
    }

    /// <summary>
    /// Indicates that the dialog should be able to be closed using Alt-F4, Escape and the title bar’s
    /// close button even if no cancel button is specified in either the CommonButtons or Buttons members.
    /// </summary>
    public bool AllowDialogCancellation
    {
      get { return (this.flags & VistaUnsafeNativeMethods.TASKDIALOG_FLAGS.TDF_ALLOW_DIALOG_CANCELLATION) != 0; }
      set { this.SetFlag(VistaUnsafeNativeMethods.TASKDIALOG_FLAGS.TDF_ALLOW_DIALOG_CANCELLATION, value); }
    }

    /// <summary>
    /// Indicates that the buttons specified in the Buttons member should be displayed as command links
    /// (using a standard task dialog glyph) instead of push buttons.  When using command links, all
    /// characters up to the first new line character in the ButtonText member (of the TaskDialogButton
    /// structure) will be treated as the command link’s main text, and the remainder will be treated
    /// as the command link’s note. This flag is ignored if the Buttons member has no entires.
    /// </summary>
    public bool UseCommandLinks
    {
      get { return (this.flags & VistaUnsafeNativeMethods.TASKDIALOG_FLAGS.TDF_USE_COMMAND_LINKS) != 0; }
      set { this.SetFlag(VistaUnsafeNativeMethods.TASKDIALOG_FLAGS.TDF_USE_COMMAND_LINKS, value); }
    }

    /// <summary>
    /// Indicates that the buttons specified in the Buttons member should be displayed as command links
    /// (without a glyph) instead of push buttons. When using command links, all characters up to the
    /// first new line character in the ButtonText member (of the TaskDialogButton structure) will be
    /// treated as the command link’s main text, and the remainder will be treated as the command link’s
    /// note. This flag is ignored if the Buttons member has no entires.
    /// </summary>
    public bool UseCommandLinksNoIcon
    {
      get { return (this.flags & VistaUnsafeNativeMethods.TASKDIALOG_FLAGS.TDF_USE_COMMAND_LINKS_NO_ICON) != 0; }
      set { this.SetFlag(VistaUnsafeNativeMethods.TASKDIALOG_FLAGS.TDF_USE_COMMAND_LINKS_NO_ICON, value); }
    }

    /// <summary>
    /// Indicates that the string specified by the ExpandedInformation member should be displayed at the
    /// bottom of the dialog’s footer area instead of immediately after the dialog’s content. This flag
    /// is ignored if the ExpandedInformation member is null.
    /// </summary>
    public bool ExpandFooterArea
    {
      get { return (this.flags & VistaUnsafeNativeMethods.TASKDIALOG_FLAGS.TDF_EXPAND_FOOTER_AREA) != 0; }
      set { this.SetFlag(VistaUnsafeNativeMethods.TASKDIALOG_FLAGS.TDF_EXPAND_FOOTER_AREA, value); }
    }

    /// <summary>
    /// Indicates that the string specified by the ExpandedInformation member should be displayed
    /// when the dialog is initially displayed. This flag is ignored if the ExpandedInformation member
    /// is null.
    /// </summary>
    public bool ExpandedByDefault
    {
      get { return (this.flags & VistaUnsafeNativeMethods.TASKDIALOG_FLAGS.TDF_EXPANDED_BY_DEFAULT) != 0; }
      set { this.SetFlag(VistaUnsafeNativeMethods.TASKDIALOG_FLAGS.TDF_EXPANDED_BY_DEFAULT, value); }
    }

    /// <summary>
    /// Indicates that the verification checkbox in the dialog should be checked when the dialog is
    /// initially displayed. This flag is ignored if the VerificationText parameter is null.
    /// </summary>
    public bool VerificationFlagChecked
    {
      get { return (this.flags & VistaUnsafeNativeMethods.TASKDIALOG_FLAGS.TDF_VERIFICATION_FLAG_CHECKED) != 0; }
      set { this.SetFlag(VistaUnsafeNativeMethods.TASKDIALOG_FLAGS.TDF_VERIFICATION_FLAG_CHECKED, value); }
    }

    /// <summary>
    /// Indicates that a Progress Bar should be displayed.
    /// </summary>
    public bool ShowProgressBar
    {
      get { return (this.flags & VistaUnsafeNativeMethods.TASKDIALOG_FLAGS.TDF_SHOW_PROGRESS_BAR) != 0; }
      set { this.SetFlag(VistaUnsafeNativeMethods.TASKDIALOG_FLAGS.TDF_SHOW_PROGRESS_BAR, value); }
    }

    /// <summary>
    /// Indicates that an Marquee Progress Bar should be displayed.
    /// </summary>
    public bool ShowMarqueeProgressBar
    {
      get { return (this.flags & VistaUnsafeNativeMethods.TASKDIALOG_FLAGS.TDF_SHOW_MARQUEE_PROGRESS_BAR) != 0; }
      set { this.SetFlag(VistaUnsafeNativeMethods.TASKDIALOG_FLAGS.TDF_SHOW_MARQUEE_PROGRESS_BAR, value); }
    }

    /// <summary>
    /// Indicates that the TaskDialog’s callback should be called approximately every 200 milliseconds.
    /// </summary>
    public bool CallbackTimer
    {
      get { return (this.flags & VistaUnsafeNativeMethods.TASKDIALOG_FLAGS.TDF_CALLBACK_TIMER) != 0; }
      set { this.SetFlag(VistaUnsafeNativeMethods.TASKDIALOG_FLAGS.TDF_CALLBACK_TIMER, value); }
    }

    /// <summary>
    /// Indicates that the TaskDialog should be positioned (centered) relative to the owner window
    /// passed when calling Show. If not set (or no owner window is passed), the TaskDialog is
    /// positioned (centered) relative to the monitor.
    /// </summary>
    public bool PositionRelativeToWindow
    {
      get { return (this.flags & VistaUnsafeNativeMethods.TASKDIALOG_FLAGS.TDF_POSITION_RELATIVE_TO_WINDOW) != 0; }
      set { this.SetFlag(VistaUnsafeNativeMethods.TASKDIALOG_FLAGS.TDF_POSITION_RELATIVE_TO_WINDOW, value); }
    }

    /// <summary>
    /// Indicates that the TaskDialog should have right to left layout.
    /// </summary>
    public bool RightToLeftLayout
    {
      get { return (this.flags & VistaUnsafeNativeMethods.TASKDIALOG_FLAGS.TDF_RTL_LAYOUT) != 0; }
      set { this.SetFlag(VistaUnsafeNativeMethods.TASKDIALOG_FLAGS.TDF_RTL_LAYOUT, value); }
    }

    /// <summary>
    /// Indicates that the TaskDialog should have no default radio button.
    /// </summary>
    public bool NoDefaultRadioButton
    {
      get { return (this.flags & VistaUnsafeNativeMethods.TASKDIALOG_FLAGS.TDF_NO_DEFAULT_RADIO_BUTTON) != 0; }
      set { this.SetFlag(VistaUnsafeNativeMethods.TASKDIALOG_FLAGS.TDF_NO_DEFAULT_RADIO_BUTTON, value); }
    }

    /// <summary>
    /// Indicates that the TaskDialog can be minimised. Works only if there if parent window is null. Will enable cancellation also.
    /// </summary>
    public bool CanBeMinimized
    {
      get { return (this.flags & VistaUnsafeNativeMethods.TASKDIALOG_FLAGS.TDF_CAN_BE_MINIMIZED) != 0; }
      set { this.SetFlag(VistaUnsafeNativeMethods.TASKDIALOG_FLAGS.TDF_CAN_BE_MINIMIZED, value); }
    }

    /// <summary>
    /// Indicates the default button for the dialog. This may be any of the values specified
    /// in ButtonId members of one of the TaskDialogButton structures in the Buttons array,
    /// or one a DialogResult value that corresponds to a buttons specified in the CommonButtons Member.
    /// If this member is zero or its value does not correspond to any button ID in the dialog,
    /// then the first button in the dialog will be the default. 
    /// </summary>
    public int DefaultButton
    {
      get { return this.defaultButton; }
      set { this.defaultButton = value; }
    }

    /// <summary>
    /// Indicates the default radio button for the dialog. This may be any of the values specified
    /// in ButtonId members of one of the TaskDialogButton structures in the RadioButtons array.
    /// If this member is zero or its value does not correspond to any radio button ID in the dialog,
    /// then the first button in RadioButtons will be the default.
    /// The property NoDefaultRadioButton can be set to have no default.
    /// </summary>
    public int DefaultRadioButton
    {
      get { return this.defaultRadioButton; }
      set { this.defaultRadioButton = value; }
    }

    /// <summary>
    /// The string to be used to label the verification checkbox. If this member is null, the
    /// verification checkbox is not displayed in the dialog box.
    /// </summary>
    public string VerificationText
    {
      get { return this.verificationText; }
      set { this.verificationText = value; }
    }

    /// <summary>
    /// The string to be used for displaying additional information. The additional information is
    /// displayed either immediately below the content or below the footer text depending on whether
    /// the ExpandFooterArea member is true. If the EnameHyperlinks member is true, then this string
    /// may contain hyperlinks in the form: <A HREF="executablestring">Hyperlink Text</A>.
    /// WARNING: Enabling hyperlinks when using content from an unsafe source may cause security vulnerabilities.
    /// </summary>
    public string ExpandedInformation
    {
      get { return this.expandedInformation; }
      set { this.expandedInformation = value; }
    }

    /// <summary>
    /// The string to be used to label the button for collapsing the expanded information. This
    /// member is ignored when the ExpandedInformation member is null. If this member is null
    /// and the CollapsedControlText is specified, then the CollapsedControlText value will be
    /// used for this member as well.
    /// </summary>
    public string ExpandedControlText
    {
      get { return this.expandedControlText; }
      set { this.expandedControlText = value; }
    }

    /// <summary>
    /// The string to be used to label the button for expanding the expanded information. This
    /// member is ignored when the ExpandedInformation member is null.  If this member is null
    /// and the ExpandedControlText is specified, then the ExpandedControlText value will be
    /// used for this member as well.
    /// </summary>
    public string CollapsedControlText
    {
      get { return this.collapsedControlText; }
      set { this.collapsedControlText = value; }
    }

    /// <summary>
    /// The string to be used in the footer area of the dialog box. If the EnableHyperlinks member
    /// is true, then this string may contain hyperlinks in the form: <A HREF="executablestring">
    /// Hyperlink Text</A>.
    /// WARNING: Enabling hyperlinks when using content from an unsafe source may cause security vulnerabilities.
    /// </summary>
    public string Footer
    {
      get { return this.footer; }
      set { this.footer = value; }
    }

    /// <summary>
    /// width of the Task Dialog's client area in DLU's. If 0, Task Dialog will calculate the ideal width.
    /// </summary>
    public uint Width
    {
      get { return this.width; }
      set { this.width = value; }
    }

    /// <summary>
    /// The callback that receives messages from the Task Dialog when various events occur.
    /// </summary>
    public VistaTaskDialogCallback Callback
    {
      get { return this.callback; }
      set { this.callback = value; }
    }

    /// <summary>
    /// Reference that is passed to the callback.
    /// </summary>
    public object CallbackData
    {
      get { return this.callbackData; }
      set { this.callbackData = value; }
    }

    /// <summary>
    /// Resets the Task Dialog to the state when first constructed, all properties set to their default value.
    /// </summary>
    public void Reset()
    {
      this.windowTitle = null;
      this.mainInstruction = null;
      this.content = null;
      this.commonButtons = 0;
      this.mainIcon = VistaTaskDialogIcon.None;
      this.customMainIcon = null;
      this.footerIcon = VistaTaskDialogIcon.None;
      this.customFooterIcon = null;
      this.buttons = new VistaTaskDialogButton[0];
      this.radioButtons = new VistaTaskDialogButton[0];
      this.flags = 0;
      this.defaultButton = 0;
      this.defaultRadioButton = 0;
      this.verificationText = null;
      this.expandedInformation = null;
      this.expandedControlText = null;
      this.collapsedControlText = null;
      this.footer = null;
      this.callback = null;
      this.callbackData = null;
      this.width = 0;
    }

    /// <summary>
    /// Creates, displays, and operates a task dialog. The task dialog contains application-defined messages, title,
    /// verification check box, command links and push buttons, plus any combination of predefined icons and push buttons
    /// as specified on the other members of the class before calling Show.
    /// </summary>
    /// <returns>The result of the dialog, either a DialogResult value for common push buttons set in the CommonButtons
    /// member or the ButtonID from a TaskDialogButton structure set on the Buttons member.</returns>
    public int Show()
    {
      bool verificationFlagChecked;
      int radioButtonResult;
      return this.Show(IntPtr.Zero, out verificationFlagChecked, out radioButtonResult);
    }

    /// <summary>
    /// Creates, displays, and operates a task dialog. The task dialog contains application-defined messages, title,
    /// verification check box, command links and push buttons, plus any combination of predefined icons and push buttons
    /// as specified on the other members of the class before calling Show.
    /// </summary>
    /// <param name="owner">Owner window the task Dialog will modal to.</param>
    /// <returns>The result of the dialog, either a DialogResult value for common push buttons set in the CommonButtons
    /// member or the ButtonID from a TaskDialogButton structure set on the Buttons member.</returns>
    public int Show(IWin32Window owner)
    {
      bool verificationFlagChecked;
      int radioButtonResult;
      return this.Show((owner == null ? IntPtr.Zero : owner.Handle), out verificationFlagChecked, out radioButtonResult);
    }

    /// <summary>
    /// Creates, displays, and operates a task dialog. The task dialog contains application-defined messages, title,
    /// verification check box, command links and push buttons, plus any combination of predefined icons and push buttons
    /// as specified on the other members of the class before calling Show.
    /// </summary>
    /// <param name="hwndOwner">Owner window the task Dialog will modal to.</param>
    /// <returns>The result of the dialog, either a DialogResult value for common push buttons set in the CommonButtons
    /// member or the ButtonID from a TaskDialogButton structure set on the Buttons member.</returns>
    public int Show(IntPtr hwndOwner)
    {
      bool verificationFlagChecked;
      int radioButtonResult;
      return this.Show(hwndOwner, out verificationFlagChecked, out radioButtonResult);
    }

    /// <summary>
    /// Creates, displays, and operates a task dialog. The task dialog contains application-defined messages, title,
    /// verification check box, command links and push buttons, plus any combination of predefined icons and push buttons
    /// as specified on the other members of the class before calling Show.
    /// </summary>
    /// <param name="owner">Owner window the task Dialog will modal to.</param>
    /// <param name="verificationFlagChecked">Returns true if the verification checkbox was checked when the dialog
    /// was dismissed.</param>
    /// <returns>The result of the dialog, either a DialogResult value for common push buttons set in the CommonButtons
    /// member or the ButtonID from a TaskDialogButton structure set on the Buttons member.</returns>
    public int Show(IWin32Window owner, out bool verificationFlagChecked)
    {
      int radioButtonResult;
      return this.Show((owner == null ? IntPtr.Zero : owner.Handle), out verificationFlagChecked, out radioButtonResult);
    }

    /// <summary>
    /// Creates, displays, and operates a task dialog. The task dialog contains application-defined messages, title,
    /// verification check box, command links and push buttons, plus any combination of predefined icons and push buttons
    /// as specified on the other members of the class before calling Show.
    /// </summary>
    /// <param name="hwndOwner">Owner window the task Dialog will modal to.</param>
    /// <param name="verificationFlagChecked">Returns true if the verification checkbox was checked when the dialog
    /// was dismissed.</param>
    /// <returns>The result of the dialog, either a DialogResult value for common push buttons set in the CommonButtons
    /// member or the ButtonID from a TaskDialogButton structure set on the Buttons member.</returns>
    public int Show(IntPtr hwndOwner, out bool verificationFlagChecked)
    {
      // We have to call a private version or PreSharp gets upset about a unsafe
      // block in a public method. (PreSharp error 56505)
      int radioButtonResult;
      return this.PrivateShow(hwndOwner, out verificationFlagChecked, out radioButtonResult);
    }

    /// <summary>
    /// Creates, displays, and operates a task dialog. The task dialog contains application-defined messages, title,
    /// verification check box, command links and push buttons, plus any combination of predefined icons and push buttons
    /// as specified on the other members of the class before calling Show.
    /// </summary>
    /// <param name="owner">Owner window the task Dialog will modal to.</param>
    /// <param name="verificationFlagChecked">Returns true if the verification checkbox was checked when the dialog
    /// was dismissed.</param>
    /// <param name="radioButtonResult">The radio botton selected by the user.</param>
    /// <returns>The result of the dialog, either a DialogResult value for common push buttons set in the CommonButtons
    /// member or the ButtonID from a TaskDialogButton structure set on the Buttons member.</returns>
    public int Show(IWin32Window owner, out bool verificationFlagChecked, out int radioButtonResult)
    {
      return this.Show((owner == null ? IntPtr.Zero : owner.Handle), out verificationFlagChecked, out radioButtonResult);
    }

    /// <summary>
    /// Creates, displays, and operates a task dialog. The task dialog contains application-defined messages, title,
    /// verification check box, command links and push buttons, plus any combination of predefined icons and push buttons
    /// as specified on the other members of the class before calling Show.
    /// </summary>
    /// <param name="hwndOwner">Owner window the task Dialog will modal to.</param>
    /// <param name="verificationFlagChecked">Returns true if the verification checkbox was checked when the dialog
    /// was dismissed.</param>
    /// <param name="radioButtonResult">The radio botton selected by the user.</param>
    /// <returns>The result of the dialog, either a DialogResult value for common push buttons set in the CommonButtons
    /// member or the ButtonID from a TaskDialogButton structure set on the Buttons member.</returns>
    public int Show(IntPtr hwndOwner, out bool verificationFlagChecked, out int radioButtonResult)
    {
      // We have to call a private version or PreSharp gets upset about a unsafe
      // block in a public method. (PreSharp error 56505)
      return this.PrivateShow(hwndOwner, out verificationFlagChecked, out radioButtonResult);
    }

    /// <summary>
    /// Creates, displays, and operates a task dialog. The task dialog contains application-defined messages, title,
    /// verification check box, command links and push buttons, plus any combination of predefined icons and push buttons
    /// as specified on the other members of the class before calling Show.
    /// </summary>
    /// <param name="hwndOwner">Owner window the task Dialog will modal to.</param>
    /// <param name="verificationFlagChecked">Returns true if the verification checkbox was checked when the dialog
    /// was dismissed.</param>
    /// <param name="radioButtonResult">The radio botton selected by the user.</param>
    /// <returns>The result of the dialog, either a DialogResult value for common push buttons set in the CommonButtons
    /// member or the ButtonID from a TaskDialogButton structure set on the Buttons member.</returns>
    private int PrivateShow(IntPtr hwndOwner, out bool verificationFlagChecked, out int radioButtonResult)
    {
      verificationFlagChecked = false;
      radioButtonResult = 0;
      int result = 0;
      VistaUnsafeNativeMethods.TASKDIALOGCONFIG config = new VistaUnsafeNativeMethods.TASKDIALOGCONFIG();

      try
      {
        config.cbSize = (uint)Marshal.SizeOf(typeof(VistaUnsafeNativeMethods.TASKDIALOGCONFIG));
        config.hwndParent = hwndOwner;
        config.dwFlags = this.flags;
        config.dwCommonButtons = this.commonButtons;

        if (!string.IsNullOrEmpty(this.windowTitle))
        {
          config.pszWindowTitle = this.windowTitle;
        }

        config.MainIcon = (IntPtr)this.mainIcon;
        if (this.customMainIcon != null)
        {
          config.dwFlags |= VistaUnsafeNativeMethods.TASKDIALOG_FLAGS.TDF_USE_HICON_MAIN;
          config.MainIcon = this.customMainIcon.Handle;
        }

        if (!string.IsNullOrEmpty(this.mainInstruction))
        {
          config.pszMainInstruction = this.mainInstruction;
        }

        if (!string.IsNullOrEmpty(this.content))
        {
          config.pszContent = this.content;
        }

        VistaTaskDialogButton[] customButtons = this.buttons;
        if (customButtons.Length > 0)
        {
          // Hand marshal the buttons array.
          int elementSize = Marshal.SizeOf(typeof(VistaTaskDialogButton));
          config.pButtons = Marshal.AllocHGlobal(elementSize * (int)customButtons.Length);
          for (int i = 0; i < customButtons.Length; i++)
          {
            unsafe // Unsafe because of pointer arithmatic.
            {
              byte* p = (byte*)config.pButtons;
              Marshal.StructureToPtr(customButtons[i], (IntPtr)(p + (elementSize * i)), false);
            }

            config.cButtons++;
          }
        }

        VistaTaskDialogButton[] customRadioButtons = this.radioButtons;
        if (customRadioButtons.Length > 0)
        {
          // Hand marshal the buttons array.
          int elementSize = Marshal.SizeOf(typeof(VistaTaskDialogButton));
          config.pRadioButtons = Marshal.AllocHGlobal(elementSize * (int)customRadioButtons.Length);
          for (int i = 0; i < customRadioButtons.Length; i++)
          {
            unsafe // Unsafe because of pointer arithmatic.
            {
              byte* p = (byte*)config.pRadioButtons;
              Marshal.StructureToPtr(customRadioButtons[i], (IntPtr)(p + (elementSize * i)), false);
            }

            config.cRadioButtons++;
          }
        }

        config.nDefaultButton = this.defaultButton;
        config.nDefaultRadioButton = this.defaultRadioButton;

        if (!string.IsNullOrEmpty(this.verificationText))
        {
          config.pszVerificationText = this.verificationText;
        }

        if (!string.IsNullOrEmpty(this.expandedInformation))
        {
          config.pszExpandedInformation = this.expandedInformation;
        }

        if (!string.IsNullOrEmpty(this.expandedControlText))
        {
          config.pszExpandedControlText = this.expandedControlText;
        }

        if (!string.IsNullOrEmpty(this.collapsedControlText))
        {
          config.pszCollapsedControlText = this.CollapsedControlText;
        }

        config.FooterIcon = (IntPtr)this.footerIcon;
        if (this.customFooterIcon != null)
        {
          config.dwFlags |= VistaUnsafeNativeMethods.TASKDIALOG_FLAGS.TDF_USE_HICON_FOOTER;
          config.FooterIcon = this.customFooterIcon.Handle;
        }

        if (!string.IsNullOrEmpty(this.footer))
        {
          config.pszFooter = this.footer;
        }

        // If our user has asked for a callback then we need to ask for one to
        // translate to the friendly version.
        if (this.callback != null)
        {
          config.pfCallback = new VistaUnsafeNativeMethods.VistaTaskDialogCallback(this.PrivateCallback);
        }

        ////config.lpCallbackData = this.callbackData; // How do you do this? Need to pin the ref?
        config.cxWidth = this.width;

        // The call all this mucking about is here for.
        VistaUnsafeNativeMethods.TaskDialogIndirect(ref config, out result, out radioButtonResult, out verificationFlagChecked);
      }
      finally
      {
        // Free the unmanged memory needed for the button arrays.
        // There is the possiblity of leaking memory if the app-domain is destroyed in a non clean way
        // and the hosting OS process is kept alive but fixing this would require using hardening techniques
        // that are not required for the users of this class.
        if (config.pButtons != IntPtr.Zero)
        {
          int elementSize = Marshal.SizeOf(typeof(VistaTaskDialogButton));
          for (int i = 0; i < config.cButtons; i++)
          {
            unsafe
            {
              byte* p = (byte*)config.pButtons;
              Marshal.DestroyStructure((IntPtr)(p + (elementSize * i)), typeof(VistaTaskDialogButton));
            }
          }

          Marshal.FreeHGlobal(config.pButtons);
        }

        if (config.pRadioButtons != IntPtr.Zero)
        {
          int elementSize = Marshal.SizeOf(typeof(VistaTaskDialogButton));
          for (int i = 0; i < config.cRadioButtons; i++)
          {
            unsafe
            {
              byte* p = (byte*)config.pRadioButtons;
              Marshal.DestroyStructure((IntPtr)(p + (elementSize * i)), typeof(VistaTaskDialogButton));
            }
          }

          Marshal.FreeHGlobal(config.pRadioButtons);
        }
      }

      return result;
    }

    /// <summary>
    /// The callback from the native Task Dialog. This prepares the friendlier arguments and calls the simplier callback.
    /// </summary>
    /// <param name="hwnd">The window handle of the Task Dialog that is active.</param>
    /// <param name="msg">The notification. A TaskDialogNotification value.</param>
    /// <param name="wparam">Specifies additional noitification information.  The contents of this parameter depends on the value of the msg parameter.</param>
    /// <param name="lparam">Specifies additional noitification information.  The contents of this parameter depends on the value of the msg parameter.</param>
    /// <param name="refData">Specifies the application-defined value given in the call to TaskDialogIndirect.</param>
    /// <returns>A HRESULT. It's not clear in the spec what a failed result will do.</returns>
    private int PrivateCallback([In] IntPtr hwnd, [In] uint msg, [In] UIntPtr wparam, [In] IntPtr lparam, [In] IntPtr refData)
    {
      VistaTaskDialogCallback callback = this.callback;
      if (callback != null)
      {
        // Prepare arguments for the callback to the user we are insulating from Interop casting sillyness.

        // Future: Consider reusing a single ActiveTaskDialog object and mark it as destroyed on the destry notification.
        VistaActiveTaskDialog activeDialog = new VistaActiveTaskDialog(hwnd);
        VistaTaskDialogNotificationArgs args = new VistaTaskDialogNotificationArgs();
        args.Notification = (VistaTaskDialogNotification)msg;
        switch (args.Notification)
        {
          case VistaTaskDialogNotification.ButtonClicked:
          case VistaTaskDialogNotification.RadioButtonClicked:
            args.ButtonId = (int)wparam;
            break;
          case VistaTaskDialogNotification.HyperlinkClicked:
            args.Hyperlink = Marshal.PtrToStringUni(lparam);
            break;
          case VistaTaskDialogNotification.Timer:
            args.TimerTickCount = (uint)wparam;
            break;
          case VistaTaskDialogNotification.VerificationClicked:
            args.VerificationFlagChecked = (wparam != UIntPtr.Zero);
            break;
          case VistaTaskDialogNotification.ExpandoButtonClicked:
            args.Expanded = (wparam != UIntPtr.Zero);
            break;
        }

        return (callback(activeDialog, args, this.callbackData) ? 1 : 0);
      }

      return 0; // false;
    }

    /// <summary>
    /// Helper function to set or clear a bit in the flags field.
    /// </summary>
    /// <param name="flag">The Flag bit to set or clear.</param>
    /// <param name="value">True to set, false to clear the bit in the flags field.</param>
    private void SetFlag(VistaUnsafeNativeMethods.TASKDIALOG_FLAGS flag, bool value)
    {
      if (value)
      {
        this.flags |= flag;
      }
      else
      {
        this.flags &= ~flag;
      }
    }
  }
}
