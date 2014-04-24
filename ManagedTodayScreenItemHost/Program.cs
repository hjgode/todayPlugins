using System;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using Microsoft.Win32;
using ChrisTec.WindowsMobile.TodayScreen;

namespace ManagedTodayScreenItemHost
{
  static class Program
  {
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [MTAThread]
    static void Main()
    {
      TodayScreenItem form = new TodayScreenItem(FindNativeHandle());

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

              form.AddControl(ctrl);
            }
          }
        }
        catch (IOException)
        {
          // Safely ignore this, it's probably us attempting to load
          // a native DLL via Assembly.LoadFrom()
        }
      }

      // Finally display the form
      Application.Run(form);
    }

    private static IntPtr FindNativeHandle()
    {
      // Determine the window handle for the native part of
      // our today screen item
      // [CF] - TODO - handle the exception here if this key can't be read...
      RegistryKey key = Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\Today\Items\Managed Items");
      object val = key.GetValue("NativeHandle");
      key.Close();

      return (IntPtr)(int)val;
    }

  }
}