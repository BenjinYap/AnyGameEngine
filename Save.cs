using AnyGameEngine.Entities.Logic;
using AnyGameEngine.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnyGameEngine {
	public class Save {
		public string CurrentRoomId;
		public LogicNode CurrentLogic;

		public List <ItemStack> ItemStacks = new List <ItemStack> ();
	}
}
