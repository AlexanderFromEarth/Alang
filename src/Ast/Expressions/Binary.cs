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
    StrictLess,
    StrictGreater,
    NotEqual,
    Less,
    Greater,
    Remainder,
  }
  class Binary : IExpression
  {
    public IExpression Left { get; }
    public BinaryOperator Operator { get; }
    public IExpression Right { get; }
    public int Position { get; }
    public Binary(int pos, IExpression left, BinaryOperator op, IExpression right) => (Position, Left, Operator, Right) = (pos, left, op, right);
    static readonly IReadOnlyDictionary<BinaryOperator, string> operators = new Dictionary<BinaryOperator, string> {
      { BinaryOperator.Pipe, "|>" },
      { BinaryOperator.Or, "||" },
      { BinaryOperator.And, "&&" },
      { BinaryOperator.Equal, "==" },
      { BinaryOperator.NotEqual, "!=" },
      { BinaryOperator.Less, "<=" },
      { BinaryOperator.Greater, ">=" },
      { BinaryOperator.StrictLess, "<" },
      { BinaryOperator.StrictGreater, ">" },
      { BinaryOperator.Addition, "+" },
      { BinaryOperator.Subtraction, "-" },
      { BinaryOperator.Multiplication, "*" },
      { BinaryOperator.Division, "/" },
      { BinaryOperator.Remainder, "%" },
    };
    public string FormattedString => $"{Left.FormattedString} {operators[Operator]} {Right.FormattedString}";
  }
}