namespace Lang.Ast
{
  public interface IExpression : INode
  {
    void AcceptVisitor(IExpressionVisitor visitor) => visitor.Visit(this, GetType());
    T AcceptVisitor<T>(IExpressionVisitor<T> visitor) where T : class => visitor.Visit(this, GetType());
  }
}