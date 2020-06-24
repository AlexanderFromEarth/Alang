using System.Collections.Generic;
using System.Linq;

namespace Lang.Ast.BaseNodes
{
  class Block : INode
  {
    public IReadOnlyList<IStatement> Statements { get; }
    public Block(IReadOnlyList<IStatement> statements) => Statements = statements;
    public string FormattedString => "{\n" + string.Join("", Statements.Select(x => x.FormattedString)) + "}\n";
  }
}