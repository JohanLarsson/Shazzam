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
    using Shazzam.ViewModels;

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
            ShazzamSwitchboard.CodeTabView = this.codeTabView;
            this.codeTabView.ShaderEffectChanged += this.codeTabView_ShaderEffectChanged;
            this.imageTabControl.SelectionChanged += this.codeTabControl_SelectionChanged;

            if (Properties.Settings.Default.FilePath_LastImage != string.Empty)
            {
                if (File.Exists(Properties.Settings.Default.FilePath_LastImage))
                {
                    this.LoadImage(Properties.Settings.Default.FilePath_LastImage);
                }
                else
                {
                    Uri resourceUri = new Uri("images/ColorRange.png", UriKind.Relative);
                    System.Windows.Resources.StreamResourceInfo streamInfo = Application.GetResourceStream(resourceUri);

                    BitmapFrame temp = BitmapFrame.Create(streamInfo.Stream);
                    this.userImage.Source = temp;
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
                    Uri resourceUri = new Uri("images/plasma.wmv", UriKind.Relative);
                    // System.Windows.Resources.StreamResourceInfo streamInfo = Application.GetResourceStream(resourceUri);

                    // BitmapFrame temp = BitmapFrame.Create(streamInfo.Stream);
                    this.mediaUI.Source = resourceUri;
                }
            }

            this.imageTabControl.SelectedIndex = Properties.Settings.Default.TabIndex_SelectedImage;

            if (!string.IsNullOrEmpty(Properties.Settings.Default.FilePath_LastFx))
            {
                if (File.Exists(Properties.Settings.Default.FilePath_LastFx))
                {
                    this.codeTabView.OpenFile(Properties.Settings.Default.FilePath_LastFx);
                    this.ApplyEffect(this.codeTabView.CurrentShaderEffect);
                }
                else
                {
                    Properties.Settings.Default.FilePath_LastFx = string.Empty;
                    Properties.Settings.Default.Save();
                }
            }

            this.Loaded += this.MainWindow_Loaded;
            this.Closing += this.MainWindow_Closing;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.plugin1.SelectedIndex = 0;
            this.SetupMenuBindings();
            this.Title = "Shazzam Shader Editor - v" + VersionHelper.GetShortVersionNumber();
        }

        private void SetupMenuBindings()
        {
            ICommand isCommand = ((MainWindowViewModel)this.mainGrid.DataContext).ImageStretchCommand;
            ICommand fullCodeCommand = ((MainWindowViewModel)this.mainGrid.DataContext).FullScreenCodeCommand;
            ICommand fullImageCommand = ((MainWindowViewModel)this.mainGrid.DataContext).FullScreenImageCommand;

            KeyBinding kb;

            kb = new KeyBinding(fullImageCommand, Key.F9, ModifierKeys.None);
            this.InputBindings.Add(kb);

            kb = new KeyBinding(fullCodeCommand, Key.F11, ModifierKeys.None);
            this.InputBindings.Add(kb);

            kb = new KeyBinding(isCommand, Key.F5, ModifierKeys.Control);
            kb.CommandParameter = "none";
            this.InputBindings.Add(kb);

            kb = new KeyBinding(isCommand, Key.F6, ModifierKeys.Control);
            kb.CommandParameter = "fill";
            this.InputBindings.Add(kb);

            kb = new KeyBinding(isCommand, Key.F7, ModifierKeys.Control);
            kb.CommandParameter = "uniform";
            this.InputBindings.Add(kb);

            kb = new KeyBinding(isCommand, Key.F8, ModifierKeys.Control);
            kb.CommandParameter = "uniformtofill";
            this.InputBindings.Add(kb);
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ShazzamSwitchboard.CodeTabView.SaveFileFirst();
        }

        private void codeTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Properties.Settings.Default.TabIndex_SelectedImage = this.imageTabControl.SelectedIndex;
            Properties.Settings.Default.Save();
        }

        private void codeTabView_ShaderEffectChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            this.ApplyEffect(this.codeTabView.CurrentShaderEffect);
        }

        private void LoadImage(string fileName)
        {
            this.userImage.Source = null;
            this.userImage.Source = new BitmapImage(new Uri(fileName));
        }

        private void LoadMedia(string fileName)
        {
            this.mediaUI.Source = null;
            this.mediaUI.Source = new Uri(fileName);
        }

        private void ApplyEffect(ShaderEffect se)
        {
            this.userImage.Effect = se;
            this.sampleImage1.Effect = se;
            this.sampleImage2.Effect = se;
            this.sampleImage3.Effect = se;
            this.sampleImage4.Effect = se;
            this.sampleImage5.Effect = se;
            this.sampleUI.Effect = se;
            this.mediaUI.Effect = se;
        }

        private void New_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            // codeTabView.NewShader();
            var dialog = new SaveFileDialog();
            dialog.Title = "New File Name";
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

                using (StreamWriter writer = new StreamWriter(new FileStream(dialog.FileName, FileMode.Create, FileAccess.ReadWrite)))
                {
                    writer.Write(Properties.Resources.NewShaderText);
                }

                this.LoadShaderEditor(dialog);
            }
        }

        private static bool IsValidFileName(string filename)
        {
            if (string.Equals(filename, Constants.FileNames.TempShaderFx, StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show(string.Format("'{0}' not allowed for file name as it is reserved for Shazzam.", Constants.FileNames.TempShaderFx));
                return false;
            }

            return true;
        }

        private void LoadShaderEditor(FileDialog ofd)
        {
            this.codeTabView.OpenFile(ofd.FileName);
            Properties.Settings.Default.FolderPath_FX = Path.GetDirectoryName(ofd.FileName);
            Properties.Settings.Default.FilePath_LastFx = ofd.FileName;
            Properties.Settings.Default.Save();

            if (ShazzamSwitchboard.FileLoaderPlugin != null)
            {
                ShazzamSwitchboard.FileLoaderPlugin.Update();
            }
        }

        private void Open_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "Shader Files (*.fx)|*.fx|All Files|*.*";
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

        private void Save_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                this.codeTabView.SaveFile();
            }
            catch (UnauthorizedAccessException exception)
            {
                MessageBox.Show(this, exception.Message, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                this.SaveAs_Executed(sender, e);
            }
        }

        private void SaveAs_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var sfd = new SaveFileDialog();
            sfd.Filter = "FX files|*.fx;|All Files|*.*";
            sfd.InitialDirectory = Properties.Settings.Default.FolderPath_FX;

            if (sfd.ShowDialog() == true)
            {
                this.codeTabView.SaveFile(sfd.FileName);
                Properties.Settings.Default.FolderPath_FX = Path.GetDirectoryName(sfd.FileName);
                Properties.Settings.Default.FilePath_LastFx = sfd.FileName;
                Properties.Settings.Default.Save();

                if (ShazzamSwitchboard.FileLoaderPlugin != null)
                {
                    ShazzamSwitchboard.FileLoaderPlugin.Update();
                }
            }
        }

        private void Exit_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void ApplyShader_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.codeTabView.RenderShader();
        }

        private void CompileShader_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.codeTabView.CompileShader();
        }

        private void RemoveShader_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.userImage.Effect = null;
            this.sampleImage1.Effect = null;
            this.sampleImage2.Effect = null;
            this.sampleImage3.Effect = null;
            this.sampleImage4.Effect = null;
            this.sampleImage5.Effect = null;
            this.sampleUI.Effect = null;
            this.mediaUI.Effect = null;
        }

        // private void ExploreCompiledShaders_Executed(object sender, System.Windows.RoutedEventArgs e)
        // {
        //  string path = Properties.Settings.Default.FolderPath_Output;
        //  System.Diagnostics.Process.Start(path);
        // }
        // private void ExploreTextureMaps_Executed(object sender, System.Windows.RoutedEventArgs e)
        // {
        //  string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        //  System.Diagnostics.Process.Start(path + Constants.Paths.TextureMaps);
        // }

        // private void FullScreenImage_Executed(object sender, ExecutedRoutedEventArgs e)
        // {
        //  if (codeRow.Height != new GridLength(0, GridUnitType.Pixel))
        //  {

        // codeRow.Height = new GridLength(0, GridUnitType.Pixel);
        //    imageRow.Height = new GridLength(5, GridUnitType.Star);
        //  }
        //  else
        //  {

        // codeRow.Height = new GridLength(5, GridUnitType.Star);
        //    imageRow.Height = new GridLength(5, GridUnitType.Star);
        //  }
        // }

        // private void FullScreenCode_Executed(object sender, ExecutedRoutedEventArgs e)
        // {

        // if (imageRow.Height != new GridLength(0, GridUnitType.Pixel))
        //  {
        //    imageRow.Height = new GridLength(0, GridUnitType.Pixel);
        //    codeRow.Height = new GridLength(5, GridUnitType.Star);
        //  }
        //  else
        //  {
        //    imageRow.Height = new GridLength(5, GridUnitType.Star);

        // }

        // }
        private void OpenImage_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Images|*.jpg;*.png;*.bmp;*.gif|All Files|*.*";

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

        private void OpenMedia_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Video|*.wmv;*.wma|All Files|*.*";

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

        private void ShaderCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
            // string fxcPath = Environment.ExpandEnvironmentVariables(Properties.Settings.Default.DirectX_FxcPath);
            // e.CanExecute = File.Exists(fxcPath);
        }

        //// private void ImageStretch_Executed(object sender, ExecutedRoutedEventArgs e)
        //// {
        ////  //switch (e.Parameter.ToString())
        ////  //{
        ////  //  case "none":
        ////  //    SetStretchMode(System.Windows.Media.Stretch.None);
        ////  //    break;
        ////  //  case "fill":
        ////  //    SetStretchMode(System.Windows.Media.Stretch.Fill);
        ////  //    break;
        ////  //  case "uniform":
        ////  //    SetStretchMode(System.Windows.Media.Stretch.Uniform);
        ////  //    break;
        ////  //  case "uniformtofill":
        ////  //    SetStretchMode(System.Windows.Media.Stretch.UniformToFill);
        ////  //    break;
        ////  //  default:
        ////  //    SetStretchMode(System.Windows.Media.Stretch.Uniform);

        //// //    break;
        ////  //}
        //// }

        private void SetStretchMode(System.Windows.Media.Stretch stretchMode)
        {
            this.userImage.Stretch = stretchMode;
            this.sampleImage1.Stretch = stretchMode;
            this.sampleImage2.Stretch = stretchMode;
            this.sampleImage3.Stretch = stretchMode;
            this.sampleImage4.Stretch = stretchMode;
            this.sampleImage5.Stretch = stretchMode;
            this.mediaUI.Stretch = stretchMode;
        }

        private void ChangeTab_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            switch (e.Parameter.ToString())
            {
                case "codetab":

                    break;
                case "edittab":
                    this.SetStretchMode(System.Windows.Media.Stretch.Fill);
                    break;

                default:
                    this.SetStretchMode(System.Windows.Media.Stretch.Uniform);

                    break;
            }
        }

        private void mediaUI_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.mediaUI.Position = TimeSpan.Zero;
        }

        private void mediaUI_MediaEnded(object sender, RoutedEventArgs e)
        {
            if (this.autoPlayCheckBox.IsChecked == true)
            {
                this.mediaUI.Position = TimeSpan.Zero;
            }
        }

        private void mediaUI_MediaFailed(object sender, ExceptionRoutedEventArgs e)
        {
            this.videoMessage.Text = "Cannot play the specified media.";
        }

        private void autoPlayCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (this.mediaUI != null)
            {
                this.mediaUI.Position = TimeSpan.Zero;
            }
        }

        private void imageTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.imageTabControl.SelectedItem == this.mediaTab)
            {
                this.mediaUI.Play();
            }
            else
            {
                this.mediaUI.Stop();
            }
        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            this.fruitListBox.SelectedIndex = 1;
        }

        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            this.fruitListBox.SelectedIndex = 2;
        }
    }
}
