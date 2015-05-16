using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace AnyGameEngine.Entities.Logic.Flow {
	public class LogicLoop:LogicList {
		public int Repeat = 0;

		public LogicLoop () {

		}

		public LogicLoop (XmlNode node):base (node) {

		}

		public override LogicNode Clone (LogicNode parent) {
			LogicLoop clone = (LogicLoop) base.Clone (parent);
			clone.Repeat = this.Repeat;
			return clone;
		}
	}
}
