namespace Shazzam
{
    using System;
    using System.IO;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media.Effects;
    using System.Windows.Media.Imaging;
    using Microsoft.Win32;
    using Shazzam.Helpers;

    // Images
    //  creative commons license
    //  StuffEyeSee  http://www.flickr.com/photos/rcsaxon/689732379/
    //  http://www.flickr.com/photos/glockenblume/2228713567/sizes/l/
    //  http://www.flickr.com/photos/96dpi/2329024258/
    //  http://www.flickr.com/photos/pachytime/2554307339/
    //  http://www.flickr.com/photos/madram/492839665/
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            Commands.AppCommands.Initialize();
            this.InitializeComponent();

            ShazzamSwitchboard.MainWindow = this;
            ShazzamSwitchboard.CodeTabView = this.CodeTabView;
            this.CodeTabView.ShaderEffectChanged += this.CodeTabViewShaderEffectChanged;
            this.ImageTabControl.SelectionChanged += this.CodeTabControlSelectionChanged;

            if (Properties.Settings.Default.FilePath_LastImage != string.Empty)
            {
                if (File.Exists(Properties.Settings.Default.FilePath_LastImage))
                {
                    this.LoadImage(Properties.Settings.Default.FilePath_LastImage);
                }
                else
                {
                    var resourceUri = new Uri("images/ColorRange.png", UriKind.Relative);
                    var streamInfo = Application.GetResourceStream(resourceUri);

                    if (streamInfo != null)
                    {
                        var temp = BitmapFrame.Create(streamInfo.Stream);
                        this.UserImage.SetCurrentValue(Image.SourceProperty, temp);
                    }
                }
            }

            if (Properties.Settings.Default.FilePath_LastMedia != string.Empty)
            {
                if (File.Exists(Properties.Settings.Default.FilePath_LastMedia))
                {
                    this.LoadMedia(Properties.Settings.Default.FilePath_LastMedia);
                }
                else
                {
                    var resourceUri = new Uri("images/plasma.wmv", UriKind.Relative);
                    //// System.Windows.Resources.StreamResourceInfo streamInfo = Application.GetResourceStream(resourceUri);
                    //// BitmapFrame temp = BitmapFrame.Create(streamInfo.Stream);
                    this.MediaUi.SetCurrentValue(MediaElement.SourceProperty, resourceUri);
                }
            }

            this.ImageTabControl.SelectedIndex = Properties.Settings.Default.TabIndex_SelectedImage;

            if (!string.IsNullOrEmpty(Properties.Settings.Default.FilePath_LastFx))
            {
                if (File.Exists(Properties.Settings.Default.FilePath_LastFx))
                {
                    this.CodeTabView.OpenFile(Properties.Settings.Default.FilePath_LastFx);
                    this.ApplyEffect(this.CodeTabView.CurrentShaderEffect);
                }
                else
                {
                    Properties.Settings.Default.FilePath_LastFx = string.Empty;
                    Properties.Settings.Default.Save();
                }
            }

            this.Loaded += this.MainWindowLoaded;
            this.Closing += this.MainWindowClosing;
        }

        private static bool IsValidFileName(string filename)
        {
            if (string.Equals(filename, Constants.FileNames.TempShaderFx, StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show($"'{Constants.FileNames.TempShaderFx}' not allowed for file name as it is reserved for Shazzam.");
                return false;
            }

            return true;
        }

        private void MainWindowLoaded(object sender, RoutedEventArgs e)
        {
            this.Plugin1.SelectedIndex = 0;
            this.SetupMenuBindings();
            this.SetCurrentValue(TitleProperty, "Shazzam Shader Editor - v" + VersionHelper.GetShortVersionNumber());
        }

        private void SetupMenuBindings()
        {
            ICommand isCommand = ((MainWindowViewModel)this.MainGrid.DataContext).ImageStretchCommand;
            ICommand fullCodeCommand = ((MainWindowViewModel)this.MainGrid.DataContext).FullScreenCodeCommand;
            ICommand fullImageCommand = ((MainWindowViewModel)this.MainGrid.DataContext).FullScreenImageCommand;

            this.InputBindings.Add(new KeyBinding(fullImageCommand, Key.F9, ModifierKeys.None));
            this.InputBindings.Add(new KeyBinding(fullCodeCommand, Key.F11, ModifierKeys.None));
            this.InputBindings.Add(new KeyBinding(isCommand, Key.F5, ModifierKeys.Control) { CommandParameter = "none" });
            this.InputBindings.Add(new KeyBinding(isCommand, Key.F6, ModifierKeys.Control) { CommandParameter = "fill" });
            this.InputBindings.Add(new KeyBinding(isCommand, Key.F7, ModifierKeys.Control) { CommandParameter = "uniform" });
            this.InputBindings.Add(new KeyBinding(isCommand, Key.F8, ModifierKeys.Control) { CommandParameter = "uniformtofill" });
        }

        private void MainWindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ShazzamSwitchboard.CodeTabView.SaveFileFirst();
        }

        private void CodeTabControlSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Properties.Settings.Default.TabIndex_SelectedImage = this.ImageTabControl.SelectedIndex;
            Properties.Settings.Default.Save();
        }

        private void CodeTabViewShaderEffectChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            this.ApplyEffect(this.CodeTabView.CurrentShaderEffect);
        }

        private void LoadImage(string fileName)
        {
            this.UserImage.SetCurrentValue(Image.SourceProperty, null);
            this.UserImage.SetCurrentValue(Image.SourceProperty, new BitmapImage(new Uri(fileName)));
        }

        private void LoadMedia(string fileName)
        {
            this.MediaUi.SetCurrentValue(MediaElement.SourceProperty, null);
            this.MediaUi.SetCurrentValue(MediaElement.SourceProperty, new Uri(fileName));
        }

        private void ApplyEffect(ShaderEffect se)
        {
            this.UserImage.SetCurrentValue(EffectProperty, se);
            this.SampleImage1.SetCurrentValue(EffectProperty, se);
            this.SampleImage2.SetCurrentValue(EffectProperty, se);
            this.SampleImage3.SetCurrentValue(EffectProperty, se);
            this.SampleImage4.SetCurrentValue(EffectProperty, se);
            this.SampleImage5.SetCurrentValue(EffectProperty, se);
            this.SampleUi.SetCurrentValue(EffectProperty, se);
            this.MediaUi.SetCurrentValue(EffectProperty, se);
        }

        private void NewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            // codeTabView.NewShader();
            var dialog = new SaveFileDialog { Title = "New File Name" };
            if (Properties.Settings.Default.FolderPath_FX != string.Empty)
            {
                dialog.InitialDirectory = Properties.Settings.Default.FolderPath_FX;
            }

            dialog.CheckPathExists = true;
            dialog.CreatePrompt = true;
            dialog.Filter = "Shader File (*.fx) |*.fx";
            if (dialog.ShowDialog() == true)
            {
                if (!IsValidFileName(dialog.SafeFileName))
                {
                    return;
                }

                using (var writer = new StreamWriter(new FileStream(dialog.FileName, FileMode.Create, FileAccess.ReadWrite)))
                {
                    writer.Write(Properties.Resources.NewShaderText);
                }

                this.LoadShaderEditor(dialog);
            }
        }

        private void LoadShaderEditor(FileDialog ofd)
        {
            this.CodeTabView.OpenFile(ofd.FileName);
            Properties.Settings.Default.FolderPath_FX = Path.GetDirectoryName(ofd.FileName);
            Properties.Settings.Default.FilePath_LastFx = ofd.FileName;
            Properties.Settings.Default.Save();

            if (ShazzamSwitchboard.FileLoaderPlugin != null)
            {
                ShazzamSwitchboard.FileLoaderPlugin.Update();
            }
        }

        private void OpenExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var dialog = new OpenFileDialog { Filter = "Shader Files (*.fx)|*.fx|All Files|*.*" };
            if (Properties.Settings.Default.FolderPath_FX != string.Empty)
            {
                dialog.InitialDirectory = Properties.Settings.Default.FolderPath_FX;
            }

            if (dialog.ShowDialog() == true)
            {
                if (!IsValidFileName(dialog.SafeFileName))
                {
                    return;
                }

                this.LoadShaderEditor(dialog);
            }
        }

        private void SaveExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                this.CodeTabView.SaveFile();
            }
            catch (UnauthorizedAccessException exception)
            {
                MessageBox.Show(this, exception.Message, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                this.SaveAsExecuted(sender, e);
            }
        }

        private void SaveAsExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var sfd = new SaveFileDialog
            {
                Filter = "FX files|*.fx;|All Files|*.*",
                InitialDirectory = Properties.Settings.Default.FolderPath_FX
            };

            if (sfd.ShowDialog() == true)
            {
                this.CodeTabView.SaveFile(sfd.FileName);
                Properties.Settings.Default.FolderPath_FX = Path.GetDirectoryName(sfd.FileName);
                Properties.Settings.Default.FilePath_LastFx = sfd.FileName;
                Properties.Settings.Default.Save();

                if (ShazzamSwitchboard.FileLoaderPlugin != null)
                {
                    ShazzamSwitchboard.FileLoaderPlugin.Update();
                }
            }
        }

        private void ExitExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void ApplyShaderExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            this.CodeTabView.RenderShader();
        }

        private void CompileShaderExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            this.CodeTabView.CompileShader();
        }

        private void RemoveShaderExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            this.UserImage.SetCurrentValue(EffectProperty, null);
            this.SampleImage1.SetCurrentValue(EffectProperty, null);
            this.SampleImage2.SetCurrentValue(EffectProperty, null);
            this.SampleImage3.SetCurrentValue(EffectProperty, null);
            this.SampleImage4.SetCurrentValue(EffectProperty, null);
            this.SampleImage5.SetCurrentValue(EffectProperty, null);
            this.SampleUi.SetCurrentValue(EffectProperty, null);
            this.MediaUi.SetCurrentValue(EffectProperty, null);
        }

        private void OpenImageExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var ofd = new OpenFileDialog { Filter = "Images|*.jpg;*.png;*.bmp;*.gif|All Files|*.*" };

            if (Properties.Settings.Default.FolderPath_Images != string.Empty)
            {
                ofd.InitialDirectory = Properties.Settings.Default.FolderPath_Images;
            }

            if (ofd.ShowDialog(this) == true)
            {
                this.LoadImage(ofd.FileName);
                Properties.Settings.Default.FilePath_LastImage = ofd.FileName;
                Properties.Settings.Default.FolderPath_Images = Path.GetDirectoryName(ofd.FileName);
                Properties.Settings.Default.Save();
            }
        }

        private void OpenMediaExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var ofd = new OpenFileDialog { Filter = "Video|*.wmv;*.wma|All Files|*.*" };

            if (Properties.Settings.Default.FolderPath_Images != string.Empty)
            {
                ofd.InitialDirectory = Properties.Settings.Default.FolderPath_Images;
            }

            if (ofd.ShowDialog(this) == true)
            {
                this.LoadMedia(ofd.FileName);
                Properties.Settings.Default.FilePath_LastMedia = ofd.FileName;
                Properties.Settings.Default.FolderPath_Images = Path.GetDirectoryName(ofd.FileName);
                Properties.Settings.Default.Save();
            }
        }

        private void ShaderCommandCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
            //// string fxcPath = Environment.ExpandEnvironmentVariables(Properties.Settings.Default.DirectX_FxcPath);
            //// e.CanExecute = File.Exists(fxcPath);
        }

        private void MediaUiMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.MediaUi.Position = TimeSpan.Zero;
        }

        private void MediaUiMediaEnded(object sender, RoutedEventArgs e)
        {
            if (this.AutoPlayCheckBox.IsChecked == true)
            {
                this.MediaUi.Position = TimeSpan.Zero;
            }
        }

        private void MediaUiMediaFailed(object sender, ExceptionRoutedEventArgs e)
        {
            this.VideoMessage.SetCurrentValue(TextBlock.TextProperty, "Cannot play the specified media.");
        }

        private void AutoPlayCheckBoxChecked(object sender, RoutedEventArgs e)
        {
            if (this.MediaUi != null)
            {
                this.MediaUi.Position = TimeSpan.Zero;
            }
        }

        private void ImageTabControlSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ReferenceEquals(this.ImageTabControl.SelectedItem, this.MediaTab))
            {
                this.MediaUi.Play();
            }
            else
            {
                this.MediaUi.Stop();
            }
        }

        private void Button1Click(object sender, RoutedEventArgs e)
        {
            this.FruitListBox.SetCurrentValue(System.Windows.Controls.Primitives.Selector.SelectedIndexProperty, 1);
        }

        private void Button2Click(object sender, RoutedEventArgs e)
        {
            this.FruitListBox.SetCurrentValue(System.Windows.Controls.Primitives.Selector.SelectedIndexProperty, 2);
        }
    }
}
