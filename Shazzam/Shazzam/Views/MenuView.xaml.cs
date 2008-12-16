using System.Windows.Controls;
using System.Diagnostics;
using System;

namespace Shazzam.Views {

	public partial class MenuView : UserControl {
		public MenuView() {
			InitializeComponent();
		}

		private void MenuItem_Click(object sender, System.Windows.RoutedEventArgs e) {
			string path = string.Format("{0}{1}", Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),Constants.Paths.GeneratedShaders);
			Process.Start(path);
		}
	}
}
