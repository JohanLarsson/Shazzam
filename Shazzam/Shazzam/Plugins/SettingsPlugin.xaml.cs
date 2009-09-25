using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace Shazzam.Plugins {
	/// <summary>
	/// Interaction logic for SettingsPlugin.xaml
	/// </summary>
	public partial class SettingsPlugin : UserControl {
		public SettingsPlugin() {
			InitializeComponent();
		}

		private void ChooseFxcLocation_Click(object sender, RoutedEventArgs e) {
			var ofd = new Microsoft.Win32.OpenFileDialog();
			ofd.Filter = "DirectX compiler|*.exe|All Files|*.*";

			if (Properties.Settings.Default.DirectX_FxcPath != string.Empty)
			{
				string fxcPath = Environment.ExpandEnvironmentVariables(Properties.Settings.Default.DirectX_FxcPath);
				ofd.InitialDirectory = System.IO.Path.GetDirectoryName(fxcPath);
			}
			if (ofd.ShowDialog() == true)
			{
				Properties.Settings.Default.DirectX_FxcPath = ofd.FileName;
				Properties.Settings.Default.Save();
			}
		}

		private void RestoreDefaultFxcLocation_Click(object sender, RoutedEventArgs e) {
			Properties.Settings.Default.DirectX_FxcPath = @"%DXSDK_DIR%\Utilities\bin\x86\fxc.exe";
			Properties.Settings.Default.Save();
		}
	}
}
