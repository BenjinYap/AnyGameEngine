using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnyGameEngine.Modules.CustomVars {
	public abstract class CustomVar {
		public string Name;

		public CustomVar (string name) {
			this.Name = name;
		}

		public virtual CustomVar Clone () {
			return (CustomVar) Activator.CreateInstance (Type.GetType (base.GetType ().AssemblyQualifiedName), new object [] {this.Name});
			
		}
	}
}
