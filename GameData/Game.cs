using AnyGameEngine.CustomVars;
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

		public Game (Overlord overlord, string path) {
			XmlDocument doc = new XmlDocument ();
			XmlReaderSettings settings = new XmlReaderSettings ();
			settings.IgnoreComments = true;
			XmlReader reader = XmlReader.Create (new StringReader (File.ReadAllText (path)), settings);
			doc.Load (reader);
			XmlNode root = doc ["Game"];
			
			LoadGeneral (root);
			LoadGlobalResources (root ["GlobalResources"]);
			LoadRooms (root ["Rooms"], overlord);
			LoadCustomVars (root ["CustomVars"]);
			LoadItems (root ["Items"]);
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

		public LogicNode CreateLogic (XmlNode node, Overlord overlord) {
			if (overlord.LogicConstructorInfos.ContainsKey (node.Name) == false) {
				throw new Exception ("Logic class not found for " + node.Name);
			}

			List <object> args = new List <Object> {node};

			if (overlord.LogicConstructorInfos [node.Name].ContainsExpressions) {
				args.Add (overlord.ExpressionConstructorInfos);
			}

			LogicNode logic = (LogicNode) Activator.CreateInstance (overlord.LogicConstructorInfos [node.Name].Type, args.ToArray ());

			if (logic is LogicList) {
				//create the child nodes
				for (int i = 0; i < node.ChildNodes.Count; i++) {
					LogicNode childLogic = CreateLogic (node.ChildNodes [i], overlord);
					logic.Nodes.Add (childLogic);
				
					if (i > 0) {
						logic.Nodes [i].Prev = logic.Nodes [i - 1];
						logic.Nodes [i - 1].Next = logic.Nodes [i];
					}
				}
			}

			return logic;
		}

		private void LoadGeneral (XmlNode node) {
			XmlAttributeCollection attrs = node.Attributes;
			this.Name = attrs ["name"].Value;
			this.Description = attrs ["description"].Value;
			this.Author = attrs ["author"].Value;
			this.StartingRoomId = attrs ["startingRoomId"].Value;
		}

		private void LoadGlobalResources (XmlNode node) {
			this.GlobalResources = new List <GlobalResource> ();

			UniqueList <string> existing = new UniqueList <string> ("Duplicate global resource {{}}");

			for (int i = 0; i < node.ChildNodes.Count; i++) {
				XmlNode n = node.ChildNodes [i];
				XmlAttributeCollection attrs = n.Attributes;

				existing.Add (attrs ["id"].Value);

				GlobalResource resource = new GlobalResource ();
				resource.Id = attrs ["id"].Value;
				resource.Name = attrs ["name"].Value;
				resource.StartingAmount = float.Parse (attrs ["startingAmount"].Value);
				this.GlobalResources.Add (resource);
			}
		}

		private void LoadRooms (XmlNode node, Overlord overlord) {
			List <string> existingRooms = new List <string> ();
			
			for (int i = 0; i < node.ChildNodes.Count; i++) {
				XmlNode n = node.ChildNodes [i];
				XmlAttributeCollection attrs = n.Attributes;
				Room room = new Room ();
				room.Id = attrs ["id"].Value;
				room.Name = attrs ["name"].Value;
				room.LogicList = (LogicList) CreateLogic (n.ChildNodes [0], overlord);
				
				existingRooms.Add (room.Id);
				
				this.Rooms.Add (room);
			}
		}

		private void LoadCustomVars (XmlNode node) {
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

		private void LoadItems (XmlNode node) {
			UniqueList <string> existing = new UniqueList <string> ("Duplicate item {{}}");

			for (int i = 0; i < node.ChildNodes.Count; i++) {
				XmlNode n = node.ChildNodes [i];
				XmlAttributeCollection attrs = n.Attributes;

				existing.Add (attrs ["id"].Value);

				Item item = new Item ();
				item.Id = attrs ["id"].Value;
				item.Name = attrs ["name"].Value;
				this.Items.Add (item);
			}
		}
	}
}