using System;

namespace Lang.Ast
{
  public interface IExpressionVisitor
  {
    void Visit(IExpression expression, Type type) => GetType().GetMethod("Visit" + type.Name).Invoke(this, new[] { expression });
  }

  public interface IExpressionVisitor<T>
  {
    T Visit(IExpression expression, Type type) => (T)GetType().GetMethod("Visit" + type.Name).Invoke(this, new[] { expression });
  }
}