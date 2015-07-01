﻿using AnyGameEngine.GameData;
using AnyGameEngine.Modules.CustomVars.ExpressionTokens;
using AnyGameEngine.Other;
using AnyGameEngine.SaveData;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml;

namespace AnyGameEngine.Modules.CustomVars {
	public class CustomVarsModule:Module {
		
		public CustomVarsModule (Overlord overlord):base (overlord) {
			
		}

		public override void RegisterExpressionConstructors (Overlord overlord) {
			overlord.ExpressionConstructorInfos.Add ("CustomSingleVarValue", new ExpressionConstructorInfo (typeof (CustomSingleVarValue), true));
		}

		public override void LoadGame (Game game, Overlord overlord, XmlNode root) {
			XmlNode vars = root ["CustomVars"];

			UniqueList <string> existing = new UniqueList <string> ("Duplicate custom var {{}}");

			for (var i = 0; i < vars.ChildNodes.Count; i++) {
				XmlNode v = vars.ChildNodes [i];
				XmlAttributeCollection attrs = v.Attributes;
				string name = attrs ["name"].Value;

				existing.Add (name);

				CustomVar customVar = null;

				if (v.Name == "CustomArrayVar") {
					string [] values = attrs ["values"].Value.Split (',');
					customVar = new CustomArrayVar (values);
				} else {
					string value = attrs ["value"].Value;
					customVar = new CustomSingleVar (value);
				}

				customVar.Name = name;

				game.CustomVars.Add (customVar);
			} 
		}
	}
}
