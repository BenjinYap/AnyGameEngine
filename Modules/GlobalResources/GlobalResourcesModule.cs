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
			GameStorage.types.Add ("LogicGlobalResourceSet", typeof (LogicGlobalResourceSet));
			GameStorage.types.Add ("LogicGlobalResourceModify", typeof (LogicGlobalResourceModify));

			overlord.LogicHandlers [typeof (LogicGlobalResourceSet)] = DoLogicGlobalResourceSet;
			overlord.LogicHandlers [typeof (LogicGlobalResourceModify)] = DoLogicGlobalResourceModify;
		}

		private void DoLogicGlobalResourceSet () {
			LogicGlobalResourceSet logic = (LogicGlobalResourceSet) this.Save.CurrentLogic;
			this.Save.GlobalResources [logic.ResourceId] = logic.Amount;
			this.Save.CurrentLogic = logic.GetNextLogic ();
			
			if (this.GlobalResourceSet != null) {
				this.GlobalResourceSet (this, new LogicGlobalResourceChangeEventArgs (logic.Amount));
			}
		}

		private void DoLogicGlobalResourceModify () {
			LogicGlobalResourceModify logic = (LogicGlobalResourceModify) this.Save.CurrentLogic;
			this.Save.GlobalResources [logic.ResourceId] += logic.Amount;
			this.Save.CurrentLogic = logic.GetNextLogic ();
			
			if (this.GlobalResourceModified != null) {
				this.GlobalResourceModified (this, new LogicGlobalResourceChangeEventArgs (logic.Amount));
			}
		}
	}
}
