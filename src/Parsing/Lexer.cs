using System;
using System.Collections.Generic;
using System.Linq;
using Lang.Utils;

namespace Lang.Parsing
{
  static class Lexer
  {
    public static IEnumerable<Token> GetTokens(SourceFile src)
    {
      var pos = 0;
      var names = Regexes.Instance.TokenNames;
      var text = src.Text;
      Exception MakeError(string msg) => new Exception(src.MakeErrorMessage(pos, msg));
      foreach (var m in Regexes.Instance.Regex.GetMatches(text))
      {
        if (pos < m.Index)
        {
          throw MakeError($"Skipped '{text.Substring(pos, m.Index - pos)}'");
        }
        switch (names.Count(x => m.Groups[x.Item2].Success))
        {
          case 0: throw new Exception("Nothing(");
          case 1:
            yield return new Token(m.Index, names.First(x => m.Groups[x.Item2].Success).Item1, m.Value);
            break;
          default: throw new Exception("Too many(");
        }
        pos = m.Index + m.Length;
      }
      if (pos < text.Length)
      {
        throw MakeError($"Skipped '{text.Substring(pos)}'");
      }
    }
  }
}