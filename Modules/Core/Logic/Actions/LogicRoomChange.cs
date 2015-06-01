using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace AnyGameEngine.Modules.Core.Logic.Actions {
	public class LogicRoomChange:LogicNode {
		public string RoomId;

		public LogicRoomChange () {

		}

		public LogicRoomChange (XmlNode node):base (node) {
			this.RoomId = node.Attributes ["roomId"].Value;
		}

		public override LogicNode Clone (LogicNode parent) {
			LogicRoomChange clone = (LogicRoomChange) base.Clone (parent);
			clone.RoomId = this.RoomId;
			return clone;
		}

		public override string ToString () {
			return string.Format ("Room Change, Room: {0}", this.RoomId);
		}
	}
}
