using AnyGameEngine.Modules.Core.Logic;
using AnyGameEngine.Modules.Core.Logic.Flow;
using AnyGameEngine.Modules.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace AnyGameEngine.Modules.Conditions.Logic.Flow {
	public class LogicCondition:LogicNode {
		public IEvaluate Condition;

		public LogicList TrueLogicList;
		public LogicList FalseLogicList;

		public LogicCondition () {
			
		}

		public LogicCondition (XmlNode node, CreateLogicVessel vessel, Dictionary <string, ExpressionConstructorInfo> expressionConstructorInfos):base (node) {
			this.Condition = new Expression (node.ChildNodes [0], expressionConstructorInfos);
			this.TrueLogicList = (LogicList) vessel.CreateLogic (node.ChildNodes [1].ChildNodes [0]);
			this.FalseLogicList = (LogicList) vessel.CreateLogic (node.ChildNodes [2].ChildNodes [0]);
		}

		public override LogicNode Clone (LogicNode parent) {
			LogicCondition clone = (LogicCondition) base.Clone (parent);
			clone.Condition = this.Condition;
			clone.TrueLogicList = (LogicList) this.TrueLogicList.Clone (null);
			clone.FalseLogicList = (LogicList) this.FalseLogicList.Clone (null);
			return clone;
		}

		public override string ToString () {
			return null;
			//return string.Format ("Currency Modify, Amount: {0}", this.Amount);
		}
	}
}
