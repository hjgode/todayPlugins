using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

//using todayDeviceID;

using System.IO;
using System.Reflection;
using ChrisTec.WindowsMobile.TodayScreen;

namespace testPlugin
{
    public partial class testPluginsForm : Form
    {
        public testPluginsForm()
        {
            InitializeComponent();
            //todayDeviceID.todayDeviceID td = new todayDeviceID.todayDeviceID();
            //this.Controls.Add(td);

            //todayApp1.todayApp1 ta1 = new todayApp1.todayApp1();
            //this.Controls.Add(ta1);
            //ta1.Top = td.Height + 6;
            loadDLLs();
        }

        void loadDLLs()
        {
            // For each DLL in the same folder as the ManagedTodayScreenItemHost executable
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);

            foreach (string dll in Directory.GetFiles(path, "*.dll"))
            {
                try
                {
                    Assembly assembly = Assembly.LoadFrom(dll);

                    // Check each type defined within it
                    foreach (Type type in assembly.GetTypes())
                    {
                        // To see if it has been marked with the TodayScreenItem attribute
                        TodayScreenItemAttribute[] attrs = (TodayScreenItemAttribute[])type.GetCustomAttributes(typeof(TodayScreenItemAttribute), false);
                        if (attrs != null && attrs.Length == 1)
                        {
                            // If it has create an instance of the class (it should be a control of some type)
                            // and place it onto the form.
                            Control ctrl = (Control)Activator.CreateInstance(type);

                            this.AddControl(ctrl);
                        }
                    }
                }
                catch (IOException)
                {
                    // Safely ignore this, it's probably us attempting to load
                    // a native DLL via Assembly.LoadFrom()
                }
            }
        }
        public void AddControl(Control c)
        {
          if (Controls.Count > 0)
          {
            // Add a seperator between the previous control
            Panel pnl = new Panel();
            pnl.Height = 6;
            pnl.Dock = DockStyle.Top;
            Controls.Add(pnl);
          }

          // Add the actual control
          c.Dock = DockStyle.Top;

          //if (hWndNative != IntPtr.Zero)
          //{
          //  hooks.Add(new WindowHook(this, c));
          //}
          
          Controls.Add(c);
        }
    }//CLASS
}//NAMESPACE