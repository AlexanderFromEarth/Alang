namespace Lang.Ast.Expressions
{
  class Parentheses : IExpression
  {
    public int Position { get; }
    public IExpression Expression { get; }
    public Parentheses(int pos, IExpression expr) => (Position, Expression) = (pos, expr);
    public string FormattedString => $"({Expression.FormattedString})";
  }
}