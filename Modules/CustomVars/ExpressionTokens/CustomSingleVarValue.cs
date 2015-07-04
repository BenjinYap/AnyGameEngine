

using AnyGameEngine.GameData;
using AnyGameEngine.Modules.Expressions;
using AnyGameEngine.SaveData;
using System;
using System.Xml;
namespace AnyGameEngine.Modules.CustomVars.ExpressionTokens {
	public class CustomSingleVarValue:ExpressionToken {
		private string name;

		public CustomSingleVarValue (XmlNode node) {
			this.name = node.Attributes ["name"].Value;
		}

		public override string Evaluate (Game game, Save save) {
			CustomSingleVar var = (CustomSingleVar) save.CustomVars.Find (a => a.Name == this.name);
			return var.Value;
		}
	}
}
