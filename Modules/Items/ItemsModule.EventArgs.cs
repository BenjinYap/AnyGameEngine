using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnyGameEngine.Modules.Items {

	public class LogicItemModifyEventArgs {
		public string ItemName;
		public int Quantity;

		public LogicItemModifyEventArgs (string itemName, int quantity) {
			this.ItemName = itemName;
			this.Quantity = quantity;
		}
	}
}
