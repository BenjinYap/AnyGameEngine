using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace AnyGameEngine.Other {
	public static class Helper {
		public static T Find <T> (this ObservableCollection <T> collection, Predicate <T> match) {
			foreach (T item in collection) {
				if (match (item)) {
					return item;
				}
			}

			return default (T);
		}

		public static int FindIndex <T> (this ObservableCollection <T> collection, Predicate <T> match) {
			for (int i = 0; i < collection.Count; i++) {
				if (match (collection [i])) {
					return i;
				}
			}

			return -1;
		}
	}
}
