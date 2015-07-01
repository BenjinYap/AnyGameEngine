using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnyGameEngine.Modules.CustomVars {
	public class CustomSingleVar:CustomVar {
		public string Value;

		public CustomSingleVar (string value) {
			this.Value = value;
		}
	}
}
