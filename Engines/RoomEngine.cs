using AnyGameEngine.Entities.Logic;
using AnyGameEngine.Entities.Logic.Actions;
using AnyGameEngine.Entities.Logic.Flow;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace AnyGameEngine.Engines {
	public class RoomEngine:Engine {
		public delegate void LogicTextEventHandler (object sender, LogicTextEventArgs e);
		public event LogicTextEventHandler Texted;
		
		public delegate void LogicOptionListEventHandler (object sender, LogicOptionListEventArgs e);
		public event LogicOptionListEventHandler OptionListed;

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
			} else if (currentLogic is LogicLoopContinue) {
				DoLogicLoopContinue ();
			} else if (currentLogic is LogicLoopBreak) {
				DoLogicLoopBreak ();
			} else if (currentLogic is LogicOptionList) {
				DoLogicOptionList ();
			} else if (currentLogic is LogicIgnorePoint) {

			} else if (currentLogic is LogicBackUpOptionList) {

				
			} else if (currentLogic is LogicList) {
				this.save.CurrentLogic = currentLogic.Nodes [0];
				Step ();
			//logic actions
			} else if (currentLogic is LogicText) {
				DoLogicText ();
			} else if (currentLogic is LogicRoomChange) {

			}
		}

		public void SelectOption (int index) {
			if (this.state != State.LogicOptionList) {
				throw new Exception (string.Format ("Bad operation. Engine is in {0} state.", this.state));
			}

			if (index < 0 || index > this.save.CurrentLogic.Nodes.Count - 1) {
				throw new Exception ("Option index out of bounds.");
			}

			this.state = State.LogicAction;
			this.save.CurrentLogic = this.save.CurrentLogic.Nodes [index].Nodes [0];
			Step ();
		}
		
		#region Do Logic Flows
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

			Step ();
		}

		private void DoLogicLoopContinue () {
			this.save.CurrentLogic = this.save.CurrentLogic.GetParentByType (typeof (LogicLoop));
			Step ();
		}

		private void DoLogicLoopBreak () {
			LogicLoop loop = (LogicLoop) this.save.CurrentLogic.GetParentByType (typeof (LogicLoop));
			loop.Count = 0;
			this.save.CurrentLogic = loop.GetNextLogic ();
			Step ();
		}

		private void DoLogicOptionList () {
			this.state = State.LogicOptionList;
			LogicOptionList optionList = (LogicOptionList) this.save.CurrentLogic;
			string [] options = optionList.Nodes.Select (a => ((LogicOption) a).Text).ToArray ();

			if (this.OptionListed != null) {
				this.OptionListed (this, new LogicOptionListEventArgs (optionList.Text, options));
			}
		}

		//private void DoLogicBackUpOptionList () {
		//	var times = this.save.currentLogic.times;
		//	var logic = this.save.currentLogic;

		//	for (var i = 0; i < times; i++) {
		//		while (true) {
		//			if (logic.parent === null) {
		//				throw 'bad happened';
		//			}

		//			logic = logic.parent;

		//			if (logic instanceof LogicOptionList) {
		//				break;
		//			}
		//		}
		//	}

		//	this.save.currentLogic = logic;
		//	this.step ();
		//}
		#endregion

		#region Do Logic Actions
		private void DoLogicText () {
			string text = ((LogicText) this.save.CurrentLogic).Text;
			this.save.CurrentLogic = this.save.CurrentLogic.GetNextLogic ();
			
			if (this.Texted != null) {
				this.Texted (this, new LogicTextEventArgs (text));
			}
		}
		#endregion

		private enum State { LogicAction, LogicOptionList };
	}
}
