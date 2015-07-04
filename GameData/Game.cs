using AnyGameEngine.Modules.CustomVars;
using AnyGameEngine.Modules.Core;
using AnyGameEngine.Modules.Core.Logic;
using AnyGameEngine.Modules.Core.Logic.Flow;
using AnyGameEngine.Modules.GlobalResources;
using AnyGameEngine.Modules.Items;
using AnyGameEngine.Other;
using AnyGameEngine.SaveData;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Xml;
using System.Diagnostics;

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

		public Dictionary <string, object> ModuleVars = new Dictionary <string, object> ();

		public Game (Overlord overlord, string path) {
			XmlDocument doc = new XmlDocument ();
			XmlReaderSettings settings = new XmlReaderSettings ();
			settings.IgnoreComments = true;
			XmlReader reader = XmlReader.Create (new StringReader (File.ReadAllText (path)), settings);
			doc.Load (reader);
			XmlNode root = doc ["Game"];
			
			overlord.Modules.ForEach (a => a.LoadGame (this, overlord, root));
		}

		public Save GetFreshSave () {
			Save save = new Save ();

			this.GlobalResources.ForEach (a => save.GlobalResources [a.Id] = a.StartingAmount);
			
			this.CustomVars.ForEach (a => save.CustomVars.Add (a.Clone ()));

			Room room = this.Rooms.Find (a => a.Id == this.StartingRoomId);
			save.CurrentLogic = room.LogicList.Clone (null);
			return save;
		}

		public override string ToString () {
			return string.Format ("Game: {0}, Author: {1}", this.Name, this.Author);
		}
	}
}