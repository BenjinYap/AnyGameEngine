using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace AnyGameEngine.Modules.Items {
	public class ItemStack:INotifyPropertyChanged {
		public event PropertyChangedEventHandler PropertyChanged;

		public string ItemId;
		
		public int Quantity {
			get { return this.quantity; }
			set {
				if (this.quantity != value) {
					this.quantity = value;
					this.NotifyPropertyChanged ();
				}
			}
		}

		private int quantity; 

		public ItemStack (string itemId) {
			this.ItemId = itemId;
		}

		public ItemStack (string itemId, int quantity) {
			this.ItemId = itemId;
			this.Quantity = quantity;
		}

		private void NotifyPropertyChanged ([CallerMemberName] string propertyName = null) {
			if (this.PropertyChanged != null) {
				this.PropertyChanged (this, new PropertyChangedEventArgs (propertyName));
			}
		}
	}
}
