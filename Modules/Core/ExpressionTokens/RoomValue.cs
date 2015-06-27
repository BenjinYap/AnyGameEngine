

using AnyGameEngine.Modules.Expressions;
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

		public override string Evaluate () {
			if (this.property == Property.Name) {

			}

			return "1";
		}

		public enum Property { Name };
	}
}
