using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnyGameEngine.Modules.CustomVars {
	public class CustomArrayVar:CustomVar {
		public string [] Values;

		public CustomArrayVar (string [] values) {
			this.Values = values;
		}
	}
}
