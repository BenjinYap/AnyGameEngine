using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Xml;

namespace AnyGameEngine.Modules.Expressions.Functions {
	public class Concatenate:Function {
		

		public Concatenate (XmlNode node) {
			
		}

		public override string Evaluate () {
			string s = "";
			this.Tokens.ForEach (a => s = s += a.Evaluate ());
			return s;
		}
	}
}
