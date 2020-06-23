using System.Collections.Generic;

namespace Lang.Ast.Expressions
{
  enum BinaryOperator
  {
    Addition,
    Subtraction,
    Multiplication,
    Division,
    Or,
    And,
    Pipe,
    Equal,
    Less,
  }
  class Binary : IExpression
  {
    public IExpression Left { get; }
    public BinaryOperator Operator { get; }
    public IExpression Right { get; }
    public Binary(IExpression left, BinaryOperator op, IExpression right) => (Left, Operator, Right) = (left, op, right);
    static readonly IReadOnlyDictionary<BinaryOperator, string> operators = new Dictionary<BinaryOperator, string> {
      { BinaryOperator.And, "&&" },
      { BinaryOperator.Division, "/" },
      { BinaryOperator.Equal, "==" },
      { BinaryOperator.Less, "<" },
      { BinaryOperator.Subtraction, "-" },
      { BinaryOperator.Or, "||" },
      { BinaryOperator.Pipe, "|>" },
      { BinaryOperator.Addition, "+" },
      { BinaryOperator.Multiplication, "*" },
    };
  }
}