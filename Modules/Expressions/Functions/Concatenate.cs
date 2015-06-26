using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Xml;

namespace AnyGameEngine.Modules.Expressions.Functions {
	public class Concatenate:Function {
		private List <ExpressionToken> tokens = new List <ExpressionToken> ();

		public Concatenate (XmlNode node) {
			for (int i = 0; i < node.ChildNodes.Count; i++) {
				this.tokens.Add (ExpressionToken.FromXml (node.ChildNodes [i]));
			}
		}

		public override string Evaluate () {
			string s = "";
			this.tokens.ForEach (a => s = s += a.Evaluate ());
			return s;
		}
	}
}
