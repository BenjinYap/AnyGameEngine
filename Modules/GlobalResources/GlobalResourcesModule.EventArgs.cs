using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnyGameEngine.Modules.GlobalResources {
	
	public class LogicGlobalResourceChangeEventArgs {
		public float Amount;

		public LogicGlobalResourceChangeEventArgs (float amount) {
			this.Amount = amount;
		}
	}
}
