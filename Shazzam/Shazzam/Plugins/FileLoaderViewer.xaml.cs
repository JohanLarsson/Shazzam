using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace Shazzam.Views {

	public partial class FileLoaderPlugin : UserControl {
		string _exePath;
		public FileLoaderPlugin() {
			InitializeComponent();
			_exePath = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
			FillList();
			FillSampleList();
			FillTutorialList();
			ShazzamSwitchboard.FileLoaderPlugin = this;
		}
		public CodeTabView CodeTabView { get; set; }

		public void Update()
		{
			this.fileListBox.SelectedItem = null;
			this.sampleListBox.SelectedItem = null;
			FillList();
			FillSampleList();
			FillTutorialList();
		}

		private void FillList() {
			if (Directory.Exists(Properties.Settings.Default.FolderFX))
			{
				fileListBox.ItemsSource = Directory.GetFiles(Properties.Settings.Default.FolderFX, "*.fx").Select(filename => System.IO.Path.GetFileName(filename));

			}
		}

		private void fileListBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
			if (fileListBox.SelectedItem == null)
			{
				return;
			}
			string path = System.IO.Path.Combine(Properties.Settings.Default.FolderFX, fileListBox.SelectedItem.ToString());

			ShazzamSwitchboard.CodeTabView.OpenFile(path);
			Properties.Settings.Default.LastFxFile = path;
			Properties.Settings.Default.Save();
		}

		private void Hyperlink_Click(object sender, RoutedEventArgs e) {
			var ofd = new System.Windows.Forms.FolderBrowserDialog();

			if (Properties.Settings.Default.FolderFX != string.Empty)
			{
				ofd.SelectedPath = Properties.Settings.Default.FolderFX;
			}
			if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				Properties.Settings.Default.FolderFX = ofd.SelectedPath;
				Properties.Settings.Default.Save();
				FillList();
			}
		}
		private void FillSampleList() {

			string path = System.IO.Path.Combine(_exePath, "samples");
			if (Directory.Exists(path))
			{
				sampleListBox.ItemsSource = Directory.GetFiles(path, "*.fx").Select(filename => System.IO.Path.GetFileName(filename));

			}
		}

			private void FillTutorialList() {

			string path = System.IO.Path.Combine(_exePath, "tutorials");
			if (Directory.Exists(path))
			{
				tutorialListBox.ItemsSource = Directory.GetFiles(path, "*.fx").Select(filename => System.IO.Path.GetFileName(filename));

			}
		}

		private void sampleListBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
			string samplesPath = System.IO.Path.Combine(_exePath, "samples");
			if (sampleListBox.SelectedItem == null)
			{
				return;
			}
			string path = System.IO.Path.Combine(samplesPath, sampleListBox.SelectedItem.ToString());

			ShazzamSwitchboard.CodeTabView.OpenFile(path);
			Properties.Settings.Default.LastFxFile = path;
			Properties.Settings.Default.Save();
		}
		
		private void tutorialListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			string tutorialsPath = System.IO.Path.Combine(_exePath, "tutorials");
			if (tutorialListBox.SelectedItem == null)
			{
				return;
			}
			string path = System.IO.Path.Combine(tutorialsPath, tutorialListBox.SelectedItem.ToString());

			ShazzamSwitchboard.CodeTabView.OpenFile(path);
			Properties.Settings.Default.LastFxFile = path;
			Properties.Settings.Default.Save();
		}
		private void locationHyperlink_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
		{
			locationHyperlink.ToolTip = Properties.Settings.Default.FolderFX;
		}
	}
}
