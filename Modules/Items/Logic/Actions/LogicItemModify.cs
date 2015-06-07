using AnyGameEngine.Modules.Core.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace AnyGameEngine.Modules.Items.Logic.Actions {
	public class LogicItemModify:LogicNode {
		public string ItemId;
		public int Quantity;

		public LogicItemModify () {

		}

		public LogicItemModify (XmlNode node):base (node) {
			this.ItemId = node.Attributes ["itemId"].Value;
			this.Quantity = int.Parse (node.Attributes ["quantity"].Value);
		}

		public override LogicNode Clone (LogicNode parent) {
			LogicItemModify clone = (LogicItemModify) base.Clone (parent);
			clone.ItemId = this.ItemId;
			clone.Quantity = this.Quantity;
			return clone;
		}

		public override string ToString () {
			return string.Format ("Item Add, Item: {0}, Quantity: {1}", this.ItemId, this.Quantity);
		}
	}
}
