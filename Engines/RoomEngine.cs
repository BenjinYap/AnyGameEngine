using AnyGameEngine.Entities.Logic;
using AnyGameEngine.Entities.Logic.Actions;
using AnyGameEngine.Entities.Logic.Flow;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
		
		public RoomEngine (Game game, Save save):base (game, save) {
		
		}

		public void Step () {
			if (this.state != State.LogicAction) {
				throw new Exception ("Bad operation");
			}

			LogicNode currentLogic = this.save.CurrentLogic;
			//logic flows
			
			if (currentLogic is LogicLoop) {
				DoLogicLoop ();
			} else if (currentLogic is LogicOptionList) {

			} else if (currentLogic is LogicIgnorePoint) {

			} else if (currentLogic is LogicBackUpOptionList) {

			} else if (currentLogic is LogicLoopContinue) {

			} else if (currentLogic is LogicLoopBreak) {

			} else if (currentLogic is LogicList) {
				this.save.CurrentLogic = currentLogic.Nodes [0];
				this.Step ();
			//logic actions
			} else if (currentLogic is LogicText) {
				DoLogicText ();
			} else if (currentLogic is LogicRoomChange) {

			}
		}

		private void DoLogicLoop () {
			LogicLoop loop = (LogicLoop) this.save.CurrentLogic;
			int repeat = loop.Repeat;
			int count = loop.Count;

			if (count < repeat) {
				loop.Count++;
				this.save.CurrentLogic = loop.Nodes [0];
			} else {
				loop.Count = 0;
				this.save.CurrentLogic = loop.GetNextLogic ();
			}

			this.Step ();
		}

		private void DoLogicText () {
			string text = ((LogicText) this.save.CurrentLogic).Text;
			this.save.CurrentLogic = this.save.CurrentLogic.GetNextLogic ();
			
			if (this.Texted != null) {
				this.Texted (this, new LogicTextEventArgs (text));
			}
		}

		private enum State { LogicAction, LogicOptionList };
	}
}
