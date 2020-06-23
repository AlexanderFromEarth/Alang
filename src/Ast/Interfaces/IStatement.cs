namespace Lang.Ast
{
  public interface IStatement : INode
  {
    void AcceptVisitor(IStatementVisitor visitor) => visitor.Visit(this, GetType());
    T AcceptVisitor<T>(IStatementVisitor<T> visitor) where T : class => visitor.Visit(this, GetType());
  }
}