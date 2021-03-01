class Interpreter : ExprNamespace.Expr.Visitor<object>
{
    public void interpret(ExprNamespace.Expr expression)
    {
        try
        {
            object value = evaluate(expression);
            System.Console.WriteLine(stringify(value));
        }
        catch (Errors.RuntimeError error)
        {

        }
    }
    public object visitLiteralExpr(ExprNamespace.Expr.Literal expr)
    {
        return expr.value;
    }

    public object visitUnaryExpr(ExprNamespace.Expr.Unary expr)
    {
        object right = evaluate(expr.right);

        switch (expr.op.type)
        {
            case LoxSharp.TokenType.BANG:
                return !isTruthy(right);
            case LoxSharp.TokenType.MINUS:
                checkNumberOperand(expr.op, right);
                return -(double)right;
        }

        return null; // Unreachable
    }

    private void checkNumberOperand(LoxSharp.Token op, object operand)
    {
        if (operand.GetType() == typeof(double)) return;
        throw new Errors.RuntimeError(op, "Operand must be a number.");
    }

    private void checkNumberOperands(LoxSharp.Token op, object left, object right)
    {
        if (left.GetType() == typeof(double) && right.GetType() == typeof(double)) return;

        throw new Errors.RuntimeError(op, "Operands must be numbers.");
    }

    private bool isTruthy(object obj)
    {
        if (obj == null) return false;
        if (obj.GetType() == typeof(bool)) return (bool)obj;
        return true;
    }

    private bool isEqual(object a, object b)
    {
        if (a == null && b == null) return true;
        if (a == null) return false;

        return a.Equals(b);
    }

    private string stringify(object obj)
    {
        if (obj == null) return "nil";

        if (obj.GetType() == typeof(double))
        {
            string text = obj.ToString();
            if (text.EndsWith(".0"))
            {
                text = text.Substring(0, text.Length - 3);
            }
            return text;
        }

        return obj.ToString();
    }

    public object visitGroupingExpr(ExprNamespace.Expr.Grouping expr)
    {
        return evaluate(expr.expression);
    }

    private object evaluate(ExprNamespace.Expr expr)
    {
        return expr.accept(this);
    }

    public object visitBinaryExpr(ExprNamespace.Expr.Binary expr)
    {
        object left = evaluate(expr.left);
        object right = evaluate(expr.right);

        switch (expr.op.type)
        {
            case LoxSharp.TokenType.GREATER:
                checkNumberOperands(expr.op, left, right);
                return (double)left > (double)right;
            case LoxSharp.TokenType.GREATER_EQUAL:
                checkNumberOperands(expr.op, left, right);
                return (double)left >= (double)right;
            case LoxSharp.TokenType.LESS:
                checkNumberOperands(expr.op, left, right);
                return (double)left < (double)right;
            case LoxSharp.TokenType.LESS_EQUAL:
                checkNumberOperands(expr.op, left, right);
                return (double)left <= (double)right;
            case LoxSharp.TokenType.MINUS:
                checkNumberOperands(expr.op, left, right);
                return (double)left - (double)right;
            case LoxSharp.TokenType.PLUS:
                if (left.GetType() == typeof(double) && right.GetType() == typeof(double))
                {
                    return (double)left + (double)right;
                }
                if (left.GetType() == typeof(string) && right.GetType() == typeof(string))
                {
                    return (string)left + (string)right;
                }
                throw new Errors.RuntimeError(expr.op, "Operands must be two numbers or two strings.");
            case LoxSharp.TokenType.SLASH:
                checkNumberOperands(expr.op, left, right);
                return (double)left / (double)right;
            case LoxSharp.TokenType.STAR:
                checkNumberOperands(expr.op, left, right);
                return (double)left * (double)right;
            case LoxSharp.TokenType.BANG:
                return !isEqual(left, right);
            case LoxSharp.TokenType.BANG_EQUAL:
                return !isEqual(left, right);
        }

        return null; // Unreachable
    }
}