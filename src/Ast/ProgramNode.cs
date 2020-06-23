using System.Collections.Generic;
using Lang.Parsing;

namespace Lang.Ast
{
  class ProgramNode : INode
  {
    public SourceFile SourceFile { get; }
    public IReadOnlyList<IStatement> Statements { get; }
    public ProgramNode(SourceFile src, IReadOnlyList<IStatement> statements) => (SourceFile, Statements) = (src, statements);
  }
}