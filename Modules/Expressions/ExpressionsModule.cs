

using AnyGameEngine.Modules.Expressions.Functions;
namespace AnyGameEngine.Modules.Expressions {
	public class ExpressionsModule:Module {
		
		public ExpressionsModule (Overlord overlord):base (overlord) {
			
		}

		public override void RegisterExpressionConstructors (Overlord overlord) {
			overlord.ExpressionConstructorInfos.Add ("Constant", new ExpressionConstructorInfo (typeof (Constant)));

			overlord.ExpressionConstructorInfos.Add ("Concatenate", new ExpressionConstructorInfo (typeof (Concatenate)));
		}
	}
}
