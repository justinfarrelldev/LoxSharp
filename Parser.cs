using System.Collections.Generic;
using LoxSharp;
using ExprNamespace;

public class Parser
{
    public class ParseError : System.Exception
    {
        public ParseError()
        {

        }
    }
    private List<Token> tokens;
    private int current = 0;
    public Parser(List<Token> tokens)
    {
        this.tokens = tokens;
    }


    public List<StmtNamespace.Stmt> parse()
    {
        List<StmtNamespace.Stmt> statements = new List<StmtNamespace.Stmt>();
        while (!isAtEnd())
        {
            statements.Add(declaration());
        }

        return statements;
    }

    private Expr expression()
    {
        return assignment();
    }

    private StmtNamespace.Stmt declaration()
    {
        try
        {
            if (match(new TokenType[] { TokenType.VAR })) return varDeclaration();

            return statement();
        }
        catch (ParseError error)
        {
            synchronize();
            return null;
        }
    }

    private StmtNamespace.Stmt statement()
    {
        if (match(new TokenType[] { TokenType.PRINT })) return printStatement();

        return expressionStatement();
    }

    private StmtNamespace.Stmt printStatement()
    {
        Expr value = expression();
        consume(TokenType.SEMICOLON, "Expect ';' after value.");
        return new StmtNamespace.Stmt.Print(value);
    }

    private StmtNamespace.Stmt varDeclaration()
    {
        Token name = consume(TokenType.IDENTIFIER, "Expect variable name.");

        Expr initializer = null;
        if (match(new TokenType[] { TokenType.EQUAL }))
        {
            initializer = expression();
        }

        consume(TokenType.SEMICOLON, "Expect ';' after variable declaration.");
        return new StmtNamespace.Stmt.Var(name, initializer);
    }

    private StmtNamespace.Stmt expressionStatement()
    {
        Expr expr = expression();
        consume(TokenType.SEMICOLON, "Expect ';' after expression.");
        return new StmtNamespace.Stmt.Expression(expr);
    }

    private Expr assignment()
    {
        Expr expr = equality();

        if (match(new TokenType[] { TokenType.EQUAL }))
        {
            Token equals = previous();
            Expr value = assignment();

            if (expr.GetType() == typeof(Expr.Variable))
            {
                Token name = ((Expr.Variable)expr).name;
                return new Expr.Assign(name, value);
            }

            error(equals, "Invalid assignment target.");
        }

        return expr;
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

        if (match(new TokenType[] { TokenType.IDENTIFIER }))
        {
            return new Expr.Variable(previous());
        }

        if (match(new TokenType[] { TokenType.LEFT_PAREN }))
        {
            Expr expr = expression();
            consume(TokenType.RIGHT_PAREN, "Expect ')' after expression.");
            return new Expr.Grouping(expr);
        }

        throw error(peek(), "Expect expression.");
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

    private void synchronize()
    {
        advance();

        while (!isAtEnd())
        {
            if (previous().type == TokenType.SEMICOLON) return;

            switch (peek().type)
            {
                case TokenType.CLASS:
                    break;
                case TokenType.FUN:
                    break;
                case TokenType.VAR:
                    break;
                case TokenType.FOR:
                    break;
                case TokenType.IF:
                    break;
                case TokenType.WHILE:
                    break;
                case TokenType.PRINT:
                    break;
                case TokenType.RETURN:
                    return;
            }

            advance();
        }
    }
}