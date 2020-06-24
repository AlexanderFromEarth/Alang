using System.Collections.Generic;

namespace Lang.Utils
{
  public static class EnumExt
  {
    public static IEnumerable<T> Yield<T>(this T item)
    {
      yield return item;
    }
  }
}