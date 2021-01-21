using LoxSharp;
namespace Expr {
	abstract class Expr {
		public interface Visitor<R> {
			R visitBinaryExpr(Binary expr);
			R visitGroupingExpr(Grouping expr);
			R visitLiteralExpr(Literal expr);
			R visitUnaryExpr(Unary expr);
		}
		public class Binary : Expr {
			public Binary(Expr left, Token op, Expr right) {
				this.left = left;
				this.op = op;
				this.right = right;

			}
			Expr left;
			Token op;
			Expr right;
			public override R accept<R>(Visitor<R> visitor) {
				return visitor.visitBinaryExpr(this);
			}
		}

		public class Grouping : Expr {
			public Grouping(Expr expression) {
				this.expression = expression;

			}
			Expr expression;
			public override R accept<R>(Visitor<R> visitor) {
				return visitor.visitGroupingExpr(this);
			}
		}

		public class Literal : Expr {
			public Literal(object value) {
				this.value = value;

			}
			object value;
			public override R accept<R>(Visitor<R> visitor) {
				return visitor.visitLiteralExpr(this);
			}
		}

		public class Unary : Expr {
			public Unary(Token op, Expr right) {
				this.op = op;
				this.right = right;

			}
			Token op;
			Expr right;
			public override R accept<R>(Visitor<R> visitor) {
				return visitor.visitUnaryExpr(this);
			}
		}

	public abstract R accept<R>(Visitor<R> visitor);
	}
}
