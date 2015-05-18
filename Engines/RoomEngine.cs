using AnyGameEngine.Entities;
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
		public delegate void LogicOptionListEventHandler (object sender, LogicOptionListEventArgs e);
		public event LogicOptionListEventHandler OptionListed;

		public delegate void LogicTextEventHandler (object sender, LogicTextEventArgs e);
		public event LogicTextEventHandler Texted;

		public delegate void LogicRoomChangeEventHandler (object sender, LogicRoomChangeEventArgs e);
		public event LogicRoomChangeEventHandler RoomChanged;

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
			} else if (currentLogic is LogicBackUpOptionList) {
				DoLogicBackUpOptionList ();
			} else if (currentLogic is LogicIgnorePoint) {
				DoLogicIgnorePoint ();
			} else if (currentLogic is LogicList) {
				this.save.CurrentLogic = currentLogic.Nodes [0];
				Step ();
			//logic actions
			} else if (currentLogic is LogicText) {
				DoLogicText ();
			} else if (currentLogic is LogicRoomChange) {
				DoLogicRoomChange ();
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

		private void DoLogicBackUpOptionList () {
			int times = ((LogicBackUpOptionList) this.save.CurrentLogic).Times;
			LogicNode logic = this.save.CurrentLogic;

			for (var i = 0; i < times; i++) {
				while (true) {
					if (logic.Parent == null) {
						throw new Exception ("bad happened");
					}

					logic = logic.Parent;

					if (logic is LogicOptionList) {
						break;
					}
				}
			}

			this.save.CurrentLogic = logic;
			Step ();
		}

		private void DoLogicIgnorePoint () {
			this.save.CurrentLogic = this.save.CurrentLogic.GetNextLogic ();
			this.Step ();
		}
		#endregion

		#region Do Logic Actions
		private void DoLogicText () {
			string text = ((LogicText) this.save.CurrentLogic).Text;
			this.save.CurrentLogic = this.save.CurrentLogic.GetNextLogic ();
			
			if (this.Texted != null) {
				this.Texted (this, new LogicTextEventArgs (text));
			}
		}

		private void DoLogicRoomChange () {
			//get the zone to change to
			string newRoomId = ((LogicRoomChange) this.save.CurrentLogic).RoomId;
			Room room = this.game.Rooms.Find (a => a.Id == newRoomId);
			
			//get the logic after the logiczonechange
			LogicNode nextLogic = this.save.CurrentLogic.GetNextLogic ();

			//if there is no logic after the logiczonechange or the next logic is an ignore
			if (nextLogic == null || nextLogic is LogicIgnorePoint) {
				//set the new current logic to the first logic of the new zone
				this.save.CurrentLogic = room.LogicList.Clone (null).Nodes [0];
			} else {  //if there is logic after the logiczonechange
				//the first logic in the new chain
				LogicNode newCurrentLogic = nextLogic.Clone (null);

				//reference to the previous logic in the new chain
				LogicNode prevLogicNew = newCurrentLogic;

				//reference to the previous logic in the old chain
				LogicNode prevLogic = nextLogic;

				while (true) {
					//get the next logic in the old chain
					nextLogic = prevLogic.GetNextLogic ();

					//stop if there's nothing or it's an ignore
					if (nextLogic == null || nextLogic is LogicIgnorePoint) {
						break;
					}

					//clone the old chain into the new chain
					prevLogicNew.Next = nextLogic.Clone (null);
					prevLogicNew.Next.Prev = prevLogicNew;

					//reset the old and new references
					prevLogicNew = prevLogicNew.Next;
					prevLogic = nextLogic;
				}

				//clone the new zone into the new chain
				prevLogicNew.Next = room.LogicList.Clone (null).Nodes [0];
				prevLogicNew.Next.Prev = prevLogicNew;

				//finally set the new logic
				this.save.CurrentLogic = newCurrentLogic;
			}

			if (this.RoomChanged != null) {
				this.RoomChanged (this, new LogicRoomChangeEventArgs (room.Name));
			}
		}
		#endregion

		private enum State { LogicAction, LogicOptionList };
	}
}
