
using ExprNamespace;
/*
class AstPrinter : Expr.Visitor<string>
{
    public string print(Expr expr)
    {
        return expr.accept(this);
    }
    public string visitBinaryExpr(Expr.Binary expr)
    {
        return parenthesize(expr.op.lexeme, new Expr[] { expr.left, expr.right });
    }

    public string visitGroupingExpr(Expr.Grouping expr)
    {
        return parenthesize("group", new Expr[] { expr.expression });
    }

    public string visitLiteralExpr(Expr.Literal expr)
    {
        if (expr.value == null) return "nil";
        return expr.value.ToString();
    }

    public string visitUnaryExpr(Expr.Unary expr)
    {
        return parenthesize(expr.op.lexeme, new Expr[] { expr.right });
    }

    private string parenthesize(string name, Expr[] exprs)
    {
        string returned = $"({name}";

        foreach (Expr expr in exprs)
        {
            returned += $" {expr.accept(this)}";
        }
        returned += ")";

        return returned;
    }

    public static void Test()
    {
        Expr expression = new Expr.Binary(
            new Expr.Unary(
                new LoxSharp.Token(LoxSharp.TokenType.MINUS, "-", null, 1),
                new Expr.Literal(123)
            ),
            new LoxSharp.Token(LoxSharp.TokenType.STAR, "*", null, 1),
            new Expr.Grouping(new Expr.Literal(45.67))
        );

        System.Console.WriteLine(new AstPrinter().print(expression));
    }
}*/