using AnyGameEngine.Modules.Core.Logic;
using AnyGameEngine.Modules.GlobalResources;
using AnyGameEngine.Modules.Items;
using AnyGameEngine.Other;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace AnyGameEngine.SaveData {
	public class Save:INotifyPropertyChanged {
		public event PropertyChangedEventHandler PropertyChanged;

		public string CurrentRoomId;
		public LogicNode CurrentLogic;

		public ObservableDictionary <string, float> GlobalResources = new ObservableDictionary <string, float> ();

		public ObservableCollection <ItemStack> ItemStacks { get; set; }

		public Save () {
			this.ItemStacks = new ObservableCollection <ItemStack> ();
		}

		private void NotifyPropertyChanged ([CallerMemberName] string propertyName = null) {
			if (this.PropertyChanged != null) {
				this.PropertyChanged (this, new PropertyChangedEventArgs (propertyName));
			}
		}
	}
}
