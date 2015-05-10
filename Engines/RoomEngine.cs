using AnyGameEngine.Entities.Logic;
using AnyGameEngine.Entities.Logic.Actions;
using AnyGameEngine.Entities.Logic.Flow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnyGameEngine.Engines {
	public class LogicTextEventArgs {
		public string Text;

		public LogicTextEventArgs (string text) {
			this.Text = text;
		}
	}

	public class RoomEngine:Engine {
		public delegate void LogicTextEventHandler (object sender, LogicTextEventArgs e);
		public event LogicTextEventHandler Texted;
		
		private State state = State.LogicAction;
		private LogicNode currentLogic;

		public RoomEngine (Game game, Save save):base (game, save) {
		
		}

		public void Step () {
			if (this.state != State.LogicAction) {
				throw new Exception ("Bad operation");
			}

			this.currentLogic = this.save.CurrentLogic;

			if (this.currentLogic is LogicOptionList) {

			} else if (this.currentLogic is LogicIgnorePoint) {

			} else if (this.currentLogic is LogicBackUpOptionList) {

			} else if (this.currentLogic is LogicLoop) {

			} else if (this.currentLogic is LogicLoopContinue) {

			} else if (this.currentLogic is LogicLoopBreak) {

			} else if (this.currentLogic is LogicText) {
				DoLogicText ();
			} else if (this.currentLogic is LogicRoomChange) {

			}
		}

		private void DoLogicText () {
			string text = ((LogicText) this.currentLogic).Text;
			this.save.CurrentLogic = this.currentLogic.GetNextLogic ();

			if (this.Texted != null) {
				this.Texted (this, new LogicTextEventArgs (text));
			}
		}

		private enum State { LogicAction, LogicOptionList };
	}
}
