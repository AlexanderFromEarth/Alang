namespace Lang.Ast.Statements
{
  class Declaration : IStatement
  {
    public string Name { get; }
    public IExpression Expression { get; }
    public Declaration(string name, IExpression expr) => (Name, Expression) = (name, expr);
  }
}