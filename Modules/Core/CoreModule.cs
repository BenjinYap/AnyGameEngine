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
using AnyGameEngine.Modules.Core.ExpressionTokens;
using System.Xml;
using AnyGameEngine.Modules.Conditions.Logic.Flow;

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
			overlord.LogicConstructorInfos.Add ("LogicText", new LogicConstructorInfo (typeof (LogicText), false, true));
			overlord.LogicConstructorInfos.Add ("LogicRoomChange", new LogicConstructorInfo (typeof (LogicRoomChange), false, false));

			overlord.LogicConstructorInfos.Add ("LogicList", new LogicConstructorInfo (typeof (LogicList), true, false));
			overlord.LogicConstructorInfos.Add ("LogicOption", new LogicConstructorInfo (typeof (LogicOption), true, false));
			overlord.LogicConstructorInfos.Add ("LogicOptionList", new LogicConstructorInfo (typeof (LogicOptionList), true, false));
			overlord.LogicConstructorInfos.Add ("LogicBackUpOptionList", new LogicConstructorInfo (typeof (LogicBackUpOptionList), false, false));
			overlord.LogicConstructorInfos.Add ("LogicLoop", new LogicConstructorInfo (typeof (LogicLoop), true, false));
			overlord.LogicConstructorInfos.Add ("LogicLoopBreak", new LogicConstructorInfo (typeof (LogicLoopBreak), false, false));
			overlord.LogicConstructorInfos.Add ("LogicLoopContinue", new LogicConstructorInfo (typeof (LogicLoopContinue), false, false));
			overlord.LogicConstructorInfos.Add ("LogicIgnorePoint", new LogicConstructorInfo (typeof (LogicIgnorePoint), false, false));
		}

		public override void RegisterLogicHandlers (Overlord overlord) {
			overlord.LogicHandlers [typeof (LogicList)] = DoLogicList;
			overlord.LogicHandlers [typeof (LogicLoop)] = DoLogicLoop;
			overlord.LogicHandlers [typeof (LogicLoopContinue)] = DoLogicLoopContinue;
			overlord.LogicHandlers [typeof (LogicLoopBreak)] = DoLogicLoopBreak;
			overlord.LogicHandlers [typeof (LogicOptionList)] = DoLogicOptionList;
			overlord.LogicHandlers [typeof (LogicBackUpOptionList)] = DoLogicBackUpOptionList;

			overlord.LogicHandlers [typeof (LogicText)] = DoLogicText;
			overlord.LogicHandlers [typeof (LogicRoomChange)] = DoLogicRoomChange;
		}

		public override void RegisterExpressionConstructors (Overlord overlord) {
			overlord.ExpressionConstructorInfos.Add ("RoomValue", new ExpressionConstructorInfo (typeof (RoomValue), true));
		}

		public override void LoadGame (Game game, Overlord overlord, XmlNode root) {
			LoadGeneral (game, root);
			LoadRooms (game, root ["Rooms"], overlord);
			LoadLogic (game, root ["Logic"], overlord);
		}

		private void LoadGeneral (Game game, XmlNode node) {
			XmlAttributeCollection attrs = node.Attributes;
			game.Name = attrs ["name"].Value;
			game.Description = attrs ["description"].Value;
			game.Author = attrs ["author"].Value;
			game.StartingRoomId = attrs ["startingRoomId"].Value;
		}

		private LogicNode CreateLogic (XmlNode node, Overlord overlord) {
			if (overlord.LogicConstructorInfos.ContainsKey (node.Name) == false) {
				throw new Exception ("Logic class not found for " + node.Name);
			}

			List <object> args = new List <Object> {node};

			if (overlord.LogicConstructorInfos [node.Name].ContainsLogic) {
				args.Add (new CreateLogicVessel (CreateLogic, overlord));
			}

			if (overlord.LogicConstructorInfos [node.Name].ContainsExpressions) {
				args.Add (overlord.ExpressionConstructorInfos);
			}

			LogicNode logic = (LogicNode) Activator.CreateInstance (overlord.LogicConstructorInfos [node.Name].Type, args.ToArray ());
			return logic;
		}

		private void LoadRooms (Game game, XmlNode node, Overlord overlord) {
			List <string> existingRooms = new List <string> ();
			
			for (int i = 0; i < node.ChildNodes.Count; i++) {
				XmlNode n = node.ChildNodes [i];
				XmlAttributeCollection attrs = n.Attributes;
				Room room = new Room ();
				room.Id = attrs ["id"].Value;
				room.Name = attrs ["name"].Value;
				room.LogicList = (LogicList) CreateLogic (n.ChildNodes [0], overlord);
				
				existingRooms.Add (room.Id);
				
				game.Rooms.Add (room);
			}
		}

		private void LoadLogic (Game game, XmlNode node, Overlord overlord) {
			for (int i = 0; i < node.ChildNodes.Count; i++) {
				XmlNode n = node.ChildNodes [i];
				game.Logic.Add (CreateLogic (n, overlord));
			}
		}

		public void SelectOption (Game game, Save save, int index) {
			if (this.state != State.OptionList) {
				throw new Exception (string.Format ("Bad operation. Engine is in {0} state.", this.state));
			}

			LogicList list = (LogicList) save.CurrentLogic;

			if (index < 0 || index > list.Nodes.Count - 1) {
				throw new Exception ("Option index out of bounds.");
			}
			
			this.state = State.Action;
			this.Overlord.EnableStep ();
			save.CurrentLogic = ((LogicList) (list.Nodes [index])).Nodes [0];
			this.Overlord.Step (game, save);
		}
		
		private void DoLogicList (Game game, Save save) {
			LogicList list = (LogicList) save.CurrentLogic;

			if (list.Nodes.Count > 0) {
				save.CurrentLogic = list.Nodes [0];
			} else {
				save.CurrentLogic = save.CurrentLogic.GetNextLogic ();
			}

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
			string text = ((LogicText) save.CurrentLogic).Text.Evaluate (game, save);
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
				save.CurrentLogic = room.LogicList.Clone (null);
			} else {  //if there is logic after the logiczonechange
				//the first logic in the new chain
				LogicNode newCurrentLogic = nextLogic;

				//the final logic in the old chain
				LogicNode finalLogic = nextLogic;

				//get the final logic in the old chain
				while (true) {
					nextLogic = finalLogic.GetNextLogic ();

					if (nextLogic == null || nextLogic is LogicIgnorePoint) {
						break;
					}

					finalLogic = nextLogic;
				}
				
				//clone the new zone into the new chain
				finalLogic.Next = room.LogicList.Clone (finalLogic.Parent);
				finalLogic.Next.Prev = finalLogic.Next;

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
