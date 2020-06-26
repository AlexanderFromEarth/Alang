using System.Collections.Generic;
using System.Linq;

namespace Lang.Ast.Expressions
{
  class Lambda : IExpression
  {
    public int Position { get; }
    public IExpression Body { get; }
    public IReadOnlyList<Identifier> Arguments { get; }
    public string FormattedString => $"({string.Join(", ", Arguments.Select(x => x.FormattedString))}) => {Body.FormattedString}";
    public Lambda(int pos, IReadOnlyList<Identifier> args, IExpression expr) => (Position, Arguments, Body) = (pos, args, expr);
  }
}