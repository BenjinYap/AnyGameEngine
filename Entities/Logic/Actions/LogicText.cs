using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace AnyGameEngine.Entities.Logic.Actions {
	public class LogicText:LogicNode {
		public string Text;

		public LogicText () {

		}

		public LogicText (XmlNode node):base (node) {
			this.Text = node.Attributes ["text"].Value;
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
