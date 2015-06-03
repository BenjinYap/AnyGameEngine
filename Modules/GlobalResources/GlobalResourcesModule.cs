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
		}

		#region Do Logic Actions
		private void DoLogicGlobalResourceSet () {
			float amount = ((LogicGlobalResourceSet) this.Save.CurrentLogic).Amount;
			this.Save.CurrentLogic = this.Save.CurrentLogic.GetNextLogic ();
			
			if (this.GlobalResourceSet != null) {
				this.GlobalResourceSet (this, new LogicGlobalResourceChangeEventArgs (amount));
			}
		}

		private void DoLogicGlobalResourceModify () {
			float amount = ((LogicGlobalResourceModify) this.Save.CurrentLogic).Amount;
			this.Save.CurrentLogic = this.Save.CurrentLogic.GetNextLogic ();
			
			if (this.GlobalResourceModified != null) {
				this.GlobalResourceModified (this, new LogicGlobalResourceChangeEventArgs (amount));
			}
		}
		#endregion
	}
}
