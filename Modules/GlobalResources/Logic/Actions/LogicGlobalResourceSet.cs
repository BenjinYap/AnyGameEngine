using AnyGameEngine.Modules.Core.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace AnyGameEngine.Modules.GlobalResources.Logic.Actions {
	public class LogicGlobalResourceSet:LogicNode {
		public float Amount;

		public LogicGlobalResourceSet () {

		}

		public LogicGlobalResourceSet (XmlNode node):base (node) {
			this.Amount = float.Parse (node.Attributes ["amount"].Value);
		}

		public override LogicNode Clone (LogicNode parent) {
			LogicGlobalResourceSet clone = (LogicGlobalResourceSet) base.Clone (parent);
			clone.Amount = this.Amount;
			return clone;
		}

		public override string ToString () {
			return string.Format ("Currency Set, Amount: {0}", this.Amount);
		}
	}
}
