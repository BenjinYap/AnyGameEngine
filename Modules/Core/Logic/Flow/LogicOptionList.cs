using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace AnyGameEngine.Entities.Logic.Flow {
	public class LogicOptionList:LogicList {
		public string Text;

		public LogicOptionList () {

		}

		public LogicOptionList (XmlNode node):base (node) {
			this.Text = node.Attributes ["text"].Value;
		}

		public override LogicNode Clone (LogicNode parent) {
			LogicOptionList clone = (LogicOptionList) base.Clone (parent);
			clone.Text = this.Text;
			return clone;
		}

		public override string ToString () {
			return string.Format ("LogicOptionList, Text: {0}, Nodes: {1}", this.Text, this.Nodes.Count);
		}
	}
}
