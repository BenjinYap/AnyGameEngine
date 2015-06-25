using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnyGameEngine.Modules.Expressions {
	public class Constant <T>:ExpressionToken, IEvaluate <T> {
		private T value;

		public Constant (T value) {
			this.value = value;
		}

		public T Evaluate () {
			return this.value;
		}
	}
}
