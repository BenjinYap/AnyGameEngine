

using AnyGameEngine.GameData;
using AnyGameEngine.Modules.Expressions;
using AnyGameEngine.SaveData;
using System;
using System.Xml;
namespace AnyGameEngine.Modules.Core.ExpressionTokens {
	public class RoomValue:ExpressionToken {
		private string id;
		private Property property;

		public RoomValue (XmlNode node) {
			XmlAttributeCollection attrs = node.Attributes;
			this.id = attrs ["id"].Value;
			this.property = (Property) Enum.Parse (typeof (Property), attrs ["property"].Value);
		}

		public override string Evaluate (Game game, Save save) {
			Room room = game.Rooms.Find (a => a.Id == this.id);

			if (this.property == Property.Name) {
				return room.Name;
			}

			return null;
		}

		public enum Property { Name };
	}
}
