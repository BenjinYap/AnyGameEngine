using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace AnyGameEngine.Modules.Core.Logic.Flow {
	public class LogicPointer:LogicNode {
		public string LogicId;

		public LogicPointer () {

		}

		public LogicPointer (XmlNode node):base (node) {
			this.LogicId = node.Attributes ["logicId"].Value;
		}

		public override LogicNode Clone (LogicNode parent) {
			LogicPointer clone = (LogicPointer) base.Clone (parent);
			clone.LogicId = this.LogicId;
			return clone;
		}
	}
}
