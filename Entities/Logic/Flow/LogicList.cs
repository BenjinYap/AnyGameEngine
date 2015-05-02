using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace AnyGameEngine.Entities.Logic.Flow {
	public class LogicList:LogicNode {

		public LogicList (XmlNode node):base (node) {

		}

		public override string ToString () {
			return string.Format ("LogicList, Nodes: {0}", this.Nodes.Count);
		}
	}
}
