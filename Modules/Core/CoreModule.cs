using AnyGameEngine.Modules.GlobalResources;
using AnyGameEngine.Modules.Core.Logic;
using AnyGameEngine.Modules.Core.Logic.Actions;
using AnyGameEngine.Modules.Core.Logic.Flow;
using AnyGameEngine.SaveData;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace AnyGameEngine.Modules.Core {
	public class CoreModule:Module {
		public delegate void LogicOptionListEventHandler (object sender, LogicOptionListEventArgs e);
		public event LogicOptionListEventHandler OptionListed;

		public delegate void LogicTextEventHandler (object sender, LogicTextEventArgs e);
		public event LogicTextEventHandler Texted;

		public delegate void LogicRoomChangeEventHandler (object sender, LogicRoomChangeEventArgs e);
		public event LogicRoomChangeEventHandler RoomChanged;

		private State state = State.Action;
		
		public CoreModule (Overlord overlord):base (overlord) {
			overlord.LogicHandlers [typeof (LogicList)] = DoLogicList;
			overlord.LogicHandlers [typeof (LogicLoop)] = DoLogicLoop;
			overlord.LogicHandlers [typeof (LogicLoopContinue)] = DoLogicLoopContinue;
			overlord.LogicHandlers [typeof (LogicLoopBreak)] = DoLogicLoopBreak;
			overlord.LogicHandlers [typeof (LogicOptionList)] = DoLogicOptionList;

			overlord.LogicHandlers [typeof (LogicText)] = DoLogicText;
			overlord.LogicHandlers [typeof (LogicRoomChange)] = DoLogicRoomChange;
		}

		public void SelectOption (int index) {
			
			if (this.state != State.OptionList) {
				throw new Exception (string.Format ("Bad operation. Engine is in {0} state.", this.state));
			}

			if (index < 0 || index > this.Save.CurrentLogic.Nodes.Count - 1) {
				throw new Exception ("Option index out of bounds.");
			}

			this.state = State.Action;
			this.Save.CurrentLogic = this.Save.CurrentLogic.Nodes [index].Nodes [0];
			this.Overlord.Step ();
		}
		
		#region Do Logic Flows
		private void DoLogicList () {
			this.Save.CurrentLogic = this.Save.CurrentLogic.Nodes [0];
			this.Overlord.Step ();
		}

		private void DoLogicLoop () {
			LogicLoop loop = (LogicLoop) this.Save.CurrentLogic;
			int repeat = loop.Repeat;
			int count = loop.Count;

			if (count < repeat) {
				loop.Count++;
				this.Save.CurrentLogic = loop.Nodes [0];
			} else {
				loop.Count = 0;
				this.Save.CurrentLogic = loop.GetNextLogic ();
			}

			this.Overlord.Step ();
		}

		private void DoLogicLoopContinue () {
			this.Save.CurrentLogic = this.Save.CurrentLogic.GetParentByType (typeof (LogicLoop));
			this.Overlord.Step ();
		}

		private void DoLogicLoopBreak () {
			LogicLoop loop = (LogicLoop) this.Save.CurrentLogic.GetParentByType (typeof (LogicLoop));
			loop.Count = 0;
			this.Save.CurrentLogic = loop.GetNextLogic ();
			this.Overlord.Step ();
		}

		private void DoLogicOptionList () {
			this.state = State.OptionList;
			LogicOptionList optionList = (LogicOptionList) this.Save.CurrentLogic;
			string [] options = optionList.Nodes.Select (a => ((LogicOption) a).Text).ToArray ();

			if (this.OptionListed != null) {
				this.OptionListed (this, new LogicOptionListEventArgs (optionList.Text, options));
			}
		}

		private void DoLogicBackUpOptionList () {
			int times = ((LogicBackUpOptionList) this.Save.CurrentLogic).Times;
			LogicNode logic = this.Save.CurrentLogic;

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

			this.Save.CurrentLogic = logic;
			this.Overlord.Step ();
		}

		private void DoLogicIgnorePoint () {
			this.Save.CurrentLogic = this.Save.CurrentLogic.GetNextLogic ();
			this.Overlord.Step ();
		}
		#endregion

		#region Do Logic Actions
		private void DoLogicText () {
			string text = ((LogicText) this.Save.CurrentLogic).Text;
			this.Save.CurrentLogic = this.Save.CurrentLogic.GetNextLogic ();
			
			if (this.Texted != null) {
				this.Texted (this, new LogicTextEventArgs (text));
			}
		}

		private void DoLogicRoomChange () {
			//get the zone to change to
			string newRoomId = ((LogicRoomChange) this.Save.CurrentLogic).RoomId;
			Room room = this.Game.Rooms.Find (a => a.Id == newRoomId);
			
			//get the logic after the logiczonechange
			LogicNode nextLogic = this.Save.CurrentLogic.GetNextLogic ();

			//if there is no logic after the logiczonechange or the next logic is an ignore
			if (nextLogic == null || nextLogic is LogicIgnorePoint) {
				//set the new current logic to the first logic of the new zone
				this.Save.CurrentLogic = room.LogicList.Clone (null).Nodes [0];
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
				this.Save.CurrentLogic = newCurrentLogic;
			}

			if (this.RoomChanged != null) {
				this.RoomChanged (this, new LogicRoomChangeEventArgs (room.Name));
			}
		}
		#endregion

		private enum State { Action, OptionList };
	}
}
