using AnyGameEngine.GameData;
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

		public override void LoadGame (Game game, Overlord overlord, XmlNode root) {
			XmlNode vars = root ["CustomVars"];

			List <string> existingVars = new List <string> ();

			for (var i = 0; i < vars.ChildNodes.Count; i++) {
				XmlNode v = vars.ChildNodes [i];
				XmlAttributeCollection attrs = v.Attributes;
				string name = attrs ["name"].Value;

				if (existingVars.Contains (name)) {
					throw new Exception (string.Format ("CustomVar {0} already exists", name));
				}

				CustomVar customVar = null;

				if (v.Name == "CustomArrayVar") {
					string type = attrs ["type"].Value;
					string [] values = attrs ["values"].Value.Split (',');

					if (type == "number") {
						customVar = new CustomArrayVar <float> (new List <string> (values).Select (a => float.Parse (a)).ToArray ());
					} else if (type == "string") {
						customVar = new CustomArrayVar <string> (values);
					}
				} else {
					string value = attrs ["value"].Value;

					if (v.Name == "CustomNumberVar") {
						customVar = new CustomSingleVar <float> (float.Parse (value));
					} else if (v.Name == "CustomStringVar") {
						customVar = new CustomSingleVar <string> (value);
					}
				}

				customVar.Name = name;
				existingVars.Add (name);

				game.CustomVars.Add (customVar);
			} 
		}
	}
}
