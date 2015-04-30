using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnyGameEngine.Entities.Logic.Actions {
	public class LogicRoomChange:LogicNode {
		public string ZoneId;

		public override LogicNode Clone (LogicNode parent) {
			LogicRoomChange clone = (LogicRoomChange) base.Clone (parent);
			clone.ZoneId = this.ZoneId;
			return clone;
		}
	}
}
