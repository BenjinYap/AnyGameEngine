using AnyGameEngine.Inventory;
using AnyGameEngine.Modules.Core.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnyGameEngine.SaveData {
	public class Save {
		public string CurrentRoomId;
		public LogicNode CurrentLogic;

		public float CurrencyAmount;

		public List <ItemStack> ItemStacks = new List <ItemStack> ();
	}
}
