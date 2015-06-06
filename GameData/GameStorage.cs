using AnyGameEngine.CustomVars;
using AnyGameEngine.Modules.Core;
using AnyGameEngine.Modules.Core.Logic;
using AnyGameEngine.Modules.Core.Logic.Actions;
using AnyGameEngine.Modules.Core.Logic.Flow;
using AnyGameEngine.Modules.GlobalResources;
using AnyGameEngine.Modules.GlobalResources.Logic.Actions;
using AnyGameEngine.Modules.Items;
using AnyGameEngine.Other;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace AnyGameEngine.GameData {
	internal static class GameStorage {
		public static Dictionary <string, Type> types = new Dictionary <string, Type> ();

		public static LogicNode CreateLogic (XmlNode node) {
			LogicNode logic = (LogicNode) Activator.CreateInstance (GameStorage.types [node.Name], new object [] {node});				
			return logic;
		}

		public static Game FromXml (string xml) {
			XmlDocument doc = new XmlDocument ();
			XmlReaderSettings settings = new XmlReaderSettings ();
			settings.IgnoreComments = true;
			XmlReader reader = XmlReader.Create (new StringReader (xml), settings);
			doc.Load (reader);
			XmlElement root = doc ["Game"];

			Game game = new Game ();
			GameStorage.LoadGeneral (game, root);
			GameStorage.LoadGlobalResources (game, root ["GlobalResources"]);
			GameStorage.LoadRooms (game, root ["Rooms"]);
			GameStorage.LoadCustomVars (game, root ["CustomVars"]);
			GameStorage.LoadItems (game, root ["Items"]);

			return game;
		}

		private static void LoadGeneral (Game game, XmlElement node) {
			XmlAttributeCollection attrs = node.Attributes;
			game.Name = attrs ["name"].Value;
			game.Description = attrs ["description"].Value;
			game.Author = attrs ["author"].Value;
			game.StartingRoomId = attrs ["startingRoomId"].Value;
		}

		private static void LoadGlobalResources (Game game, XmlElement node) {
			UniqueList <string> existing = new UniqueList <string> ("Duplicate global resource {{}}");

			for (int i = 0; i < node.ChildNodes.Count; i++) {
				XmlNode n = node.ChildNodes [i];
				XmlAttributeCollection attrs = n.Attributes;

				existing.Add (attrs ["id"].Value);

				GlobalResource resource = new GlobalResource ();
				resource.Id = attrs ["id"].Value;
				resource.Name = attrs ["name"].Value;
				resource.StartingAmount = float.Parse (attrs ["startingAmount"].Value);
				game.GlobalResources.Add (resource);
			}
		}

		private static void LoadRooms (Game game, XmlElement node) {
			List <string> existingRooms = new List <string> ();
			
			for (int i = 0; i < node.ChildNodes.Count; i++) {
				XmlNode n = node.ChildNodes [i];
				XmlAttributeCollection attrs = n.Attributes;
				Room room = new Room ();
				room.Id = attrs ["id"].Value;
				room.Name = attrs ["name"].Value;
				room.LogicList = new LogicList (n.ChildNodes [0]);
				
				existingRooms.Add (room.Id);
				
				game.Rooms.Add (room);
			}
		}

		private static void LoadCustomVars (Game game, XmlElement node) {
			List <string> existingVars = new List <string> ();

			for (var i = 0; i < node.ChildNodes.Count; i++) {
				XmlNode v = node.ChildNodes [i];
				XmlAttributeCollection attrs = v.Attributes;
				string name = attrs ["name"].Value;

				if (existingVars.Contains (name)) {
					throw new Exception (string.Format ("CustomVar {0} already exists", name));
				}

				CustomVar customVar = null;

				if (v.Name == "CustomArrayVar") {
					string type = attrs ["type"].Value;
					string [] values = attrs ["values"].Value.Split (',');

					if (type == "number") {
						customVar = new CustomArrayVar <float> (new List <string> (values).Select (a => float.Parse (a)).ToArray ());
					} else if (type == "string") {
						customVar = new CustomArrayVar <string> (values);
					}
				} else {
					string value = attrs ["value"].Value;

					if (v.Name == "CustomNumberVar") {
						customVar = new CustomSingleVar <float> (float.Parse (value));
					} else if (v.Name == "CustomStringVar") {
						customVar = new CustomSingleVar <string> (value);
					}
				}

				customVar.Name = name;
				existingVars.Add (name);
			}
		}

		private static void LoadItems (Game game, XmlElement node) {
			UniqueList <string> existing = new UniqueList <string> ("Duplicate item {{}}");

			for (int i = 0; i < node.ChildNodes.Count; i++) {
				XmlNode n = node.ChildNodes [i];
				XmlAttributeCollection attrs = n.Attributes;

				existing.Add (attrs ["id"].Value);

				Item item = new Item ();
				item.Id = attrs ["id"].Value;
				item.Name = attrs ["name"].Value;
				game.Items.Add (item);
			}
		}
	}
}
