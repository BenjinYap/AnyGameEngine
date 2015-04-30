using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnyGameEngine.Entities.Logic {
	public abstract class LogicNode:Entity {
		public LogicNode Parent;
		public LogicNode Next;
		public LogicNode Prev;
		public List <LogicNode> Nodes;

		public virtual LogicNode Clone (LogicNode parent) {
			LogicNode clone = (LogicNode) Activator.CreateInstance (Type.GetType (base.GetType ().AssemblyQualifiedName));
			clone.Parent = parent;

			for (int i = 0; i < this.Nodes.Count; i++) {
				clone.Nodes.Add (this.Nodes [i].Clone (clone));

				if (i > 0) {
					clone.Nodes [i].Prev = clone.Nodes [i - 1];
					clone.Nodes [i].Next = clone.Nodes [i];
				}
			}
			
			return clone;
		}
	}
}
