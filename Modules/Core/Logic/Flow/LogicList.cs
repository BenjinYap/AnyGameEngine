using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace AnyGameEngine.Modules.Core.Logic.Flow {
	public class LogicList:LogicNode {

		public LogicList () {

		}

		public LogicList (XmlNode node, CreateLogicVessel vessel):base (node) {
			//create the child nodes
			for (int i = 0; i < node.ChildNodes.Count; i++) {
				LogicNode childLogic = vessel.CreateLogic (node.ChildNodes [i]);
				childLogic.Parent = this;
				this.Nodes.Add (childLogic);
				
				if (i > 0) {
					this.Nodes [i].Prev = this.Nodes [i - 1];
					this.Nodes [i - 1].Next = this.Nodes [i];
				}
			}
		}

		public override LogicNode Clone (LogicNode parent) {
			LogicNode clone = (LogicList) base.Clone (parent);
			clone.Nodes = new List <LogicNode> ();

			for (int i = 0; i < this.Nodes.Count; i++) {
				clone.Nodes.Add (this.Nodes [i].Clone (clone));

				if (i > 0) {
					clone.Nodes [i].Prev = clone.Nodes [i - 1];
					clone.Nodes [i - 1].Next = clone.Nodes [i];
				}
			}
			
			return clone;
		}

		public override string ToString () {
			return string.Format ("LogicList, Nodes: {0}", this.Nodes.Count);
		}
	}
}
