﻿using AnyGameEngine.GameData;
using AnyGameEngine.Modules.Currencies.Logic.Actions;
using AnyGameEngine.SaveData;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace AnyGameEngine.Modules.Currencies {
	public class CurrenciesModule:Module {
		public delegate void LogicCurrencySetEventHandler (object sender, LogicCurrencyChangeEventArgs e);
		public event LogicCurrencySetEventHandler CurrencySet;

		public delegate void LogicCurrencyModifyEventHandler (object sender, LogicCurrencyChangeEventArgs e);
		public event LogicCurrencyModifyEventHandler CurrencyModified;

		public CurrenciesModule (Overlord overlord):base (overlord) {
			overlord.LogicHandlers [typeof (LogicCurrencySet)] = DoLogicCurrencySet;
			overlord.LogicHandlers [typeof (LogicCurrencyModify)] = DoLogicCurrencyModify;
		}

		#region Do Logic Actions
		private void DoLogicCurrencySet () {
			float amount = ((LogicCurrencySet) this.Save.CurrentLogic).Amount;
			this.Save.CurrentLogic = this.Save.CurrentLogic.GetNextLogic ();
			
			if (this.CurrencySet != null) {
				this.CurrencySet (this, new LogicCurrencyChangeEventArgs (amount));
			}
		}

		private void DoLogicCurrencyModify () {
			float amount = ((LogicCurrencyModify) this.Save.CurrentLogic).Amount;
			this.Save.CurrentLogic = this.Save.CurrentLogic.GetNextLogic ();
			
			if (this.CurrencyModified != null) {
				this.CurrencyModified (this, new LogicCurrencyChangeEventArgs (amount));
			}
		}
		#endregion
	}
}
