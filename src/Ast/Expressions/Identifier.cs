namespace Lang.Ast.Expressions
{
  class Identifier : IExpression
  {
    public int Position { get; }
    public string Name { get; }
    public Identifier(int pos, string name) => (Position, Name) = (pos, name);
  }
}