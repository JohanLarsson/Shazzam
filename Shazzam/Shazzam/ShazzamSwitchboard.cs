using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Shazzam.Views;
using System.Windows;
using System.ComponentModel;

namespace Shazzam {
	class ShazzamSwitchboard {

		private static Window _MainWindow;
		public static Window MainWindow {
			get { return _MainWindow; }
			set {
				_MainWindow = value;
				NotifyPropertyChanged("MainWindow");
			}
		}
		private static CodeTabView _codeTabView;
		public static CodeTabView CodeTabView {
			get { return _codeTabView; }
			set {
				_codeTabView = value;
				NotifyPropertyChanged("CodeTabView");
			}
		}

		#region INotifyPropertyChanged

		public static event PropertyChangedEventHandler PropertyChanged;

		private static void NotifyPropertyChanged(String info) {
			if (PropertyChanged != null)
			{
				PropertyChanged(null, new PropertyChangedEventArgs(info));
			}
		}

		#endregion
		public static Boolean IsFXCompilerAvailable() {
			if (System.IO.Path.GetFileName(Properties.Settings.Default.DirectX_FxcPath).ToLower() == "fxc.exe")
			{
				if (System.IO.File.Exists(Properties.Settings.Default.DirectX_FxcPath))
				{
					return true;
				}
			}
			return false;
		}
	}

}
