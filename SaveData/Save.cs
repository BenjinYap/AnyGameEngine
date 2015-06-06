using AnyGameEngine.Modules.Core.Logic;
using AnyGameEngine.Modules.GlobalResources;
using AnyGameEngine.Modules.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnyGameEngine.SaveData {
	public class Save {
		public string CurrentRoomId;
		public LogicNode CurrentLogic;

		public Dictionary <string, float> GlobalResources = new Dictionary <string, float> ();

		public float CurrencyAmount;

		public List <ItemStack> ItemStacks = new List <ItemStack> ();
	}
}
