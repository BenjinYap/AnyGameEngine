

using AnyGameEngine.Modules.Core.Logic;
using System;
using System.Xml;
namespace AnyGameEngine {
	public class CreateLogicVessel {
		private Overlord overlord;
		private Func <XmlNode, Overlord, LogicNode> func;

		public CreateLogicVessel (Func <XmlNode, Overlord, LogicNode> createLogic, Overlord overlord) {
			this.overlord = overlord;
			this.func = createLogic;
		}

		public LogicNode CreateLogic (XmlNode node) {
			return this.func (node, this.overlord);
		}
	}
}
