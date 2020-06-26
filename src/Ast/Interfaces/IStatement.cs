namespace Lang.Ast
{
  public interface IStatement : INode
  {
    void AcceptVisitor(IStatementVisitor visitor) => visitor.Visit(this, GetType());
    T AcceptVisitor<T>(IStatementVisitor<T> visitor) => visitor.Visit(this, GetType());
  }
}