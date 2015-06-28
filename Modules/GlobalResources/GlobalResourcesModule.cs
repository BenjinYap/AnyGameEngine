using AnyGameEngine.GameData;
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
			overlord.LogicHandlers [typeof (LogicGlobalResourceSet)] = DoLogicGlobalResourceSet;
			overlord.LogicHandlers [typeof (LogicGlobalResourceModify)] = DoLogicGlobalResourceModify;

			overlord.Types.Add ("LogicGlobalResourceSet", new LogicConstructorInfo (typeof (LogicGlobalResourceSet), false));
			overlord.Types.Add ("LogicGlobalResourceModify", new LogicConstructorInfo (typeof (LogicGlobalResourceModify), false));
		}

		private void DoLogicGlobalResourceSet () {
			LogicGlobalResourceSet logic = (LogicGlobalResourceSet) this.Save.CurrentLogic;
			this.Save.GlobalResources [logic.ResourceId] = logic.Amount;
			this.Save.CurrentLogic = logic.GetNextLogic ();
			
			if (this.GlobalResourceSet != null) {
				string resourceName = this.Game.GlobalResources.Find (a => a.Id == logic.ResourceId).Name;
				this.GlobalResourceSet (this, new LogicGlobalResourceChangeEventArgs (resourceName, logic.Amount));
			}
		}

		private void DoLogicGlobalResourceModify () {
			LogicGlobalResourceModify logic = (LogicGlobalResourceModify) this.Save.CurrentLogic;
			this.Save.GlobalResources [logic.ResourceId] += logic.Amount;
			this.Save.GlobalResources [logic.ResourceId] = this.Save.GlobalResources [logic.ResourceId] < 0 ? 0 : this.Save.GlobalResources [logic.ResourceId];
			this.Save.CurrentLogic = logic.GetNextLogic ();
			
			if (this.GlobalResourceModified != null) {
				string resourceName = this.Game.GlobalResources.Find (a => a.Id == logic.ResourceId).Name;
				this.GlobalResourceModified (this, new LogicGlobalResourceChangeEventArgs (resourceName, logic.Amount));
			}
		}
	}
}
