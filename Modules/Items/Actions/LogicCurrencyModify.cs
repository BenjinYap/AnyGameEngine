using AnyGameEngine.Entities.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace AnyGameEngine.Modules.Items.Actions {
	public class LogicCurrencyModify:LogicNode {
		public float Amount;

		public LogicCurrencyModify () {

		}

		public LogicCurrencyModify (XmlNode node):base (node) {
			this.Amount = float.Parse (node.Attributes ["amount"].Value);
		}

		public override LogicNode Clone (LogicNode parent) {
			LogicCurrencyModify clone = (LogicCurrencyModify) base.Clone (parent);
			clone.Amount = this.Amount;
			return clone;
		}

		public override string ToString () {
			return string.Format ("Currency Modify, Amount: {0}", this.Amount);
		}
	}
}
