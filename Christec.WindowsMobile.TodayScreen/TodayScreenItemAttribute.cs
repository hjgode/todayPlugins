using System;

namespace ChrisTec.WindowsMobile.TodayScreen
{
  [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
  public class TodayScreenItemAttribute : Attribute
  {
    protected string name;

    public TodayScreenItemAttribute(string name)
    {
      this.name = name;
    }

    public string Name
    {
      get { return name; }
    }
  }
}
