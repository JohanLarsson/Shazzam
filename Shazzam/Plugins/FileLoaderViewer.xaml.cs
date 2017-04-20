namespace Shazzam.Views
{
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Controls;

    public partial class FileLoaderPlugin : UserControl
    {
        private readonly string exePath;

        public FileLoaderPlugin()
        {
            this.InitializeComponent();
            this.exePath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

            this.FillList();
            this.FillSampleList();
            this.FillTutorialList();
            ShazzamSwitchboard.FileLoaderPlugin = this;
            this.loaderTab.SelectedIndex = Properties.Settings.Default.TabIndex_LastLoader;

            // this is fragile as it depends the tab order not changing
            // rewrite to use references instead
            switch (Properties.Settings.Default.TabIndex_LastLoader)
            {
                case 0:
                    this.fileListBox.SelectedItem = Path.GetFileName(Properties.Settings.Default.FilePath_LastFx);
                    this.fileListBox.ScrollIntoView(this.fileListBox.SelectedItem);
                    break;
                case 1:
                    this.sampleListBox.SelectedItem = Path.GetFileName(Properties.Settings.Default.FilePath_LastFx);
                    this.sampleListBox.ScrollIntoView(this.sampleListBox.SelectedItem);
                    break;
                case 2:
                    this.tutorialListBox.SelectedItem = Path.GetFileName(Properties.Settings.Default.FilePath_LastFx);
                    this.tutorialListBox.ScrollIntoView(this.tutorialListBox.SelectedItem);
                    break;
                default:
                    break;
            }

            this.sampleListBox.SelectionChanged += this.sampleListBox_SelectionChanged;
            this.fileListBox.SelectionChanged += this.fileListBox_SelectionChanged;
            this.tutorialListBox.SelectionChanged += this.tutorialListBox_SelectionChanged;
            this.loaderTab.SelectionChanged += this.loaderTab_SelectionChanged;
        }

        private void loaderTab_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Properties.Settings.Default.TabIndex_LastLoader = this.loaderTab.SelectedIndex;
            Properties.Settings.Default.Save();
        }

        public CodeTabView CodeTabView { get; set; }

        public void Update()
        {
            this.fileListBox.SelectedItem = null;
            this.sampleListBox.SelectedItem = null;
            this.FillList();
            this.FillSampleList();
            this.FillTutorialList();
        }

        private void FillList()
        {
            if (Directory.Exists(Properties.Settings.Default.FolderPath_FX))
            {
                this.fileListBox.ItemsSource = Directory.GetFiles(Properties.Settings.Default.FolderPath_FX, "*.fx").Select(filename => Path.GetFileName(filename));
            }
        }

        private void fileListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.fileListBox.SelectedItem == null)
            {
                return;
            }

            string path = Path.Combine(Properties.Settings.Default.FolderPath_FX, this.fileListBox.SelectedItem.ToString());

            ShazzamSwitchboard.CodeTabView.OpenFile(path);
            Properties.Settings.Default.FilePath_LastFx = path;
            Properties.Settings.Default.Save();
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            using (var ofd = new System.Windows.Forms.FolderBrowserDialog())
            {
                if (Properties.Settings.Default.FolderPath_FX != string.Empty)
                {
                    ofd.SelectedPath = Properties.Settings.Default.FolderPath_FX;
                }

                if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    Properties.Settings.Default.FolderPath_FX = ofd.SelectedPath;
                    Properties.Settings.Default.Save();
                    this.FillList();
                }
            }
        }

        private void FillSampleList()
        {
            string path = Path.Combine(this.exePath, "samples");
            if (Directory.Exists(path))
            {
                this.sampleListBox.ItemsSource = Directory.GetFiles(path, "*.fx").Select(filename => Path.GetFileName(filename));
            }
        }

        private void FillTutorialList()
        {
            string path = Path.Combine(this.exePath, "tutorials");
            if (Directory.Exists(path))
            {
                this.tutorialListBox.ItemsSource = Directory.GetFiles(path, "*.fx").Select(filename => Path.GetFileName(filename));
            }
        }

        private void sampleListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string samplesPath = Path.Combine(this.exePath, "samples");
            if (this.sampleListBox.SelectedItem == null)
            {
                return;
            }

            string path = Path.Combine(samplesPath, this.sampleListBox.SelectedItem.ToString());

            ShazzamSwitchboard.CodeTabView.OpenFile(path);
            Properties.Settings.Default.FilePath_LastFx = path;
            Properties.Settings.Default.Save();
        }

        private void tutorialListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string tutorialsPath = Path.Combine(this.exePath, "tutorials");
            if (this.tutorialListBox.SelectedItem == null)
            {
                return;
            }

            string path = Path.Combine(tutorialsPath, this.tutorialListBox.SelectedItem.ToString());

            ShazzamSwitchboard.CodeTabView.OpenFile(path);
            Properties.Settings.Default.FilePath_LastFx = path;
            Properties.Settings.Default.Save();
        }

        private void locationHyperlink_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            this.locationHyperlink.ToolTip = Properties.Settings.Default.FolderPath_FX;
        }
    }
}
