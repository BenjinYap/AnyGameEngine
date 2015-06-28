using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace AnyGameEngine.Modules.Expressions {
	public abstract class ExpressionToken:IEvaluate {

		public abstract string Evaluate ();
	}
}
