using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace AnyGameEngine.Modules.Core.Logic.Flow {
	public class LogicOption:LogicList {
		public string Text;

		public LogicOption () {

		}

		public LogicOption (XmlNode node):base (node) {
			this.Text = node.Attributes ["text"].Value;
		}

		public override LogicNode Clone (LogicNode parent) {
			LogicOption clone = (LogicOption) base.Clone (parent);
			clone.Text = this.Text;
			return clone;
		}

		public override string ToString () {
			return string.Format ("LogicOption, Text: {0}, Nodes: {1}", this.Text, this.Nodes.Count);
		}
	}
}
