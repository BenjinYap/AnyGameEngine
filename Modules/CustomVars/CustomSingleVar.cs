using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnyGameEngine.Modules.CustomVars {
	public class CustomSingleVar:CustomVar {
		public string Value;

		public CustomSingleVar (string name):base (name) {

		}

		public CustomSingleVar (string name, string value):base (name) {
			this.Value = value;
		}

		public override CustomVar Clone () {
			CustomSingleVar clone = (CustomSingleVar) base.Clone ();
			clone.Value = this.Value;
			return clone;
		}
	}
}
