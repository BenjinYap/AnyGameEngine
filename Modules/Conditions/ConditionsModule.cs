using AnyGameEngine.GameData;
using AnyGameEngine.Modules.Conditions.Functions;
using AnyGameEngine.Modules.Conditions.Logic.Flow;
using AnyGameEngine.Modules.Core.Logic.Flow;
using AnyGameEngine.SaveData;
using System;
using System.Diagnostics;
using System.Linq;
using System.Xml;

namespace AnyGameEngine.Modules.Conditions {
	public class ConditionsModule:Module {
		//public delegate void LogicGlobalResourceSetEventHandler (object sender, LogicGlobalResourceChangeEventArgs e);
		//public event LogicGlobalResourceSetEventHandler GlobalResourceSet;

		public ConditionsModule (Overlord overlord):base (overlord) {
			
		}

		public override void RegisterLogicConstructors (Overlord overlord) {
			overlord.LogicConstructorInfos.Add ("LogicCondition", new LogicConstructorInfo (typeof (LogicCondition), true, true));
		}

		public override void RegisterLogicHandlers (Overlord overlord) {
			overlord.LogicHandlers [typeof (LogicCondition)] = DoLogicCondition;
		}

		public override void RegisterExpressionConstructors (Overlord overlord) {
			overlord.ExpressionConstructorInfos.Add ("Compare", new ExpressionConstructorInfo (typeof (Compare), false));
		}

		private void DoLogicCondition (Game game, Save save) {
			LogicCondition condition = (LogicCondition) save.CurrentLogic;

			//evaluate the condition
			bool result = bool.Parse (condition.Condition.Evaluate (game, save));

			//get the new list depending on result
			LogicList newList = result ? condition.TrueLogicList : condition.FalseLogicList;

			//set the prev and next logic to be the same as the condition's
			newList.Prev = condition.Prev;
			newList.Next = condition.GetNextLogic ();

			save.CurrentLogic = newList;

			this.Overlord.Step (game, save);
		}
	}
}
