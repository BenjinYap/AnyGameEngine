

using System;
namespace AnyGameEngine {
	public class ExpressionConstructorInfo {
		public Type Type;
		public bool NeedsGameAndSave;

		public ExpressionConstructorInfo (Type type, bool needsGameAndSave) {
			this.Type = type;
			this.NeedsGameAndSave = needsGameAndSave;
		}
	}
}
