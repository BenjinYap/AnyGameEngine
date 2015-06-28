

using System;
namespace AnyGameEngine {
	public class LogicConstructorInfo {
		public Type Type;
		public bool ContainsExpressions;

		public LogicConstructorInfo (Type type, bool containsExpressions) {
			this.Type = type;
			this.ContainsExpressions = containsExpressions;
		}
	}
}
