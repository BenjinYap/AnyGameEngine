using AnyGameEngine.Modules.Core.Logic.Flow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnyGameEngine.Modules.Core {
	public class Room:Entity {
		public string Name;
		public LogicList LogicList;

		public override string ToString () {
			return string.Format ("{0}, {1}, Nodes: {1}", this.Id, this.Name, this.LogicList.Nodes.Count);
		}
	}
}
