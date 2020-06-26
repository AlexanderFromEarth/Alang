using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Lang.Ast;
using Lang.Ast.BaseNodes;
using Lang.Ast.Expressions;
using Lang.Ast.Statements;
using Lang.Interpreting.Types;
using Lang.Parsing;
using Lang.Utils;

namespace Lang.Interpreting
{
  class Interpreter : IStatementVisitor, IExpressionVisitor<object>
  {
    SourceFile SourceFile { get; set; }
    public IDictionary<string, object> Constants { get; set; }
    public Interpreter()
    {
      Assembly.GetExecutingAssembly()
                  .GetTypes()
                  .Where(t => t.IsClass && t.Namespace == "Lang.Interpreting.Values" && t.DeclaringType == null)
                  .ToList().ForEach(Console.WriteLine);
      Constants = new Dictionary<string, object>(Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => t.IsClass && t.Namespace == "Lang.Interpreting.Values" && t.DeclaringType == null)
            .Select(Activator.CreateInstance)
            .Select(x => KeyValuePair.Create((string)x.GetType().GetMethod("GetPrintString").Invoke(x, null), x))
            .Concat(new KeyValuePair<string, object>[] { KeyValuePair.Create("true", (object)true), KeyValuePair.Create("false", (object)false), KeyValuePair.Create("null", (object)null) }));
    }
    Exception MakeError(IExpression expr, string msg) => new Exception(SourceFile.MakeErrorMessage(expr.Position, msg));
    public void RunProgram(ProgramNode program)
    {
      SourceFile = program.SourceFile;
      try
      {
        foreach (var statement in program.Statements)
        {
          Run(statement);
        }
      }
      finally
      {
        SourceFile = null;
      }
    }
    void RunBlock(Block block)
    {
      var old = Constants;
      Constants = new Dictionary<string, object>();
      foreach (var statement in block.Statements)
      {
        Run(statement);
      }
      Constants = old;
    }

    void Run(IStatement statement) => statement.AcceptVisitor(this);
    public void VisitExpressionStatement(ExpressionStatement expressionStatement) => Calc(expressionStatement.Expression);
    public void VisitDeclaration(Declaration declaration)
    {
      var name = declaration.Name;
      if (Constants.ContainsKey(name))
      {
        throw MakeError(declaration.Expression, "Can't assigment to constant");
      }
      Constants[name] = Calc(declaration.Expression);
    }
    object Calc(IExpression expr) => expr.AcceptVisitor(this);
    T Calc<T>(IExpression expr)
    {
      var value = Calc(expr);
      if (!(value is T))
      {
        throw MakeError(expr, $"Expected {typeof(T)}, but get {value}");
      }
      return (T)value;
    }
    public object VisitBinary(Binary binary)
    {
      switch (binary.Operator)
      {
        case BinaryOperator.Addition:
          return CalcAddition(binary);
        case BinaryOperator.Subtraction:
          return CalcSubtraction(binary);
        case BinaryOperator.Multiplication:
          return CalcMultiplication(binary);
        case BinaryOperator.Division:
          return CalcDivision(binary);
        case BinaryOperator.Equal:
          return CalcEqual(binary);
        case BinaryOperator.NotEqual:
          return CalcNotEqual(binary);
        case BinaryOperator.StrictLess:
          return CalcStrictLess(binary);
        case BinaryOperator.Less:
          return CalcLess(binary);
        case BinaryOperator.StrictGreater:
          return CalcStrictGreater(binary);
        case BinaryOperator.Greater:
          return CalcGreater(binary);
        case BinaryOperator.Or:
          return CalcOr(binary);
        case BinaryOperator.And:
          return CalcAnd(binary);
        case BinaryOperator.Pipe:
          return CalcPipe(binary);
        default:
          throw MakeError(binary, $"Unknown operation {binary.Operator}");
      }
    }
    public object VisitLambda(Lambda lambda)
    {
      return new Function(x =>
      {
        var old = Constants;
        Constants = new Dictionary<string, object>(Constants);
        lambda.Arguments.Zip(x).ToList().ForEach(x => Constants[x.First.Name] = x.Second);
        var res = Calc(lambda.Body);
        Constants = old;
        return res;
      });
    }
    object CalcPipe(Binary binary)
    {
      var left = Calc(binary.Left);
      if (!(binary.Right is Call call))
      {
        throw MakeError(binary, $"Right operand must be a call, but get {binary.Right}");
      }
      var value = Calc(call.Function);
      if (!(value is ICallable func))
      {
        throw MakeError(call, $"{value} is not callable");
      }
      var args = left.Yield().Concat(call.Arguments.Select(Calc)).ToList();
      return func.Call(args);
    }
    object CalcAddition(Binary binary)
    {
      return Calc<int>(binary.Left) + Calc<int>(binary.Right);
    }
    object CalcSubtraction(Binary binary)
    {
      return Calc<int>(binary.Left) - Calc<int>(binary.Right);
    }
    object CalcMultiplication(Binary binary)
    {
      return Calc<int>(binary.Left) * Calc<int>(binary.Right);
    }
    object CalcDivision(Binary binary)
    {
      return Calc<int>(binary.Left) / Calc<int>(binary.Right);
    }
    object CalcRemainder(Binary binary)
    {
      return Calc<int>(binary.Left) % Calc<int>(binary.Right);
    }
    object CalcOr(Binary binary)
    {
      return Calc<bool>(binary.Left) || Calc<bool>(binary.Right);
    }
    object CalcAnd(Binary binary)
    {
      return Calc<bool>(binary.Left) && Calc<bool>(binary.Right);
    }
    object CalcEqual(Binary binary)
    {
      var a = Calc(binary.Left);
      var b = Calc(binary.Right);
      if (a == null)
      {
        return b == null;
      }
      if (b == null)
      {
        return a == null;
      }
      if (a.GetType() != b.GetType())
      {
        return false;
      }
      if (a is int)
      {
        return (int)a == (int)b;
      }
      if (a is bool)
      {
        return (bool)a == (bool)b;
      }
      throw MakeError(binary, $"Unsupported type of operands {a} {b}");
    }
    object CalcNotEqual(Binary binary)
    {
      return (bool)CalcEqual(binary) == false;
    }
    object CalcStrictLess(Binary binary)
    {
      var a = Calc(binary.Left);
      var b = Calc(binary.Right);
      if (a == null && b == null)
      {
        return false;
      }
      if (a is bool && b is bool)
      {
        return !(bool)a && (bool)b;
      }
      if (a is int && b is int)
      {
        return (int)a < (int)b;
      }
      throw MakeError(binary, $"Unsupported type of operands {a} {b}");
    }
    object CalcStrictGreater(Binary binary)
    {
      return (bool)CalcLess(binary) == false;
    }
    object CalcGreater(Binary binary)
    {
      return (bool)CalcStrictLess(binary) == false;
    }
    object CalcLess(Binary binary)
    {
      return (bool)CalcStrictLess(binary) || (bool)CalcEqual(binary);
    }
    public object VisitCall(Call call)
    {
      var value = Calc(call.Function);
      if (!(value is ICallable func))
      {
        throw MakeError(call, $"{value} is not callable");
      }
      var args = call.Arguments.Select(Calc).ToList();
      return func.Call(args);
    }
    public object VisitParentheses(Parentheses parentheses) => Calc(parentheses.Expression);
    public object VisitNumber(Number number)
    {
      if (int.TryParse(number.Lexeme, NumberStyles.None, NumberFormatInfo.InvariantInfo, out int value))
      {
        return value;
      }
      throw MakeError(number, $"Failed to convert {number.Lexeme} to int");
    }
    public object VisitIdentifier(Identifier identifier)
    {
      if (Constants.TryGetValue(identifier.Name, out object value))
      {
        return value;
      }
      throw MakeError(identifier, $"Unknown variable {identifier.Name}");
    }
  }
}