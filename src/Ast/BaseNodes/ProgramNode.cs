using System.Collections.Generic;
using System.Linq;
using Lang.Parsing;

namespace Lang.Ast.BaseNodes
{
  class ProgramNode : INode
  {
    public SourceFile SourceFile { get; }
    public IReadOnlyList<IStatement> Statements { get; }
    public ProgramNode(SourceFile src, IReadOnlyList<IStatement> statements) => (SourceFile, Statements) = (src, statements);
    public string FormattedString => string.Join("", Statements.Select(x => x.FormattedString));
  }
}