using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Lang.Utils
{
  public static class RegexExt
  {
    public static IEnumerable<Match> GetMatches(this Regex regex, string text)
    {
      for (var m = regex.Match(text); m.Success; m = m.NextMatch())
      {
        yield return m;
      }
    }
  }
}