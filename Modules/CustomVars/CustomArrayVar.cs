using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnyGameEngine.Modules.CustomVars {
	public class CustomArrayVar:CustomVar {
		public string [] Values;

		public CustomArrayVar (string name):base (name) {

		}

		public CustomArrayVar (string name, string [] values):base (name) {
			this.Values = values;
		}

		public override CustomVar Clone () {
			CustomArrayVar clone = (CustomArrayVar) base.Clone ();
			clone.Values = this.Values;
			return clone;
		}
	}
}
