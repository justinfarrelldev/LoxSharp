using LoxSharp;
namespace ExprNamespace {
	public abstract class Expr {
		public interface Visitor<R> {
			abstract R visitAssignExpr(Assign expr);
			abstract R visitBinaryExpr(Binary expr);
			abstract R visitGroupingExpr(Grouping expr);
			abstract R visitLiteralExpr(Literal expr);
			abstract R visitUnaryExpr(Unary expr);
			abstract R visitVariableExpr(Variable expr);
		}
		public class Assign : Expr {
			public Assign(Token name, Expr value) {
				this.name = name;
				this.value = value;

			}
			public Token name;
			public Expr value;
			public override R accept<R>(Visitor<R> visitor) {
				return visitor.visitAssignExpr(this);
			}
		}

		public class Binary : Expr {
			public Binary(Expr left, Token op, Expr right) {
				this.left = left;
				this.op = op;
				this.right = right;

			}
			public Expr left;
			public Token op;
			public Expr right;
			public override R accept<R>(Visitor<R> visitor) {
				return visitor.visitBinaryExpr(this);
			}
		}

		public class Grouping : Expr {
			public Grouping(Expr expression) {
				this.expression = expression;

			}
			public Expr expression;
			public override R accept<R>(Visitor<R> visitor) {
				return visitor.visitGroupingExpr(this);
			}
		}

		public class Literal : Expr {
			public Literal(object value) {
				this.value = value;

			}
			public object value;
			public override R accept<R>(Visitor<R> visitor) {
				return visitor.visitLiteralExpr(this);
			}
		}

		public class Unary : Expr {
			public Unary(Token op, Expr right) {
				this.op = op;
				this.right = right;

			}
			public Token op;
			public Expr right;
			public override R accept<R>(Visitor<R> visitor) {
				return visitor.visitUnaryExpr(this);
			}
		}

		public class Variable : Expr {
			public Variable(Token name) {
				this.name = name;

			}
			public Token name;
			public override R accept<R>(Visitor<R> visitor) {
				return visitor.visitVariableExpr(this);
			}
		}

	public abstract R accept<R>(Visitor<R> visitor);
	}
}
