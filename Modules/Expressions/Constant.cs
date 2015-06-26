using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace AnyGameEngine.Modules.Expressions {
	public class Constant:ExpressionToken {
		private string value;

		public Constant (string value) {
			this.value = value;
		}

		public Constant (XmlNode node) {
			this.value = node.Attributes ["value"].Value;
		}

		public override string Evaluate () {
			return this.value;
		}
	}
}
