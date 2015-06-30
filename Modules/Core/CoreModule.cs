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
using AnyGameEngine.GameData;
using AnyGameEngine.Modules.Expressions;

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
			
		}

		public override void RegisterLogicConstructors (Overlord overlord) {
			overlord.LogicConstructorInfos.Add ("LogicText", new LogicConstructorInfo (typeof (LogicText), true));
			overlord.LogicConstructorInfos.Add ("LogicRoomChange", new LogicConstructorInfo (typeof (LogicRoomChange), false));

			overlord.LogicConstructorInfos.Add ("LogicList", new LogicConstructorInfo (typeof (LogicList), false));
			overlord.LogicConstructorInfos.Add ("LogicOption", new LogicConstructorInfo (typeof (LogicOption), false));
			overlord.LogicConstructorInfos.Add ("LogicOptionList", new LogicConstructorInfo (typeof (LogicOptionList), false));
			overlord.LogicConstructorInfos.Add ("LogicBackUpOptionList", new LogicConstructorInfo (typeof (LogicBackUpOptionList), false));
			overlord.LogicConstructorInfos.Add ("LogicLoop", new LogicConstructorInfo (typeof (LogicLoop), false));
			overlord.LogicConstructorInfos.Add ("LogicLoopBreak", new LogicConstructorInfo (typeof (LogicLoopBreak), false));
			overlord.LogicConstructorInfos.Add ("LogicLoopContinue", new LogicConstructorInfo (typeof (LogicLoopContinue), false));
			overlord.LogicConstructorInfos.Add ("LogicIgnorePoint", new LogicConstructorInfo (typeof (LogicIgnorePoint), false));
		}

		public override void RegisterLogicHandlers (Overlord overlord) {
			overlord.LogicHandlers [typeof (LogicList)] = DoLogicList;
			overlord.LogicHandlers [typeof (LogicLoop)] = DoLogicLoop;
			overlord.LogicHandlers [typeof (LogicLoopContinue)] = DoLogicLoopContinue;
			overlord.LogicHandlers [typeof (LogicLoopBreak)] = DoLogicLoopBreak;
			overlord.LogicHandlers [typeof (LogicOptionList)] = DoLogicOptionList;

			overlord.LogicHandlers [typeof (LogicText)] = DoLogicText;
			overlord.LogicHandlers [typeof (LogicRoomChange)] = DoLogicRoomChange;
		}

		public void SelectOption (Game game, Save save, int index) {
			if (this.state != State.OptionList) {
				throw new Exception (string.Format ("Bad operation. Engine is in {0} state.", this.state));
			}

			if (index < 0 || index > save.CurrentLogic.Nodes.Count - 1) {
				throw new Exception ("Option index out of bounds.");
			}

			this.state = State.Action;
			this.Overlord.EnableStep ();
			save.CurrentLogic = save.CurrentLogic.Nodes [index].Nodes [0];
			this.Overlord.Step (game, save);
		}
		
		private void DoLogicList (Game game, Save save) {
			save.CurrentLogic = save.CurrentLogic.Nodes [0];
			this.Overlord.Step (game, save);
		}

		private void DoLogicLoop (Game game, Save save) {
			LogicLoop loop = (LogicLoop) save.CurrentLogic;
			int repeat = loop.Repeat;
			int count = loop.Count;

			if (count < repeat) {
				loop.Count++;
				save.CurrentLogic = loop.Nodes [0];
			} else {
				loop.Count = 0;
				save.CurrentLogic = loop.GetNextLogic ();
			}

			this.Overlord.Step (game, save);
		}

		private void DoLogicLoopContinue (Game game, Save save) {
			save.CurrentLogic = save.CurrentLogic.GetParentByType (typeof (LogicLoop));
			this.Overlord.Step (game, save);
		}

		private void DoLogicLoopBreak (Game game, Save save) {
			LogicLoop loop = (LogicLoop) save.CurrentLogic.GetParentByType (typeof (LogicLoop));
			loop.Count = 0;
			save.CurrentLogic = loop.GetNextLogic ();
			this.Overlord.Step (game, save);
		}

		private void DoLogicOptionList (Game game, Save save) {
			this.state = State.OptionList;
			this.Overlord.DisableStep ("can't step in option list state");
			LogicOptionList optionList = (LogicOptionList) save.CurrentLogic;
			string [] options = optionList.Nodes.Select (a => ((LogicOption) a).Text).ToArray ();

			if (this.OptionListed != null) {
				this.OptionListed (this, new LogicOptionListEventArgs (optionList.Text, options));
			}
		}

		private void DoLogicBackUpOptionList (Game game, Save save) {
			int times = ((LogicBackUpOptionList) save.CurrentLogic).Times;
			LogicNode logic = save.CurrentLogic;

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

			save.CurrentLogic = logic;
			this.Overlord.Step (game, save);
		}

		private void DoLogicIgnorePoint (Game game, Save save) {
			save.CurrentLogic = save.CurrentLogic.GetNextLogic ();
			this.Overlord.Step (game, save);
		}
		
		private void DoLogicText (Game game, Save save) {
			string text = ((LogicText) save.CurrentLogic).Text.Evaluate ();
			save.CurrentLogic = save.CurrentLogic.GetNextLogic ();
			
			if (this.Texted != null) {
				this.Texted (this, new LogicTextEventArgs (text));
			}
		}

		private void DoLogicRoomChange (Game game, Save save) {
			//get the zone to change to
			string newRoomId = ((LogicRoomChange) save.CurrentLogic).RoomId;
			Room room = game.Rooms.Find (a => a.Id == newRoomId);
			
			//get the logic after the logiczonechange
			LogicNode nextLogic = save.CurrentLogic.GetNextLogic ();

			//if there is no logic after the logiczonechange or the next logic is an ignore
			if (nextLogic == null || nextLogic is LogicIgnorePoint) {
				//set the new current logic to the first logic of the new zone
				save.CurrentLogic = room.LogicList.Clone (null).Nodes [0];
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
				save.CurrentLogic = newCurrentLogic;
			}

			if (this.RoomChanged != null) {
				this.RoomChanged (this, new LogicRoomChangeEventArgs (room.Name));
			}
		}
		
		//private const string state = "coremodule.state";

		private enum State { Action, OptionList };
	}
}
