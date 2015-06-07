using AnyGameEngine.CustomVars;
using AnyGameEngine.Modules.Core;
using AnyGameEngine.Modules.GlobalResources;
using AnyGameEngine.Modules.Items;
using AnyGameEngine.SaveData;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace AnyGameEngine.GameData {
	public class Game {
		public string Name { get; set; }
		public string Description;
		public string Author;
		public string StartingRoomId;
		
		public List <GlobalResource> GlobalResources { get; set; }

		public List <Room> Rooms = new List <Room> ();

		public List <CustomVar> CustomVars = new List <CustomVar> ();

		public List <Item> Items = new List <Item> ();

		public Game () {
			this.GlobalResources = new List <GlobalResource> ();
		}

		public Save GetFreshSave () {
			Save save = new Save ();

			this.GlobalResources.ForEach (a => save.GlobalResources [a.Id] = a.StartingAmount);
			
			Room room = this.Rooms.Find (a => a.Id == this.StartingRoomId);
			save.CurrentLogic = room.LogicList.Clone (null);
			return save;
		}

		public override string ToString () {
			return string.Format ("Game: {0}, Author: {1}", this.Name, this.Author);
		}

		
	}
}