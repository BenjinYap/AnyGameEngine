using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnyGameEngine.Modules.CustomVars {
	public class CustomSingleVar <T>:CustomVar {
		public T Value;

		public CustomSingleVar (T value) {
			this.Value = value;
		}
	}
}
