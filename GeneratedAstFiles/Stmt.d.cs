using LoxSharp;
namespace StmtNamespace {
	public abstract class Stmt {
		public interface Visitor<R> {
			abstract R visitExpressionStmt(Expression stmt);
			abstract R visitPrintStmt(Print stmt);
		}
		public class Expression : Stmt {
			public Expression(ExprNamespace.Expr expression) {
				this.expression = expression;

			}
			public ExprNamespace.Expr expression;
			public override R accept<R>(Visitor<R> visitor) {
				return visitor.visitExpressionStmt(this);
			}
		}

		public class Print : Stmt {
			public Print(ExprNamespace.Expr expression) {
				this.expression = expression;

			}
			public ExprNamespace.Expr expression;
			public override R accept<R>(Visitor<R> visitor) {
				return visitor.visitPrintStmt(this);
			}
		}

	public abstract R accept<R>(Visitor<R> visitor);
	}
}
