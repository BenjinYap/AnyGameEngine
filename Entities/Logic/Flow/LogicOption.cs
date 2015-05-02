using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace AnyGameEngine.Entities.Logic.Flow {
	public class LogicOption:LogicList {
		public string Text;

		public LogicOption (XmlNode node):base (node) {

		}

		public override LogicNode Clone (LogicNode parent) {
			LogicOption clone = (LogicOption) base.Clone (parent);
			clone.Text = this.Text;
			return clone;
		}
	}
}
