using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ManagedTodayScreenItemHost
{
  internal class WindowHook
  {
    private TodayScreenItem item;
    private Control control;

    private IntPtr realWindowProc = IntPtr.Zero;
    private WindowProcCallback newWindowProc;

    #region Native Win32 API stuff

    private delegate int WindowProcCallback(IntPtr hWnd, uint msg, uint wParam, uint lParam);

    private static readonly int GWL_WNDPROC = -4;
    private static readonly int WM_DESTROY = 0x0002;
    private static readonly int WM_ERASEBKGND = 0x0014;

    [DllImport("coredll.dll")]
    private static extern int CallWindowProc(IntPtr lpPrevWndFunc, IntPtr hwnd, uint msg, uint wParam, uint lParam);

    [DllImport("coredll.dll")]
    private static extern IntPtr SetWindowLong(IntPtr hwnd, int nIndex, IntPtr dwNewLong);

    #endregion

    public WindowHook(TodayScreenItem item, Control control)
    {
      this.item = item;
      this.control = control;

      // Subclass the window procedure associated with this control, so that
      // we can hook the requests to paint it's background
      newWindowProc = new WindowProcCallback(WndProc);
      realWindowProc = SetWindowLong(control.Handle, GWL_WNDPROC, Marshal.GetFunctionPointerForDelegate(newWindowProc));
    }

    // Called whenever this control gets a native Win32 message sent to it
    private int WndProc(IntPtr hwnd, uint msg, uint wParam, uint lParam)
    {
      if (msg == WM_DESTROY)
      {
        // The window is being destroyed, so unsubclass this window
        // by resetting the WNDPROC pointer to it's original value
        SetWindowLong(control.Handle, GWL_WNDPROC, realWindowProc);
      }
      else if (msg == WM_ERASEBKGND)
      {
        // Pass the request to paint the background of the control
        // to the native part of the today screen item (so we get
        // the correct watermark drawn on the image).
        item.PaintBackground(control.Handle);
        return 1; // we have erased the background
      }

      // Call the original Window Procedure associated with the control
      return CallWindowProc(realWindowProc, hwnd, msg, wParam, lParam);
    }
  }
}
