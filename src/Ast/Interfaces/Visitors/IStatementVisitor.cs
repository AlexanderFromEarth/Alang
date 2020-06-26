using System;

namespace Lang.Ast
{
  public interface IStatementVisitor
  {
    void Visit(IStatement expression, Type type) => GetType().GetMethod("Visit" + type.Name).Invoke(this, new[] { expression });
  }

  public interface IStatementVisitor<T>
  {
    T Visit(IStatement expression, Type type) => (T)GetType().GetMethod("Visit" + type.Name).Invoke(this, new[] { expression });
  }
}