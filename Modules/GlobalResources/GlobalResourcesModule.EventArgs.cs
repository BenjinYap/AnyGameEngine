using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnyGameEngine.Modules.GlobalResources {
	
	public class LogicGlobalResourceChangeEventArgs {
		public string ResourceName;
		public float Amount;

		public LogicGlobalResourceChangeEventArgs (string resourceName, float amount) {
			this.ResourceName = resourceName;
			this.Amount = amount;
		}
	}
}
