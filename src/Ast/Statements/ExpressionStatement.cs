namespace Lang.Ast.Statements
{
  class ExpressionStatement : IStatement
  {
    public IExpression Expression { get; }
    public ExpressionStatement(IExpression expr) => Expression = expr;
    public string FormattedString => $"{Expression.FormattedString}\n";
  }
}