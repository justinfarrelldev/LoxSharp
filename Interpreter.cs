using System.Collections.Generic;

class Interpreter : ExprNamespace.Expr.Visitor<object>,
                    StmtNamespace.Stmt.Visitor<object>
{
    private Environment environment = new Environment();
    public void interpret(List<StmtNamespace.Stmt> statements)
    {
        try
        {
            foreach (StmtNamespace.Stmt statement in statements)
            {
                execute(statement);
            }
        }
        catch (Errors.RuntimeError error)
        {
            LoxSharp.Program.runtimeError(error);
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

    public object visitVariableExpr(ExprNamespace.Expr.Variable expr)
    {
        return environment.get(expr.name);
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

    private void execute(StmtNamespace.Stmt stmt)
    {
        stmt.accept(this);
    }

    public object visitExpressionStmt(StmtNamespace.Stmt.Expression stmt)
    {
        evaluate(stmt.expression);
        return null;
    }

    public object visitPrintStmt(StmtNamespace.Stmt.Print stmt)
    {
        object value = evaluate(stmt.expression);
        System.Console.WriteLine(stringify(value));
        return null;
    }

    public object visitVarStmt(StmtNamespace.Stmt.Var stmt)
    {
        object value = null;
        if (stmt.initializer != null)
        {
            value = evaluate(stmt.initializer);
        }

        environment.define(stmt.name.lexeme, value);
        return null;
    }

    public object visitAssignExpr(ExprNamespace.Expr.Assign expr)
    {
        object value = evaluate(expr.value);
        environment.assign(expr.name, value);
        return value;
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