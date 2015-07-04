using AnyGameEngine.Modules.Core.Logic.Flow;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Xml;
using AnyGameEngine.GameData;

namespace AnyGameEngine.Modules.Core.Logic {
	public abstract class LogicNode:Entity {
		public LogicNode Parent;
		public LogicNode Next;
		public LogicNode Prev;
		
		public LogicNode () {
			
		}

		public LogicNode (XmlNode node) {
			XmlAttributeCollection attrs = node.Attributes;
			this.Id = attrs ["id"] == null ? "" : attrs ["id"].Value;
		}

		public virtual LogicNode Clone (LogicNode parent) {
			LogicNode clone = (LogicNode) Activator.CreateInstance (Type.GetType (base.GetType ().AssemblyQualifiedName));
			clone.Id = this.Id;
			clone.Parent = parent;
			return clone;
		}

		public LogicNode GetNextLogic () {
			if (this.Next == null) {  //no logic after this
				if (this.Parent is LogicOption) {  //if parent is logic option, get the next logic of the option list
					return this.Parent.Parent.GetNextLogic ();
				} else if (this.Parent is LogicLoop) {
					return this.Parent;
				} else if (this.Parent != null) {  //if parent is not null, get the next logic of parent
					return this.Parent.GetNextLogic ();
				} else {  //if parent is null, return null
					return null;
				}
			} else {  //logic after this, return that logic
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
