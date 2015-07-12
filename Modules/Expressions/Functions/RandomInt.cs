using AnyGameEngine.GameData;
using AnyGameEngine.SaveData;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Xml;

namespace AnyGameEngine.Modules.Expressions.Functions {
	public class RandomInt:Function {
		private int min;
		private int max;

		private static Random rand;

		public RandomInt (XmlNode node) {
			this.min = int.Parse (node.Attributes ["min"].Value);
			this.max = int.Parse (node.Attributes ["max"].Value);
		}

		public override string Evaluate (Game game, Save save) {
			return RandomInt.rand.Next (this.min, this.max).ToString ();
		}

		static RandomInt () {
			RandomInt.rand = new Random ();
		}
	}
}
