using System;

namespace Lang.Ast
{
  public interface IExpressionVisitor
  {
    void Visit(IExpression expression, Type type) => GetType().GetMethod("Visit" + type.Name).Invoke(this, new[] { expression });
  }

  public interface IExpressionVisitor<T> where T : class
  {
    T Visit(IExpression expression, Type type) => GetType().GetMethod("Visit" + type.Name).Invoke(this, new[] { expression }) as T;
  }
}