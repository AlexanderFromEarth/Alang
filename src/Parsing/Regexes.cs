using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Lang.Parsing
{
  class Regexes
  {
    static readonly IReadOnlyList<(TokenType, string)> patterns = new List<(TokenType, string)> {
      ( TokenType.Whitespaces, @"[\s\t\r\n]+" ),
      ( TokenType.Identifier, @"[a-zA-Z_][a-zA-Z0-9_]*"),
      ( TokenType.SingleLineComment, @"\#[^\r\n]*" ),
      ( TokenType.NumberLiteral, @"[0-9]+"),
      ( TokenType.OperatorOrPunctuator, @"==|!=|\|>|<=|>=|\|\||&&|[-+*/%,.<>=;(){}[\]]" ),
    };
    static Regexes instance;
    public static Regexes Instance => instance ?? (instance = new Regexes());
    public readonly Regex Regex = RegexUtils.CreateRegex(string.Join("\n|\n", patterns.Select(x => $"(?<{x.Item1}>{x.Item2})")));
    public readonly IReadOnlyDictionary<TokenType, Regex> TokenRegexes = new Dictionary<TokenType, Regex>(patterns.Select(x => KeyValuePair.Create(x.Item1, RegexUtils.CreateRegex(@"\A(" + x.Item2 + @")\z"))));
    public readonly IEnumerable<(TokenType, string)> TokenNames = patterns.Select(x => (x.Item1, x.Item1.ToString()));
  }
}
