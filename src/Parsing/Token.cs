namespace Lang.Parsing
{
  class Token
  {
    public int Position { get; }
    public TokenType Type { get; }
    public string Lexeme { get; }
    public Token(int pos, TokenType type, string lexeme) => (Position, Type, Lexeme) = (pos, type, lexeme);
  }
}