using System;

namespace ZeusInspector.Attributes
{
  [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
  public class ZExportGroupAttribute : Attribute
  {
    public enum Orientation
    {
      Horizontal,
      Vertical
    }

    public string GroupName { get; private set; }
    public Orientation Or { get; private set; }
    public string Prefix { get; private set; }

    public ZExportGroupAttribute(string groupName = "", string prefix = "", Orientation orientation = Orientation.Horizontal)
    {
      GroupName = groupName;
      Or = orientation;
      Prefix = prefix;
    }
  }
}
