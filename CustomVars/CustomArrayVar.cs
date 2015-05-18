using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnyGameEngine.CustomVars {
	public class CustomArrayVar <T>:CustomVar {
		public T [] Values;

		public CustomArrayVar (T [] values) {
			this.Values = values;
		}
	}
}
