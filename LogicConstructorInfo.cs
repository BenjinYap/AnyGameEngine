

using System;
namespace AnyGameEngine {
	public class LogicConstructorInfo {
		public Type Type;
		public bool ContainsLogic;
		public bool ContainsExpressions;

		public LogicConstructorInfo (Type type, bool containsLogic, bool containsExpressions) {
			this.Type = type;
			this.ContainsLogic = containsLogic;
			this.ContainsExpressions = containsExpressions;
		}
	}
}
