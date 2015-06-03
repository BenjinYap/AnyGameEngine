using AnyGameEngine.CustomVars;
using AnyGameEngine.Modules.Core;
using AnyGameEngine.SaveData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnyGameEngine.Modules.GlobalResources {
	public class Game {
		public string Name;
		public string Description;
		public string Author;
		public string StartingRoomId;
		
		public List <GlobalResource> GlobalResources = new List <GlobalResource> ();

		public List <Room> Rooms = new List <Room> ();

		public List <CustomVar> CustomVars = new List <CustomVar> ();

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
