using AnyGameEngine.Modules.Expressions;
using System;
using System.Collections.Generic;
using System.Xml;

namespace AnyGameEngine.Modules.Core.Logic.Actions {
	public class LogicText:LogicNode {
		public IEvaluate Text;

		public LogicText () {
			
		}

		public LogicText (XmlNode node, Dictionary <string, ExpressionConstructorInfo> expressionConstructorInfos):base (node) {
			XmlAttribute text = node.Attributes ["text"];

			if (text != null) {
				if (node.ChildNodes.Count > 0) {
					throw new Exception ("Cannot define both text attribute and expression");
				}

				this.Text = new Constant (text.Value);
			} else {
				this.Text = new Expression (node.ChildNodes [0], expressionConstructorInfos);
			}
		}

		public override LogicNode Clone (LogicNode parent) {
			LogicText clone = (LogicText) base.Clone (parent);
			clone.Text = this.Text;
			return clone;
		}

		public override string ToString () {
			return string.Format ("Text, Text: {0}", this.Text);
		}
	}
}
