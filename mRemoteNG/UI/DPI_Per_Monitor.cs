using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

/// <summary>
///
/// DPI_Per_Monitor
///
/// Use to make a simple Windows for app truely DPI-aware.
/// That is AVOID woolly apps!
///
/// 1) Make sure your forms are designed with font size set in PIXELS, not POINTS (yes!!)
/// All controls possible should use inherited fonts
/// Each font explicitly given in PIXEL needs to be handled  in the callback function
/// provided to Check_WM_DPICHANGED
/// The standard "8.25pt" matches "11.0px" (8.25 *96/72 = 11)
///
/// 2) Leave the form in the default Autoscale=Font mode
///
/// 3) Insert call to DPI_Per_Monitor.TryEnableDPIAware() , right after InitializeComponent();
///
/// 4) Add suitable call to DPI_Per_Monitor.Check_WM_DPICHANGED_WM_NCCREATE in DefWndProc
/// If a DefWndProc override is not already present add e.g. these lines
///
///     protected override void DefWndProc(ref Message m) {
///         DPI_Per_Monitor.Check_WM_DPICHANGED_WM_NCCREATE(SetUserFonts,m, this.Handle);
///         base.DefWndProc(ref m);
///     }
///
/// that has a call-back function you must provide, that set fonts as needed (in pixels or points),
/// if all are inherited, then just one set:
///
///     void SetUserFonts(float scaleFactorX, float scaleFactorY) {
///         Font = new Font(Font.FontFamily, 11f * scaleFactorX, GraphicsUnit.Pixel);
///     }
///     
/// 5) And a really odd one, due to a Visual studio BUG. 
/// This ONLY works, if your PRIMARY monitor is scalled at 100% at COMPILE time!!!
/// It is NOT just a matter of using a different reference than the 96 dpi below, and
/// it does not help to run it from a secondary monitor set to 100% !!!
/// And to make things worse, Visual Studio is one of the programs that doesn't handle
/// change of scale on primary monitor, without at the least a sign out....
/// 
/// NOTE that if you got (Checked)ListBoxes, repeated autosizing (e.g. move between monitors)
/// might fail as it rounds the height down to a multipla of the itemheight. So despite a 
/// bottom-anchor it will 'creep' upwards...
/// So I recommend to place an empty and/or hidden bottom-anchored label just below the boxes,
/// to scale the spacing and set e.g. :  yourList.Height=yourAnchor.Top-yourList.Top
/// 
/// Also note that not everything gets scaled automatically. Only new updates of Win10 handles
/// the titlebar correctly. Also the squares of checkboxes are forgotten.
/// </summary>

static class DPI_Per_Monitor
{
    [DllImport("SHCore.dll")]
    private static extern bool SetProcessDpiAwareness(PROCESS_DPI_AWARENESS awareness);

    private enum PROCESS_DPI_AWARENESS {
        Process_DPI_Unaware = 0,
        Process_System_DPI_Aware = 1,
        Process_Per_Monitor_DPI_Aware = 2
    }

    [DllImport("user32.dll", SetLastError = true)]
    static extern bool SetProcessDPIAware();

    public static float MeanDPIprimary = 96f;

    [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
    public static extern IntPtr SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);
    [DllImport("user32.dll")]
    internal static extern bool SetProcessDpiAwarenessContext(IntPtr XXXX);
    [DllImport("user32.dll")]
    internal static extern IntPtr GetWindowDpiAwarenessContext(IntPtr hWnd);

    public static IntPtr DPI_AWARENESS_CONTEXT = GetWindowDpiAwarenessContext(System.Diagnostics.Process.GetCurrentProcess().Handle);

    public enum DpiType
    {
        Effective = 0,
        Angular = 1,
        Raw = 2,
    }
    [DllImport("Shcore.dll")]
    private static extern IntPtr GetDpiForMonitor([In]IntPtr hmonitor, [In]DpiType dpiType, [Out]out uint dpiX, [Out]out uint dpiY);
    [DllImport("User32.dll")]
    private static extern IntPtr MonitorFromPoint([In]System.Drawing.Point pt, [In]uint dwFlags);

    static List<Tuple<Control, Form, float>> ManResCtrl = new List<Tuple<Control, Form, float>>();
    internal static void TryEnableDPIAware(Form form, VoidOfFloatFloatDelegate CallBackWithScale)
    {
        int handledLev = 0;
        if (0==handledLev)
            try { //"Creators update", new method, Only onw supported by newer VS: DPI_AWARENESS_CONTEXT_PER_MONITOR_AWARE_V2
                if (SetProcessDpiAwarenessContext(DPI_AWARENESS_CONTEXT - 4)) handledLev = 3;
            } catch {}
        if(0==handledLev)
            try { //"Aniversary update", 'new' method, now depreceated
                SetProcessDpiAwareness(PROCESS_DPI_AWARENESS.Process_Per_Monitor_DPI_Aware);
                handledLev = 2;
            } catch { }
        if (0==handledLev)
            try { // fallback, use (simpler) internal function - backwars compaibility that has been broken by newer Windows and Visual Studio...
                SetProcessDPIAware();
                handledLev = 1;
            } catch { }
        try {
            var mon = MonitorFromPoint(new System.Drawing.Point(1, 1), 2/*MONITOR_DEFAULTTONEAREST*/); //(0,0) always top left of primary
            uint dpiX;
            uint dpiY;
            GetDpiForMonitor(mon, DpiType.Effective, out dpiX, out dpiY);
            if (dpiX!=96 || dpiY!=96) {
                CallBackWithScale(dpiX/96f, dpiY/96f);
            }
        } catch { /* Windows older than WinX */ }
    }

    [DllImport("user32.dll", SetLastError = true)]
    public static extern bool EnableNonClientDpiScaling(IntPtr hWnd);

    private static SemaphoreSlim semaphoreScale = new SemaphoreSlim(1, 1);
    internal delegate void VoidOfFloatFloatDelegate(float x, float y);
    static Int32 Oldscales = -1;
    static bool isCurrentlyScaling=false;
    internal static void Check_WM_DPICHANGED_WM_NCCREATE(VoidOfFloatFloatDelegate CallBackWithScale, Message m, IntPtr hwnd) {
        switch (m.Msg) {
            case 0x02E0:  //WM_DPICHANGED
                try {
                    semaphoreScale.Wait(2000); //timeout??
                    bool Local_isCurrentlyScaling = isCurrentlyScaling;
                    isCurrentlyScaling = true;
                    semaphoreScale.Release();
                    if (Local_isCurrentlyScaling) break; //We will get it again if we are moving....

                    Int32 CurrentScales = m.WParam.ToInt32();
                    //if (ScaleFactorsLastAndPendingQueue[0]!=ScaleFactorsLastAndPendingQueue[1]) //We MIGHT get the message more than once!!!
                    if (Oldscales!= CurrentScales) { //We MIGHT get the message more than once!!!
                        float scaleFactorX = (CurrentScales & 0xFFFF) / 96f; //###SEE NOTES!!###
                        float scaleFactorY = (CurrentScales >> 16) / 96f; //###SEE NOTES!!###
                        CallBackWithScale(scaleFactorX, scaleFactorY);
                    }
                    semaphoreScale.Wait(2000); //timeout??
                    Oldscales = CurrentScales;
                    isCurrentlyScaling = false;
                    semaphoreScale.Release();
                }
                catch {}

                break;
            case 0x81: //WM_NCCREATE
                try { EnableNonClientDpiScaling(hwnd); } catch { } //Scale header too if updates of Windows 10 between "Aniversary" and "Creators"...
                break;
            default:
                break;
        }
    }
}
