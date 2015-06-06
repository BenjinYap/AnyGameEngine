using AnyGameEngine.GameData;
using AnyGameEngine.SaveData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnyGameEngine.Modules {
	public class Module {
		protected Overlord Overlord;
		
		protected Game Game;
		protected Save Save;

		public Module (Overlord overlord) {
			this.Overlord = overlord;
		}

		public void SetGame (Game game) {
			this.Game = game;
		}

		public void SetSave (Save save) {
			this.Save = save;
		}
	}
}
