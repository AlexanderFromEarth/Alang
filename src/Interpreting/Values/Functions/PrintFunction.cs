using System;
using System.Collections.Generic;
using System.Linq;
using Lang.Interpreting.Types;

namespace Lang.Interpreting.Values
{
  class PrintFunction : Function
  {
    public PrintFunction() : base(args =>
    {
      Console.WriteLine(string.Join(" ",
        args.Select(ValueToString)));
      return null;
    })
    { }
    public override string GetPrintString() => "print";
    public static string ValueToString(object value)
    {
      if (value == null)
      {
        return "null";
      }
      if (value is bool)
      {
        return (bool)value ? "true" : "false";
      }
      if (value is int)
      {
        return ((int)value).ToString(System.Globalization.NumberFormatInfo.InvariantInfo);
      }
      if (value is IPrintable p)
      {
        return p.GetPrintString();
      }
      throw new Exception($"Unknown type {value}");
    }
  }
}