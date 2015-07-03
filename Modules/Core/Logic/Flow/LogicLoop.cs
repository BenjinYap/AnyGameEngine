using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace AnyGameEngine.Modules.Core.Logic.Flow {
	public class LogicLoop:LogicList {
		public int Repeat = 0;
		public int Count = 0;

		public LogicLoop () {

		}

		public LogicLoop (XmlNode node, CreateLogicVessel vessel):base (node, vessel) {
			this.Repeat = int.Parse (node.Attributes ["repeat"].Value);
		}

		public override LogicNode Clone (LogicNode parent) {
			LogicLoop clone = (LogicLoop) base.Clone (parent);
			clone.Repeat = this.Repeat;
			return clone;
		}

		public override string ToString () {
			return string.Format ("LogicLoop, Repeat: {1}, Nodes: {0}", this.Repeat, this.Nodes.Count);
		}
	}
}
