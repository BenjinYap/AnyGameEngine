using AnyGameEngine.GameData;
using AnyGameEngine.Modules.Items.Logic.Actions;
using AnyGameEngine.Other;
using AnyGameEngine.SaveData;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Xml;

namespace AnyGameEngine.Modules.Items {
	public class ItemsModule:Module {
		public delegate void LogicItemModifyEventHandler (object sender, LogicItemModifyEventArgs e);
		public event LogicItemModifyEventHandler ItemModify;

		public ItemsModule (Overlord overlord):base (overlord) {
			
		}

		public override void RegisterLogicConstructors (Overlord overlord) {
			overlord.LogicConstructorInfos.Add ("LogicItemModify", new LogicConstructorInfo (typeof (LogicItemModify), false, false));
		}

		public override void RegisterLogicHandlers (Overlord overlord) {
			overlord.LogicHandlers [typeof (LogicItemModify)] = DoLogicItemModify;
		}

		public override void LoadGame (Game game, Overlord overlord, XmlNode root) {
			XmlNode items = root ["Items"];

			UniqueList <string> existing = new UniqueList <string> ("Duplicate item {{}}");

			for (int i = 0; i < items.ChildNodes.Count; i++) {
				XmlNode n = items.ChildNodes [i];
				XmlAttributeCollection attrs = n.Attributes;

				existing.Add (attrs ["id"].Value);

				Item item = new Item ();
				item.Id = attrs ["id"].Value;
				item.Name = attrs ["name"].Value;
				game.Items.Add (item);
			}
		}

		private void DoLogicItemModify (Game game, Save save) {
			LogicItemModify logic = (LogicItemModify) save.CurrentLogic;
			save.CurrentLogic = logic.GetNextLogic ();
			Item item = game.Items.Find (a => a.Id == logic.ItemId);
			
			if (logic.Quantity < 0) {
				int remaining = logic.Quantity * -1;

				while (remaining > 0) {
					int index = save.ItemStacks.FindIndex (a => a.ItemId == logic.ItemId);

					if (index != -1) {
						ItemStack stack = save.ItemStacks [index];
						stack.Quantity -= remaining;

						if (stack.Quantity <= 0) {
							save.ItemStacks.RemoveAt (index);
						}
						
						break;
					}
				}
				
				if (this.ItemModify != null) {
					this.ItemModify (this, new LogicItemModifyEventArgs (item.Name, logic.Quantity));
				}
			} else if (logic.Quantity > 0) {
				int remaining = logic.Quantity;
				ItemStack stack = save.ItemStacks.Find (a => a.ItemId == logic.ItemId);

				if (stack == null) {
					stack = new ItemStack (logic.ItemId, logic.Quantity);
					save.ItemStacks.Add (stack);
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
