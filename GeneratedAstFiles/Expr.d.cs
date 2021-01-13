using LoxSharp;
namespace Expr {
	abstract class Expr {
		class Binary : Expr {
			public Binary(Expr left, Token op, Expr right) {
				this.left = left;
				this.op = op;
				this.right = right;

			}
			Expr left;
			Token op;
			Expr right;
		}
		class Grouping : Expr {
			public Grouping(Expr expression) {
				this.expression = expression;

			}
			Expr expression;
		}
		class Literal : Expr {
			public Literal(object value) {
				this.value = value;

			}
			object value;
		}
		class Unary : Expr {
			public Unary(Token op, Expr right) {
				this.op = op;
				this.right = right;

			}
			Token op;
			Expr right;
		}
	}
}
