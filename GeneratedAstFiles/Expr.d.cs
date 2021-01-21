using LoxSharp;
namespace Expr
{
    public abstract class Expr
    {
        public interface Visitor<R>
        {
            public abstract R visitBinaryExpr(Binary expr);
            public abstract R visitGroupingExpr(Grouping expr);
            public abstract R visitLiteralExpr(Literal expr);
            public abstract R visitUnaryExpr(Unary expr);
        }
        public class Binary : Expr
        {
            public Binary(Expr left, Token op, Expr right)
            {
                this.left = left;
                this.op = op;
                this.right = right;

            }
            public Expr left;
            public Token op;
            public Expr right;
            public override R accept<R>(Visitor<R> visitor)
            {
                return visitor.visitBinaryExpr(this);
            }
        }

        public class Grouping : Expr
        {
            public Grouping(Expr expression)
            {
                this.expression = expression;

            }
            public Expr expression;
            public override R accept<R>(Visitor<R> visitor)
            {
                return visitor.visitGroupingExpr(this);
            }
        }

        public class Literal : Expr
        {
            public Literal(object value)
            {
                this.value = value;

            }
            public object value;
            public override R accept<R>(Visitor<R> visitor)
            {
                return visitor.visitLiteralExpr(this);
            }
        }

        public class Unary : Expr
        {
            public Unary(Token op, Expr right)
            {
                this.op = op;
                this.right = right;

            }
            public Token op;
            public Expr right;
            public override R accept<R>(Visitor<R> visitor)
            {
                return visitor.visitUnaryExpr(this);
            }
        }

        public abstract R accept<R>(Visitor<R> visitor);
    }
}
