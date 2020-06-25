using System.Collections.Generic;
using System.Linq;

namespace Lang.Ast.Expressions
{
  class Call : IExpression
  {
    public int Position { get; }
    public IExpression Function { get; }
    public IReadOnlyList<IExpression> Arguments { get; }
    public string FormattedString => $"{Function.FormattedString}({string.Join(", ", Arguments.Select(x => x.FormattedString))})";
    public Call(int pos, IExpression name, IReadOnlyList<IExpression> args) => (Position, Function, Arguments) = (pos, name, args);
  }
}