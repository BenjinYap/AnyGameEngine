using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnyGameEngine.Entities.Logic.Actions {
	public class LogicText:LogicNode {
		public string Text;

		public override LogicNode Clone (LogicNode parent) {
			LogicText clone = (LogicText) base.Clone (parent);
			clone.Text = this.Text;
			return clone;
		}
	}
}
