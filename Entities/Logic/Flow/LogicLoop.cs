using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnyGameEngine.Entities.Logic.Flow {
	public class LogicLoop:LogicNode {
		public int Repeat = 0;

		public override LogicNode Clone (LogicNode parent) {
			LogicLoop clone = (LogicLoop) base.Clone (parent);
			clone.Repeat = this.Repeat;
			return clone;
		}
	}
}
