using AnyGameEngine.GameData;
using AnyGameEngine.Modules.Expressions.Functions;
using AnyGameEngine.SaveData;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Xml;

namespace AnyGameEngine.Modules.Expressions {
	public class Expression:IEvaluate {
		private List <ExpressionToken> tokens = new List <ExpressionToken> ();

		public Expression () {
			
		}

		public Expression (XmlNode node, Dictionary <string, ExpressionConstructorInfo> expressionConstructorInfos) {
			for (int i = 0; i < node.ChildNodes.Count; i++) {
				XmlNode childNode = node.ChildNodes [i];
				ExpressionToken token = CreateExpressionToken (childNode, expressionConstructorInfos);

				if (token is Function) {
					Function func = (Function) token;

					for (int j = 0; j < childNode.ChildNodes.Count; j++) {
						func.Tokens.Add (CreateExpressionToken (childNode.ChildNodes [j], expressionConstructorInfos));
					}
				}

				tokens.Add (token);
			}
		}

		public string Evaluate (Game game, Save save) {
			return tokens [0].Evaluate (game, save);
		}

		private ExpressionToken CreateExpressionToken (XmlNode node, Dictionary <string, ExpressionConstructorInfo> expressionConstructorInfos) {
			return (ExpressionToken) Activator.CreateInstance (expressionConstructorInfos [node.Name].Type, new object [] {node});
		}
	}
}
