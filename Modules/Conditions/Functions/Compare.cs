using AnyGameEngine.GameData;
using AnyGameEngine.Modules.Expressions.Functions;
using AnyGameEngine.SaveData;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Xml;

namespace AnyGameEngine.Modules.Conditions.Functions {
	public class Compare:Function {
		private Type type;

		public Compare (XmlNode node) {
			this.type = (Type) Enum.Parse (typeof (Type), node.Attributes ["type"].Value);
		}

		public override string Evaluate (Game game, Save save) {
			string v1 = this.Tokens [0].Evaluate (game, save);
			string v2 = this.Tokens [1].Evaluate (game, save);
			return v1.Equals (v2).ToString ();
		}

		private enum Type { Equals };
	}
}
