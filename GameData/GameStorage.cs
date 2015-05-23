using AnyGameEngine.CustomVars;
using AnyGameEngine.Entities;
using AnyGameEngine.Entities.Logic;
using AnyGameEngine.Entities.Logic.Actions;
using AnyGameEngine.Entities.Logic.Flow;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace AnyGameEngine.GameData {
	public static class GameStorage {
		private static Dictionary <string, Type> types = new Dictionary <string, Type> ();



		static GameStorage () {
			//actions
			GameStorage.types.Add ("LogicText", typeof (LogicText));
			GameStorage.types.Add ("LogicRoomChange", typeof (LogicRoomChange));

			//flows
			GameStorage.types.Add ("LogicList", typeof (LogicList));
			GameStorage.types.Add ("LogicOption", typeof (LogicOption));
			GameStorage.types.Add ("LogicOptionList", typeof (LogicOptionList));
			GameStorage.types.Add ("LogicBackUpOptionList", typeof (LogicBackUpOptionList));
			GameStorage.types.Add ("LogicLoop", typeof (LogicLoop));
			GameStorage.types.Add ("LogicLoopBreak", typeof (LogicLoopBreak));
			GameStorage.types.Add ("LogicLoopContinue", typeof (LogicLoopContinue));
			GameStorage.types.Add ("LogicIgnorePoint", typeof (LogicIgnorePoint));
		}

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
			GameStorage.LoadCurrency (game, root ["Currency"]);
			GameStorage.LoadRooms (game, root ["Rooms"]);
			GameStorage.LoadCustomVars (game, root ["CustomVars"]);

			return game;
		}

		private static void LoadGeneral (Game game, XmlElement node) {
			XmlAttributeCollection attrs = node.Attributes;
			game.Name = attrs ["name"].Value;
			game.Description = attrs ["description"].Value;
			game.Author = attrs ["author"].Value;
			game.StartingRoomId = attrs ["startingRoomId"].Value;
		}

		private static void LoadCurrency (Game game, XmlElement node) {
			XmlAttributeCollection attrs = node.Attributes;
			game.Currency.StartingAmount = float.Parse (attrs ["startingAmount"].Value);
			game.Currency.Name = attrs ["name"].Value;
		}

		private static void LoadRooms (Game game, XmlElement node) {
			List <string> existingRooms = new List <string> ();
			
			for (var i = 0; i < node.ChildNodes.Count; i++) {
				XmlNode r = node.ChildNodes [i];
				XmlAttributeCollection attrs = r.Attributes;
				Room room = new Room ();
				room.Id = attrs ["id"].Value;
				room.Name = attrs ["name"].Value;
				room.LogicList = new LogicList (r.ChildNodes [0]);
				
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
	}
}
