
using Expr;

class AstPrinter : Expr.Expr.Visitor<string>
{
    public string print(Expr.Expr expr)
    {
        return expr.accept(this);
    }
    public string visitBinaryExpr(Expr.Expr.Binary expr)
    {
        return parenthesize(expr.op.lexeme, new Expr.Expr[] { expr.left, expr.right });
    }

    public string visitGroupingExpr(Expr.Expr.Grouping expr)
    {
        return parenthesize("group", new Expr.Expr[] { expr.expression });
    }

    public string visitLiteralExpr(Expr.Expr.Literal expr)
    {
        if (expr.value == null) return "nil";
        return expr.value.ToString();
    }

    public string visitUnaryExpr(Expr.Expr.Unary expr)
    {
        return parenthesize(expr.op.lexeme, new Expr.Expr[] { expr.right });
    }

    private string parenthesize(string name, Expr.Expr[] exprs)
    {
        string returned = $"({name}";

        foreach (Expr.Expr expr in exprs)
        {
            returned += $" {expr.accept(this)}";
        }
        returned += ")";

        return returned;
    }

    public static void Test()
    {
        Expr.Expr expression = new Expr.Expr.Binary(
            new Expr.Expr.Unary(
                new LoxSharp.Token(LoxSharp.TokenType.MINUS, "-", null, 1),
                new Expr.Expr.Literal(123)
            ),
            new LoxSharp.Token(LoxSharp.TokenType.STAR, "*", null, 1),
            new Expr.Expr.Grouping(new Expr.Expr.Literal(45.67))
        );

        System.Console.WriteLine(new AstPrinter().print(expression));
    }
}