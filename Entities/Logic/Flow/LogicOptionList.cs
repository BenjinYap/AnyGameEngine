using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnyGameEngine.Entities.Logic.Flow {
	public class LogicOptionList:LogicList {
		public string Text;

		public override LogicNode Clone (LogicNode parent) {
			LogicOptionList clone = (LogicOptionList) base.Clone (parent);
			clone.Text = this.Text;
			return clone;
		}
	}
}
