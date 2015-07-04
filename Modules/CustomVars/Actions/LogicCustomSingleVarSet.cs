using AnyGameEngine.Modules.Core.Logic;
using AnyGameEngine.Modules.Expressions;
using System;
using System.Collections.Generic;
using System.Xml;

namespace AnyGameEngine.Modules.CustomVars.Logic.Actions {
	public class LogicCustomSingleVarSet:LogicNode {
		public string Name;
		public IEvaluate Value;

		public LogicCustomSingleVarSet () {
			
		}

		public LogicCustomSingleVarSet (XmlNode node, Dictionary <string, ExpressionConstructorInfo> expressionConstructorInfos):base (node) {
			this.Name = node.Attributes ["name"].Value;
			XmlAttribute text = node.Attributes ["value"];

			if (text != null) {
				if (node.ChildNodes.Count > 0) {
					throw new Exception ("Cannot define both text attribute and expression");
				}

				this.Value = new Constant (text.Value);
			} else {
				this.Value = new Expression (node.ChildNodes [0], expressionConstructorInfos);
			}
		}

		public override LogicNode Clone (LogicNode parent) {
			LogicCustomSingleVarSet clone = (LogicCustomSingleVarSet) base.Clone (parent);
			clone.Value = this.Value;
			return clone;
		}

		public override string ToString () {
			return string.Format ("CustomSingleVarSet, Value: {0}", this.Value);
		}
	}
}
