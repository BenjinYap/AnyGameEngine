using AnyGameEngine.Entities.Logic.Flow;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Xml;

namespace AnyGameEngine.Entities.Logic {
	public abstract class LogicNode:Entity {
		public LogicNode Parent;
		public LogicNode Next;
		public LogicNode Prev;
		public List <LogicNode> Nodes;

		public LogicNode () {

		}

		public LogicNode (XmlNode node) {
			this.Nodes = new List <LogicNode> ();

			//create the child nodes
			for (var i = 0; i < node.ChildNodes.Count; i++) {
				LogicNode logic = GameStorage.CreateLogic (node.ChildNodes [i]);
				this.Nodes.Add (logic);

				if (i > 0) {
					this.Nodes [i].Prev = this.Nodes [i - 1];
					this.Nodes [i - 1].Next = this.Nodes [i];
				}
			}
		}

		public virtual LogicNode Clone (LogicNode parent) {
			LogicNode clone = (LogicNode) Activator.CreateInstance (Type.GetType (base.GetType ().AssemblyQualifiedName));
			clone.Parent = parent;
			clone.Nodes = new List <LogicNode> ();

			for (int i = 0; i < this.Nodes.Count; i++) {
				clone.Nodes.Add (this.Nodes [i].Clone (clone));

				if (i > 0) {
					clone.Nodes [i].Prev = clone.Nodes [i - 1];
					clone.Nodes [i].Next = clone.Nodes [i];
				}
			}
			
			return clone;
		}

		public LogicNode GetNextLogic () {
			//check all the cases for nulsl and stuff

			if (this.Next == null) {
				if (this.Parent is LogicLoop) {
					return this.Parent;
				} else if (this.Parent is LogicOption) {
					return this.Parent.Parent.Next;  //should this be using getnextlogic?
				} else {
					return this.Parent == null ? null : this.Parent.Next;
				}
			} else {
				return this.Next;
			}
		}

		public LogicNode GetParentByType (Type type) {
			LogicNode logic = this;

			while (true) {
				if (logic.Parent == null) {
					throw new Exception ("reached top without finding " + type.ToString ());
				} else {
					if (logic.Parent.GetType () == type) {
						return logic.Parent;
					}

					logic = logic.Parent;
				}
			}
		}
	}
}
