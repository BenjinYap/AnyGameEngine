using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnyGameEngine.Inventory {
	public class Item:Entity {
		public string Name;

		public override string ToString () {
			return string.Format ("Item, Id: {0}, Name: {1}", this.Id, this.Name);
		}
	}
}
