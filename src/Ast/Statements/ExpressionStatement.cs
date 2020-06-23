namespace Lang.Ast.Statements
{
  class ExpressionStatement : IStatement
  {
    public IExpression Expr { get; }
    public ExpressionStatement(IExpression expr) => Expr = expr;
  }
}