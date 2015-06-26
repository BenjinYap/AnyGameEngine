using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace AnyGameEngine.Modules.Expressions {
	public abstract class ExpressionToken:IEvaluate {

		public abstract string Evaluate ();

		public static ExpressionToken FromXml (XmlNode node) {
			return (ExpressionToken) Activator.CreateInstance  (ExpressionToken.Types [node.Name], new object [] {node});
		}

		public static Dictionary <string, Type> Types = new Dictionary <string, Type> ();
	}
}
