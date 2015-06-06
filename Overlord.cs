using AnyGameEngine.Modules.GlobalResources;
using AnyGameEngine.Modules;
using AnyGameEngine.Modules.Core;
using AnyGameEngine.Modules.Items;
using AnyGameEngine.SaveData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AnyGameEngine.GameData;

namespace AnyGameEngine {
	public class Overlord {
		public CoreModule CoreModule;
		public GlobalResourcesModule GlobalResourcesModule;
		public ItemsModule ItemsModule;

		internal Dictionary <Type, Action> LogicHandlers = new Dictionary <Type, Action> ();

		private List <Module> modules = new List <Module> ();

		private Game game;
		private Save save;

		private bool stepDisabled = false;
		private string invalidStepExceptionMessage = "";

		public Overlord () {
			this.CoreModule = new CoreModule (this);
			this.GlobalResourcesModule = new GlobalResourcesModule (this);
			this.ItemsModule = new ItemsModule (this);

			this.modules.Add (this.CoreModule);
			this.modules.Add (this.GlobalResourcesModule);
			this.modules.Add (this.ItemsModule);
		}

		public Game LoadGameFromXml (string xml) {
			return GameStorage.FromXml (xml);
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

			if (this.save.CurrentLogic == null) {
				return;
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
