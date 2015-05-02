using AnyGameEngine.Entities;
using AnyGameEngine.Entities.Logic;
using AnyGameEngine.Entities.Logic.Actions;
using AnyGameEngine.Entities.Logic.Flow;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Xml;

namespace AnyGameEngine {
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
			doc.LoadXml (xml);
			XmlElement root = doc ["Game"];

			Game game = new Game ();
			GameStorage.LoadGeneral (game, root);
			GameStorage.LoadRooms (game, root["Rooms"]);

			return game;
		}

		private static void LoadGeneral (Game game, XmlElement node) {
			XmlAttributeCollection attrs = node.Attributes;
			game.Name = attrs ["name"].Value;
			game.Description = attrs ["description"].Value;
			game.Author = attrs ["author"].Value;
			game.StartingRoomId = attrs ["startingRoomId"].Value;
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
	}
}
