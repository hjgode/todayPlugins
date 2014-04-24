using System;
using System.Windows.Forms;
using Microsoft.WindowsCE.Forms;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace ManagedTodayScreenItemHost
{
  internal class TodayScreenItem : Form
  {
    private IntPtr hWndNative;
    private List<WindowHook> hooks = new List<WindowHook>();

    public TodayScreenItem()
    {
      this.hWndNative = IntPtr.Zero;
    }

    public TodayScreenItem(IntPtr hWndNative)
    {
      this.hWndNative = hWndNative;
    }

    protected override void OnLoad(EventArgs e)
    {
      // Ensure we are resizeable
      this.FormBorderStyle = FormBorderStyle.None;

      // Inform the native part of this plugin what our window handle and
      // required height is (so that it can re-parent us as a child of the
      // native window).
      if (hWndNative != IntPtr.Zero)
      {
        Message msg = new Message();
        msg.HWnd = hWndNative;
        msg.Msg = 1234;
        msg.LParam = Handle;
        msg.WParam = (IntPtr)CalculateRequiredHeight();
        MessageWindow.SendMessage(ref msg);
      }

      base.OnLoad(e);
    }

    protected override void OnPaintBackground(PaintEventArgs e)
    {
      if (hWndNative == IntPtr.Zero)
      {
        // While in "test" mode paint our background normally
        base.OnPaintBackground(e);
      }
      else
      {
        // Tell the native part of this item that we
        // want it to repaint our background (so we get
        // a nice gradient fill).
        PaintBackground(Handle);
      }
    }

    internal void PaintBackground(IntPtr hWnd)
    {
      Message msg = new Message();
      msg.HWnd = hWndNative;
      msg.Msg = 1235;
      msg.LParam = hWnd;
      msg.WParam = IntPtr.Zero;
      MessageWindow.SendMessage(ref msg);
    }

    public UInt32 CalculateRequiredHeight()
    {
      UInt32 height = 0;

      foreach (Control c in this.Controls)
      {
        if (c.Dock == DockStyle.Top)
        {
          height += (UInt32)c.Height;
        }
      }

      return height;
    }

    public void AddControl(Control c)
    {
      if (Controls.Count > 0)
      {
        // Add a seperator between the previous control
        Panel pnl = new Panel();
        pnl.Height = 1;
        pnl.Dock = DockStyle.Top;
        Controls.Add(pnl);
      }

      // Add the actual control
      c.Dock = DockStyle.Top;

      if (hWndNative != IntPtr.Zero)
      {
        hooks.Add(new WindowHook(this, c));
      }
      
      Controls.Add(c);
    }
  }
}
