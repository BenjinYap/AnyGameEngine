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
using AnyGameEngine.Modules.Expressions;
using AnyGameEngine.Modules.CustomVars;

namespace AnyGameEngine {
	public class Overlord {
		public CoreModule CoreModule;
		public GlobalResourcesModule GlobalResourcesModule;
		public ItemsModule ItemsModule;

		internal Dictionary <Type, Action <Game, Save>> LogicHandlers = new Dictionary <Type, Action <Game, Save>> ();
		internal Dictionary <string, LogicConstructorInfo> LogicConstructorInfos = new Dictionary <string, LogicConstructorInfo> ();
		internal Dictionary <string, ExpressionConstructorInfo> ExpressionConstructorInfos = new Dictionary <string, ExpressionConstructorInfo> ();

		internal List <Module> Modules = new List <Module> ();

		private bool stepDisabled = false;
		private string invalidStepExceptionMessage = "";

		public Overlord () {
			this.CoreModule = new CoreModule (this);
			this.GlobalResourcesModule = new GlobalResourcesModule (this);
			this.ItemsModule = new ItemsModule (this);

			this.Modules.Add (this.CoreModule);
			this.Modules.Add (this.GlobalResourcesModule);
			this.Modules.Add (this.ItemsModule);
			this.Modules.Add (new ExpressionsModule (this));
			this.Modules.Add (new CustomVarsModule (this));

			this.Modules.ForEach (a => {
				a.RegisterLogicConstructors (this);
				a.RegisterExpressionConstructors (this);
				a.RegisterLogicHandlers (this);
			});
		}

		public void Step (Game game, Save save) {
			if (this.stepDisabled) {
				throw new Exception (this.invalidStepExceptionMessage);
			}

			if (save.CurrentLogic == null) {
				return;
			}

			Type currentLogicType = save.CurrentLogic.GetType ();

			if (this.LogicHandlers.ContainsKey (currentLogicType)) {
				this.LogicHandlers [currentLogicType] (game, save);
			} else {
				throw new Exception (currentLogicType.Name + " has no handler");
			}
		}

		internal void EnableStep () {
			this.stepDisabled = false;
		}

		internal void DisableStep (string exceptionMessage) {
			this.stepDisabled = true;
			this.invalidStepExceptionMessage = exceptionMessage;
		}
	}
}
