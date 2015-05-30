using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnyGameEngine.Modules.Items {
	
	public class LogicCurrencyChangeEventArgs {
		public float Amount;

		public LogicCurrencyChangeEventArgs (float amount) {
			this.Amount = amount;
		}
	}
}
