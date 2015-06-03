using AnyGameEngine.Modules.Core.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace AnyGameEngine.Modules.GlobalResources.Logic.Actions {
	public class LogicGlobalResourceModify:LogicNode {
		public float Amount;

		public LogicGlobalResourceModify () {

		}

		public LogicGlobalResourceModify (XmlNode node):base (node) {
			this.Amount = float.Parse (node.Attributes ["amount"].Value);
		}

		public override LogicNode Clone (LogicNode parent) {
			LogicGlobalResourceModify clone = (LogicGlobalResourceModify) base.Clone (parent);
			clone.Amount = this.Amount;
			return clone;
		}

		public override string ToString () {
			return string.Format ("Currency Modify, Amount: {0}", this.Amount);
		}
	}
}
