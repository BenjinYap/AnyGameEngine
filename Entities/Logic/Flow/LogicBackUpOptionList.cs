using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace AnyGameEngine.Entities.Logic.Flow {
	public class LogicBackUpOptionList:LogicNode {
		public int Times = 0;

		public LogicBackUpOptionList () {

		}

		public LogicBackUpOptionList (XmlNode node):base (node) {

		}

		public override LogicNode Clone (LogicNode parent) {
			LogicBackUpOptionList clone = (LogicBackUpOptionList) base.Clone (parent);
			clone.Times = this.Times;
			return clone;
		}
	}
}
