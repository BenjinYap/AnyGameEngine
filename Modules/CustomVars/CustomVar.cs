using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnyGameEngine.Modules.CustomVars {
	public abstract class CustomVar {
		public string Name;

		public abstract CustomVar Clone ();
	}
}
