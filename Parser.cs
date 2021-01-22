using System.Collections.Generic;
using LoxSharp;
using ExprNamespace;

class Parser
{
    public class ParseError : System.Exception
    {
        public ParseError()
        {

        }
    }
    private List<Token> tokens;
    private int current = 0;
    Parser(List<Token> tokens)
    {
        this.tokens = tokens;
    }
    private Expr expression()
    {
        return equality();
    }

    private Expr equality()
    {
        Expr expr = comparison();

        while (match(new TokenType[] { TokenType.BANG_EQUAL, TokenType.EQUAL_EQUAL }))
        {
            Token op = previous();
            Expr right = comparison();
            expr = new Expr.Binary(expr, op, right);
        }

        return expr;
    }

    private bool match(TokenType[] types)
    {
        foreach (TokenType type in types)
        {
            if (check(type))
            {
                advance();
                return true;
            }
        }

        return false;
    }

    private bool check(TokenType type)
    {
        if (isAtEnd()) return false;
        return peek().type == type;
    }

    private Token advance()
    {
        if (!isAtEnd()) current++;
        return previous();
    }

    private bool isAtEnd()
    {
        return peek().type == TokenType.EOF;
    }

    private Token peek()
    {
        return tokens[current];
    }

    private Token previous()
    {
        return tokens[current - 1];
    }

    private Expr comparison()
    {
        Expr expr = term();

        while (match(new TokenType[] { TokenType.GREATER, TokenType.GREATER_EQUAL, TokenType.LESS, TokenType.LESS_EQUAL }))
        {
            Token op = previous();
            Expr right = term();
            expr = new Expr.Binary(expr, op, right);
        }

        return expr;
    }

    private Expr term()
    {
        Expr expr = factor();

        while (match(new TokenType[] { TokenType.MINUS, TokenType.PLUS }))
        {
            Token op = previous();
            Expr right = factor();
            expr = new Expr.Binary(expr, op, right);
        }

        return expr;
    }

    private Expr factor()
    {
        Expr expr = unary();

        while (match(new TokenType[] { TokenType.SLASH, TokenType.STAR }))
        {
            Token op = previous();
            Expr right = unary();
            expr = new Expr.Binary(expr, op, right);
        }

        return expr;
    }

    private Expr unary()
    {
        if (match(new TokenType[] { TokenType.BANG, TokenType.MINUS }))
        {
            Token op = previous();
            Expr right = unary();
            return new Expr.Unary(op, right);
        }

        return primary();
    }

    private Expr primary()
    {
        if (match(new TokenType[] { TokenType.FALSE })) return new Expr.Literal(TokenType.FALSE);
        if (match(new TokenType[] { TokenType.TRUE })) return new Expr.Literal(TokenType.TRUE);
        if (match(new TokenType[] { TokenType.NIL })) return new Expr.Literal(TokenType.NIL);

        if (match(new TokenType[] { TokenType.NUMBER, TokenType.STRING }))
        {
            return new Expr.Literal(previous().literal);
        }

        if (match(new TokenType[] { TokenType.LEFT_PAREN }))
        {
            Expr expr = expression();
            consume(TokenType.RIGHT_PAREN, "Expect ')' after expression.");
            return new Expr.Grouping(expr);
        }

        return null; // Will never reach here, but all code paths must return a value.
    }

    private Token consume(TokenType type, string message)
    {
        if (check(type)) return advance();

        throw error(peek(), message);
    }

    private ParseError error(Token token, string message)
    {
        LoxSharp.Program.error(token, message);
        return new ParseError();
    }
}