using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnyGameEngine.Modules.Items {
	public class ItemStack {
		public string ItemId;
		public int Quantity;

		public ItemStack (string itemId) {
			this.ItemId = itemId;
		}

		public ItemStack (string itemId, int quantity) {
			this.ItemId = itemId;
			this.Quantity = quantity;
		}
	}
}
