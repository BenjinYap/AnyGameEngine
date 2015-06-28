using AnyGameEngine.GameData;
using AnyGameEngine.Modules.Items.Logic.Actions;
using AnyGameEngine.Other;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace AnyGameEngine.Modules.Items {
	public class ItemsModule:Module {
		public delegate void LogicItemModifyEventHandler (object sender, LogicItemModifyEventArgs e);
		public event LogicItemModifyEventHandler ItemModify;

		public ItemsModule (Overlord overlord):base (overlord) {
			overlord.LogicHandlers [typeof (LogicItemModify)] = DoLogicItemModify;

			overlord.LogicConstructorInfos.Add ("LogicItemModify", new LogicConstructorInfo (typeof (LogicItemModify), false));
		}

		private void DoLogicItemModify () {
			LogicItemModify logic = (LogicItemModify) this.Save.CurrentLogic;
			this.Save.CurrentLogic = logic.GetNextLogic ();
			Item item = this.Game.Items.Find (a => a.Id == logic.ItemId);
			
			if (logic.Quantity < 0) {
				int remaining = logic.Quantity * -1;

				while (remaining > 0) {
					int index = this.Save.ItemStacks.FindIndex (a => a.ItemId == logic.ItemId);

					if (index != -1) {
						ItemStack stack = this.Save.ItemStacks [index];
						stack.Quantity -= remaining;

						if (stack.Quantity <= 0) {
							this.Save.ItemStacks.RemoveAt (index);
						}
						
						break;
					}
				}
				
				if (this.ItemModify != null) {
					this.ItemModify (this, new LogicItemModifyEventArgs (item.Name, logic.Quantity));
				}
			} else if (logic.Quantity > 0) {
				int remaining = logic.Quantity;
				ItemStack stack = this.Save.ItemStacks.Find (a => a.ItemId == logic.ItemId);

				if (stack == null) {
					stack = new ItemStack (logic.ItemId, logic.Quantity);
					this.Save.ItemStacks.Add (stack);
				} else {
					stack.Quantity += logic.Quantity;
				}

				if (this.ItemModify != null) {
					this.ItemModify (this, new LogicItemModifyEventArgs (item.Name, logic.Quantity));
				}
			}
		}
	}
}
