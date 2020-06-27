using System;
using System.Collections.Generic;

namespace Lang.Utils
{
  public static class EnumExt
  {
    public static IEnumerable<T> Yield<T>(T item)
    {
      yield return item;
    }
    public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action)
    {
      foreach (var item in collection)
      {
        action(item);
      }
    }
  }
}