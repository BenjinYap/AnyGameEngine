

using AnyGameEngine.Modules.Expressions.Functions;
namespace AnyGameEngine.Modules.Expressions {
	public class ExpressionsModule:Module {
		
		public ExpressionsModule (Overlord overlord):base (overlord) {

		}

		static ExpressionsModule () {
			ExpressionToken.Types.Add ("Constant", typeof (Constant));

			ExpressionToken.Types.Add ("Concatenate", typeof (Concatenate));
		}
	}
}
