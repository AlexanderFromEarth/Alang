using System;
using System.Collections.Generic;
using System.Linq;
using Lang.Ast;
using Lang.Ast.BaseNodes;
using Lang.Ast.Expressions;
using Lang.Ast.Statements;
using Lang.Utils;

namespace Lang.Parsing
{
  class Parser
  {
    int LastIndex { get; set; } = 0;
    SourceFile SourceFile { get; }
    IReadOnlyList<Token> Tokens { get; }
    Token CurrentToken => Tokens[LastIndex];
    int CurrentPosition => CurrentToken.Position;
    Parser(SourceFile src, IReadOnlyList<Token> tokens) => (SourceFile, Tokens) = (src, tokens);
    static bool IsNotWhitespace(Token token)
    {
      switch (token.Type)
      {
        case TokenType.Whitespaces:
        case TokenType.SingleLineComment:
          return false;
        default:
          return true;
      }
    }
    Exception MakeError(string msg) => new Exception(SourceFile.MakeErrorMessage(CurrentPosition, msg));
    void ReadNext() => LastIndex += 1;
    void Reset() => LastIndex = 0;
    bool SkipIf(string lexeme)
    {
      if (CurrentToken.Lexeme == lexeme)
      {
        ReadNext();
        return true;
      }
      return false;
    }
    void Expect(string lexeme)
    {
      if (!SkipIf(lexeme))
      {
        throw MakeError($"Expected '{lexeme}', but get {CurrentToken}");
      }
    }
    void ExpectEof()
    {
      if (CurrentToken.Type != TokenType.EnfOfFile)
      {
        throw MakeError($"Left {CurrentToken}");
      }
    }
    public static ProgramNode Parse(SourceFile src) => new Parser(src, Lexer.GetTokens(src).Concat(new Token(src.Text.Length, TokenType.EnfOfFile, "").Yield()).Where(IsNotWhitespace).ToList()).ParseProgram();

    ProgramNode ParseProgram()
    {
      Reset();
      var statements = new List<IStatement>();
      while (CurrentToken.Type != TokenType.EnfOfFile)
      {
        statements.Add(ParseStatement());
      }
      var res = new ProgramNode(SourceFile, statements);
      ExpectEof();
      return res;
    }

    IStatement ParseStatement()
    {
      var leftExpr = ParseExpression();
      if (SkipIf("="))
      {
        if (!(leftExpr is Identifier id))
        {
          throw MakeError("Declaration not in variable");
        }
        var rightExpr = ParseExpression();
        SkipIf(";");
        return new Declaration(id.Name, rightExpr);
      }
      else
      {
        SkipIf(";");
        return new ExpressionStatement(leftExpr);
      }
    }

    IExpression ParseExpression()
    {
      return ParseOrExpression();
    }

    IExpression ParsePipelineExpression()
    {
      var left = ParseOrExpression();
      while (true)
      {
        var pos = CurrentPosition;
        if (SkipIf("|>"))
        {
          var right = ParseOrExpression();
          left = new Binary(pos, left, BinaryOperator.Pipe, right);
        }
        else
        {
          break;
        }
      }
      return left;
    }
    IExpression ParseOrExpression()
    {
      var left = ParseAndExpression();
      while (true)
      {
        var pos = CurrentPosition;
        if (SkipIf("||"))
        {
          var right = ParseAndExpression();
          left = new Binary(pos, left, BinaryOperator.Or, right);
        }
        else
        {
          break;
        }
      }
      return left;
    }
    IExpression ParseAndExpression()
    {
      var left = ParseEqualExpression();
      while (true)
      {
        var pos = CurrentPosition;
        if (SkipIf("&&"))
        {
          var right = ParseEqualExpression();
          left = new Binary(pos, left, BinaryOperator.And, right);
        }
        else
        {
          break;
        }
      }
      return left;
    }

    IExpression ParseEqualExpression()
    {
      var left = ParseRelationalExpression();
      while (true)
      {
        var pos = CurrentPosition;
        if (SkipIf("=="))
        {
          var right = ParseRelationalExpression();
          left = new Binary(pos, left, BinaryOperator.Equal, right);
        }
        else if (SkipIf("!="))
        {
          var right = ParseRelationalExpression();
          left = new Binary(pos, left, BinaryOperator.NotEqual, right);
        }
        else
        {
          break;
        }
      }
      return left;
    }
    IExpression ParseRelationalExpression()
    {
      var left = ParseAdditiveExpression();
      while (true)
      {
        var pos = CurrentPosition;
        if (SkipIf("<"))
        {
          var right = ParseAdditiveExpression();
          left = new Binary(pos, left, BinaryOperator.StrictLess, right);
        }
        else if (SkipIf(">"))
        {
          var right = ParseAdditiveExpression();
          left = new Binary(pos, left, BinaryOperator.StrictGreater, right);
        }
        else if (SkipIf("<="))
        {
          var right = ParseAdditiveExpression();
          left = new Binary(pos, left, BinaryOperator.Less, right);
        }
        else if (SkipIf(">="))
        {
          var right = ParseAdditiveExpression();
          left = new Binary(pos, left, BinaryOperator.Greater, right);
        }
        else
        {
          break;
        }
      }
      return left;
    }

    IExpression ParseAdditiveExpression()
    {
      var left = ParseMultiplicativeExpression();
      while (true)
      {
        var pos = CurrentPosition;
        if (SkipIf("+"))
        {
          var right = ParseMultiplicativeExpression();
          left = new Binary(pos, left, BinaryOperator.Addition, right);
        }
        else if (SkipIf("-"))
        {
          var right = ParseMultiplicativeExpression();
          left = new Binary(pos, left, BinaryOperator.Subtraction, right);
        }
        else
        {
          break;
        }
      }
      return left;
    }

    IExpression ParseMultiplicativeExpression()
    {
      var left = ParsePrimary();
      while (true)
      {
        var pos = CurrentPosition;
        if (SkipIf("*"))
        {
          var right = ParsePrimary();
          left = new Binary(pos, left, BinaryOperator.Multiplication, right);
        }
        else if (SkipIf("/"))
        {
          var right = ParsePrimary();
          left = new Binary(pos, left, BinaryOperator.Division, right);
        }
        else if (SkipIf("%"))
        {
          var right = ParsePrimary();
          left = new Binary(pos, left, BinaryOperator.Remainder, right);
        }
        else
        {
          break;
        }
      }
      return left;
    }

    IExpression ParsePrimary()
    {
      var expr = ParsePrimitive();
      while (true)
      {
        var pos = CurrentPosition;
        if (SkipIf("("))
        {
          var args = new List<IExpression>();
          if (!SkipIf(")"))
          {
            args.Add(ParseExpression());
            while (SkipIf(","))
            {
              args.Add(ParseExpression());
            }
            Expect(")");
          }
          expr = new Call(pos, expr, args);
        }
        else
        {
          break;
        }
      }
      return expr;
    }

    IExpression ParsePrimitive()
    {
      var pos = CurrentPosition;
      if (CurrentToken.Type == TokenType.NumberLiteral)
      {
        var lexeme = CurrentToken.Lexeme;
        ReadNext();
        return new Number(pos, lexeme);
      }
      if (CurrentToken.Type == TokenType.Identifier)
      {
        var lexeme = CurrentToken.Lexeme;
        ReadNext();
        return new Identifier(pos, lexeme);
      }
      if (SkipIf("("))
      {
        var parentheses = new Parentheses(pos, ParseExpression());
        Expect(")");
        return parentheses;
      }
      throw MakeError($"Expected identifier, number or parentheses, but get {CurrentToken}");
    }
  }
}