using LoxSharp;
namespace StmtNamespace {
	public abstract class Stmt {
		public interface Visitor<R> {
			abstract R visitBlockStmt(Block stmt);
			abstract R visitExpressionStmt(Expression stmt);
			abstract R visitPrintStmt(Print stmt);
			abstract R visitVarStmt(Var stmt);
		}
		public class Block : Stmt {
			public Block(System.Collections.Generic.List<Stmt> statements) {
				this.statements = statements;

			}
			public System.Collections.Generic.List<Stmt> statements;
			public override R accept<R>(Visitor<R> visitor) {
				return visitor.visitBlockStmt(this);
			}
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

		public class Var : Stmt {
			public Var(Token name, ExprNamespace.Expr initializer) {
				this.name = name;
				this.initializer = initializer;

			}
			public Token name;
			public ExprNamespace.Expr initializer;
			public override R accept<R>(Visitor<R> visitor) {
				return visitor.visitVarStmt(this);
			}
		}

	public abstract R accept<R>(Visitor<R> visitor);
	}
}
