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

	public class LogicOptionListEventArgs {
		public string Text;
		public string [] Options;

		public LogicOptionListEventArgs (string text, string [] options) {
			this.Text = text;
			this.Options = options;
		}
	}
}
