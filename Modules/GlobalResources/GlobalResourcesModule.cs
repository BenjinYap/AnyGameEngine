﻿using AnyGameEngine.GameData;
using AnyGameEngine.Modules.GlobalResources.Logic.Actions;
using AnyGameEngine.SaveData;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace AnyGameEngine.Modules.GlobalResources {
	public class GlobalResourcesModule:Module {
		public delegate void LogicGlobalResourceSetEventHandler (object sender, LogicGlobalResourceChangeEventArgs e);
		public event LogicGlobalResourceSetEventHandler GlobalResourceSet;

		public delegate void LogicGlobalResourceModifyEventHandler (object sender, LogicGlobalResourceChangeEventArgs e);
		public event LogicGlobalResourceModifyEventHandler GlobalResourceModified;

		public GlobalResourcesModule (Overlord overlord):base (overlord) {
			
		}

		public override void RegisterLogicConstructors (Overlord overlord) {
			overlord.LogicConstructorInfos.Add ("LogicGlobalResourceSet", new LogicConstructorInfo (typeof (LogicGlobalResourceSet), false));
			overlord.LogicConstructorInfos.Add ("LogicGlobalResourceModify", new LogicConstructorInfo (typeof (LogicGlobalResourceModify), false));
		}

		public override void RegisterLogicHandlers (Overlord overlord) {
			overlord.LogicHandlers [typeof (LogicGlobalResourceSet)] = DoLogicGlobalResourceSet;
			overlord.LogicHandlers [typeof (LogicGlobalResourceModify)] = DoLogicGlobalResourceModify;
		}

		private void DoLogicGlobalResourceSet (Game game, Save save) {
			LogicGlobalResourceSet logic = (LogicGlobalResourceSet) save.CurrentLogic;
			save.GlobalResources [logic.ResourceId] = logic.Amount;
			save.CurrentLogic = logic.GetNextLogic ();
			
			if (this.GlobalResourceSet != null) {
				string resourceName = game.GlobalResources.Find (a => a.Id == logic.ResourceId).Name;
				this.GlobalResourceSet (this, new LogicGlobalResourceChangeEventArgs (resourceName, logic.Amount));
			}
		}

		private void DoLogicGlobalResourceModify (Game game, Save save) {
			LogicGlobalResourceModify logic = (LogicGlobalResourceModify) save.CurrentLogic;
			save.GlobalResources [logic.ResourceId] += logic.Amount;
			save.GlobalResources [logic.ResourceId] = save.GlobalResources [logic.ResourceId] < 0 ? 0 : save.GlobalResources [logic.ResourceId];
			save.CurrentLogic = logic.GetNextLogic ();
			
			if (this.GlobalResourceModified != null) {
				string resourceName = game.GlobalResources.Find (a => a.Id == logic.ResourceId).Name;
				this.GlobalResourceModified (this, new LogicGlobalResourceChangeEventArgs (resourceName, logic.Amount));
			}
		}
	}
}
