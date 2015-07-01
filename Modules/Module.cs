using AnyGameEngine.GameData;
using AnyGameEngine.SaveData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace AnyGameEngine.Modules {
	public class Module {
		protected Overlord Overlord;
		
		public Module (Overlord overlord) {
			this.Overlord = overlord;
		}

		public virtual void RegisterLogicHandlers (Overlord overlord) {

		}

		public virtual void RegisterLogicConstructors (Overlord overlord) {

		}

		public virtual void RegisterExpressionConstructors (Overlord overlord) {

		}

		public virtual void LoadGame (Game game, Overlord overlord, XmlNode root) {

		}
	}
}
