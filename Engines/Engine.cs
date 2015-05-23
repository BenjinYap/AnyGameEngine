using AnyGameEngine.GameData;
using AnyGameEngine.SaveData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnyGameEngine.Engines {
	public class Engine {
		protected Game game;
		protected Save save;

		public Engine (Game game, Save save) {
			this.game = game;
			this.save = save;
		}
	}
}
