using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace AnyGameEngine.Other {
	public class ObservableDictionary <T1, T2>:IDictionary <T1, T2>, INotifyPropertyChanged {
		public event PropertyChangedEventHandler PropertyChanged;

		private Dictionary <T1, T2> dict = new Dictionary <T1,T2> ();

		public T2 this [T1 key] {
			get { return this.dict [key]; }
			set {
				if (this.dict.ContainsKey (key) == false || this.dict [key].Equals (value) == false) {
					this.dict [key] = value;
					this.NotifyPropertyChanged (key.ToString ());
				}
			}
		}

		private void NotifyPropertyChanged ([CallerMemberName] string propertyName = null) {
			if (this.PropertyChanged != null) {
				this.PropertyChanged (this, new PropertyChangedEventArgs (propertyName));
			}
		}

		public void Add (T1 key, T2 value) {
			throw new NotImplementedException ();
		}

		public bool ContainsKey (T1 key) {
			throw new NotImplementedException ();
		}

		public ICollection<T1> Keys {
			get { throw new NotImplementedException (); }
		}

		public bool Remove (T1 key) {
			throw new NotImplementedException ();
		}

		public bool TryGetValue (T1 key, out T2 value) {
			throw new NotImplementedException ();
		}

		public ICollection<T2> Values {
			get { throw new NotImplementedException (); }
		}

		public void Add (KeyValuePair<T1, T2> item) {
			throw new NotImplementedException ();
		}

		public void Clear () {
			throw new NotImplementedException ();
		}

		public bool Contains (KeyValuePair<T1, T2> item) {
			throw new NotImplementedException ();
		}

		public void CopyTo (KeyValuePair<T1, T2> [] array, int arrayIndex) {
			throw new NotImplementedException ();
		}

		public int Count {
			get { throw new NotImplementedException (); }
		}

		public bool IsReadOnly {
			get { throw new NotImplementedException (); }
		}

		public bool Remove (KeyValuePair<T1, T2> item) {
			throw new NotImplementedException ();
		}

		public IEnumerator<KeyValuePair<T1, T2>> GetEnumerator () {
			throw new NotImplementedException ();
		}

		IEnumerator IEnumerable.GetEnumerator () {
			return this.dict.GetEnumerator ();
		}
	}
}
