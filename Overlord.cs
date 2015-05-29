using AnyGameEngine.GameData;
using AnyGameEngine.Modules;
using AnyGameEngine.Modules.Core;
using AnyGameEngine.SaveData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnyGameEngine {
	public class Overlord {
		public CoreModule CoreModule;

		internal Dictionary <Type, Action> LogicHandlers = new Dictionary <Type, Action> ();

		private List <Module> modules = new List <Module> ();

		private Game game;
		private Save save;

		private bool stepDisabled;
		private string invalidStepExceptionMessage;

		public Overlord () {
			this.CoreModule = new CoreModule (this);

			this.modules.Add (this.CoreModule);
		}

		public void SetGameAndSave (Game game, Save save) {
			this.game = game;
			this.save = save;

			this.modules.ForEach (a => {
				a.SetGame (this.game);
				a.SetSave (this.save);
			});
		}

		public void Step () {
			if (this.stepDisabled) {
				throw new Exception (this.invalidStepExceptionMessage);
			}

			Type currentLogicType = this.save.CurrentLogic.GetType ();

			if (this.LogicHandlers.ContainsKey (currentLogicType)) {
				this.LogicHandlers [currentLogicType] ();
			} else {
				throw new Exception (currentLogicType.Name + " has no handler");
			}
		}
	}
}
