using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnyGameEngine.Entities.Logic.Flow {
	public class LogicOption:LogicList {
		public string Text;

		public override LogicNode Clone (LogicNode parent) {
			LogicOption clone = (LogicOption) base.Clone (parent);
			clone.Text = this.Text;
			return clone;
		}
	}
}
