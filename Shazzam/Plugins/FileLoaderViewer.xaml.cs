namespace Shazzam.Plugins
{
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Controls;
    using Shazzam.Views;

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
            var file = Path.GetFileName(Properties.Settings.Default.FilePath_LastFx);
            switch (Properties.Settings.Default.TabIndex_LastLoader)
            {
                case 0:
                    this.fileListBox.SetCurrentValue(System.Windows.Controls.Primitives.Selector.SelectedItemProperty, file);
                    this.fileListBox.ScrollIntoView(this.fileListBox.SelectedItem);
                    break;
                case 1:
                    this.sampleListBox.SetCurrentValue(System.Windows.Controls.Primitives.Selector.SelectedItemProperty, file);
                    this.sampleListBox.ScrollIntoView(this.sampleListBox.SelectedItem);
                    break;
                case 2:
                    this.tutorialListBox.SetCurrentValue(System.Windows.Controls.Primitives.Selector.SelectedItemProperty, file);
                    this.tutorialListBox.ScrollIntoView(this.tutorialListBox.SelectedItem);
                    break;
            }

            this.sampleListBox.SelectionChanged += this.SampleListBoxSelectionChanged;
            this.fileListBox.SelectionChanged += this.FileListBoxSelectionChanged;
            this.tutorialListBox.SelectionChanged += this.TutorialListBoxSelectionChanged;
            this.loaderTab.SelectionChanged += this.LoaderTabSelectionChanged;
        }

        public CodeTabView CodeTabView { get; set; }

        public void Update()
        {
            this.fileListBox.SetCurrentValue(System.Windows.Controls.Primitives.Selector.SelectedItemProperty, null);
            this.sampleListBox.SetCurrentValue(System.Windows.Controls.Primitives.Selector.SelectedItemProperty, null);
            this.FillList();
            this.FillSampleList();
            this.FillTutorialList();
        }

        private void LoaderTabSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Properties.Settings.Default.TabIndex_LastLoader = this.loaderTab.SelectedIndex;
            Properties.Settings.Default.Save();
        }

        private void FileListBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.fileListBox.SelectedItem == null)
            {
                return;
            }

            var path = Path.Combine(Properties.Settings.Default.FolderPath_FX, this.fileListBox.SelectedItem.ToString());

            ShazzamSwitchboard.CodeTabView.OpenFile(path);
            Properties.Settings.Default.FilePath_LastFx = path;
            Properties.Settings.Default.Save();
        }

        private void HyperlinkClick(object sender, RoutedEventArgs e)
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

        private void SampleListBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var samplesPath = Path.Combine(this.exePath, "samples");
            if (this.sampleListBox.SelectedItem == null)
            {
                return;
            }

            var path = Path.Combine(samplesPath, this.sampleListBox.SelectedItem.ToString());

            ShazzamSwitchboard.CodeTabView.OpenFile(path);
            Properties.Settings.Default.FilePath_LastFx = path;
            Properties.Settings.Default.Save();
        }

        private void TutorialListBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var tutorialsPath = Path.Combine(this.exePath, "tutorials");
            if (this.tutorialListBox.SelectedItem == null)
            {
                return;
            }

            var path = Path.Combine(tutorialsPath, this.tutorialListBox.SelectedItem.ToString());

            ShazzamSwitchboard.CodeTabView.OpenFile(path);
            Properties.Settings.Default.FilePath_LastFx = path;
            Properties.Settings.Default.Save();
        }

        private void LocationHyperlinkMouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            this.locationHyperlink.SetCurrentValue(FrameworkContentElement.ToolTipProperty, Properties.Settings.Default.FolderPath_FX);
        }

        private void FillList()
        {
            if (Directory.Exists(Properties.Settings.Default.FolderPath_FX))
            {
                this.fileListBox.SetCurrentValue(ItemsControl.ItemsSourceProperty, Directory.GetFiles(Properties.Settings.Default.FolderPath_FX, "*.fx").Select(Path.GetFileName));
            }
        }

        private void FillSampleList()
        {
            var path = Path.Combine(this.exePath, "samples");
            if (Directory.Exists(path))
            {
                this.sampleListBox.SetCurrentValue(ItemsControl.ItemsSourceProperty, Directory.GetFiles(path, "*.fx").Select(Path.GetFileName));
            }
        }

        private void FillTutorialList()
        {
            var path = Path.Combine(this.exePath, "tutorials");
            if (Directory.Exists(path))
            {
                this.tutorialListBox.SetCurrentValue(ItemsControl.ItemsSourceProperty, Directory.GetFiles(path, "*.fx").Select(Path.GetFileName));
            }
        }
    }
}
