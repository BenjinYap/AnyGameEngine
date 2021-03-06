﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnyGameEngine.Modules.Core {
	
	public class LogicOptionListEventArgs {
		public string Text;
		public string [] Options;

		public LogicOptionListEventArgs (string text, string [] options) {
			this.Text = text;
			this.Options = options;
		}
	}

	public class LogicTextEventArgs {
		public string Text;

		public LogicTextEventArgs (string text) {
			this.Text = text;
		}
	}

	public class LogicRoomChangeEventArgs {
		public string Name;

		public LogicRoomChangeEventArgs (string name) {
			this.Name = name;
		}
	}
}
