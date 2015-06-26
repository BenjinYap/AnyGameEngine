using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Xml;

namespace AnyGameEngine.Modules.Expressions {
	public class Expression:IEvaluate {
		private List <ExpressionToken> tokens = new List <ExpressionToken> ();

		public Expression () {
			
		}

		public Expression (XmlNode node) {
			for (int i = 0; i < node.ChildNodes.Count; i++) {
				tokens.Add (ExpressionToken.FromXml (node.ChildNodes [i]));
			}
		}

		public string Evaluate () {
			return tokens [0].Evaluate ();
		}
	}
}
