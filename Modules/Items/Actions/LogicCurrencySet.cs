using AnyGameEngine.Entities.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace AnyGameEngine.Modules.Items.Actions {
	public class LogicCurrencySet:LogicNode {
		public float Amount;

		public LogicCurrencySet () {

		}

		public LogicCurrencySet (XmlNode node):base (node) {
			this.Amount = float.Parse (node.Attributes ["amount"].Value);
		}

		public override LogicNode Clone (LogicNode parent) {
			LogicCurrencySet clone = (LogicCurrencySet) base.Clone (parent);
			clone.Amount = this.Amount;
			return clone;
		}

		public override string ToString () {
			return string.Format ("Currency Set, Amount: {0}", this.Amount);
		}
	}
}
