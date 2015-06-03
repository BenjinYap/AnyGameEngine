using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnyGameEngine.Other {
	public class UniqueList <T> {
		private string exceptionMessage;
		private HashSet <T> values = new HashSet <T> ();

		/// <summary>
		/// The exception message for when a duplicate item is added.
		/// </summary>
		/// <param name="exceptionMessage">The message. {{}} will be replaced with the item.</param>
		public UniqueList (string exceptionMessage) {
			this.exceptionMessage = exceptionMessage;
		}

		public void Add (T item) {
			if (this.values.Add (item) == false) {
				throw new Exception (this.exceptionMessage.Replace ("{{}}", item.ToString ()));
			}
		}
	}
}
