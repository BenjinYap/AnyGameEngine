

using System.Collections.Generic;
namespace AnyGameEngine.Modules.Expressions.Functions {
	public abstract class Function:ExpressionToken {
		public List <ExpressionToken> Tokens = new List <ExpressionToken> ();
	}
}
