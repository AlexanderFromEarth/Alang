using System.Collections.Generic;

namespace Lang.Ast.BaseNodes
{
  class Block : INode
  {
    public IReadOnlyList<IStatement> Statements { get; }
    public Block(IReadOnlyList<IStatement> statements) => Statements = statements;
  }
}