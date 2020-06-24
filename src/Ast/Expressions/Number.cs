namespace Lang.Ast.Expressions
{
  class Number : IExpression
  {
    public int Position { get; }
    public string Lexeme { get; }
    public Number(int pos, string lexeme) => (Position, Lexeme) = (pos, lexeme);
    public string FormattedString => Lexeme;
  }
}