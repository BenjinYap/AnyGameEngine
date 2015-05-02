using AnyGameEngine.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnyGameEngine {
	public class Game {
		public string Name;
		public string Description;
		public string Author;
		public string StartingRoomId;

		public List <Room> Rooms = new List <Room> ();

		public override string ToString () {
			return string.Format ("Game: {0}, Author: {1}", this.Name, this.Author);
		}
	}
}
