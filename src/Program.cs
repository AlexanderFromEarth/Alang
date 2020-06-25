using System;
using System.Linq;
using Lang.Ast;
using Lang.Ast.BaseNodes;
using Lang.Interpreting;
using Lang.Interpreting.Functions;
using Lang.Parsing;

namespace Lang
{
  class Program
  {
    static void Main(string[] args)
    {
      var interpreter = new Interpreter();
      interpreter.RunProgram(Parser.Parse(SourceFile.Read(args[0])));
      foreach (var variable in interpreter.Variables.OrderBy(x => x.Key, StringComparer.Ordinal))
      {
        Console.WriteLine($" - {variable.Key}: {PrintFunction.ValueToString(variable.Value)}");
      }
    }
  }
}