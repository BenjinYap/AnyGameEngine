using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace AnyGameEngine.Modules.Core.Logic.Flow {
	public class LogicBackUpOptionList:LogicNode {
		public int Times = 0;

		public LogicBackUpOptionList () {

		}

		public LogicBackUpOptionList (XmlNode node):base (node) {
			this.Times = int.Parse (node.Attributes ["times"].Value);
		}

		public override LogicNode Clone (LogicNode parent) {
			LogicBackUpOptionList clone = (LogicBackUpOptionList) base.Clone (parent);
			clone.Times = this.Times;
			return clone;
		}
	}
}
