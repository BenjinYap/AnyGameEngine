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

			if (type == Type.Equals) {
				return v1.Equals (v2).ToString ();
			} else if (type == Type.LessThan) {
				return (float.Parse (v1) < float.Parse (v2)).ToString ();
			} else if (type == Type.LessThanEquals) {
				return (float.Parse (v1) <= float.Parse (v2)).ToString ();
			} else if (type == Type.GreaterThan) {
				return (float.Parse (v1) > float.Parse (v2)).ToString ();
			} else if (type == Type.GreaterThanEquals) {
				return (float.Parse (v1) >= float.Parse (v2)).ToString ();
			}

			return null;
		}

		private enum Type { Equals, LessThan, LessThanEquals, GreaterThan, GreaterThanEquals };
	}
}
