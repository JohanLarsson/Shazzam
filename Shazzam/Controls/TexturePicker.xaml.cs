namespace Shazzam.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    public partial class TexturePicker : UserControl
    {
        public const string AssemblyPrefix = "images/texturemaps/";

        /// <summary>
        /// Value Dependency Property
        /// </summary>
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            nameof(Value),
            typeof(ImageBrush),
            typeof(TexturePicker),
            new FrameworkPropertyMetadata(null, OnValueChanged));

        private static readonly Dictionary<int, ImageBrush> Images = new();
        private readonly ShaderModelConstantRegister register;

        public TexturePicker(ShaderModelConstantRegister register)
        {
            this.InitializeComponent();
            this.register = register;
            //// attempt to get the already loaded value
            Images.TryGetValue(this.register.RegisterNumber, out ImageBrush result);
            this.Value = result;
            if (this.Value is null)
            {
                this.LoadTextureFromSettings();
            }

            this.SetupTextures();
        }

        /// <summary>
        /// Gets or sets the Value property.  This dependency property
        /// indicates the current value of the AdjustableSliderPair.
        /// </summary>
        public ImageBrush Value
        {
            get => (ImageBrush)this.GetValue(ValueProperty);
            set => this.SetValue(ValueProperty, value);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the Value property.
        /// </summary>
        protected virtual void OnValueChanged(DependencyPropertyChangedEventArgs e)
        {
            var brush = (ImageBrush)e.NewValue;
            Images[this.register.RegisterNumber] = brush;
            this.image2.SetCurrentValue(Image.SourceProperty, brush.ImageSource);
            this.image1.SetCurrentValue(Image.SourceProperty, brush.ImageSource);
        }

        /// <summary>
        /// Handles changes to the Value property.
        /// </summary>
        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((TexturePicker)d).OnValueChanged(e);
        }

        private void BtnOpenImageClick(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog
            {
                DefaultExt = ".jpg",
                Filter = "JPEG Images (.jpg); PNG files(.png)|*.jpg;*.png|All Files(*.*)|*.*",
                CheckFileExists = true,
                CheckPathExists = true,
            };

            // Default file extension
            // Filter files by extension
            if (dialog.ShowDialog() == true)
            {
                var filename = dialog.FileName;
                var uriLocation = new Uri(filename, UriKind.RelativeOrAbsolute);
                var brush = new ImageBrush(new BitmapImage(uriLocation));
                this.SetCurrentValue(ValueProperty, brush);

                switch (this.register.RegisterNumber)
                {
                    case 2:
                        {
                            Properties.Settings.Default.FilePath_TextureMap2 = uriLocation;
                            break;
                        }

                    case 3:
                        {
                            Properties.Settings.Default.FilePath_TextureMap3 = uriLocation;
                            break;
                        }

                    default:
                        Properties.Settings.Default.FilePath_TextureMap1 = uriLocation;
                        break;
                }

                Properties.Settings.Default.Save();
            }
        }

        private void IncludedTexturesSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ////var bi = new BitmapImage(new Uri("/Shazzam;component/Images/cactus.jpg", UriKind.RelativeOrAbsolute));
            //// string uriLocation = ((TextureMapLocator)IncludedTexturesCombo.SelectedItem).TrimmedFileName;
            var uriLocation = ((TextureMapLocator)this.IncludedTexturesList.SelectedItem).TrimmedFileName;
            this.popSelectTexture.SetCurrentValue(System.Windows.Controls.Primitives.Popup.IsOpenProperty, false);
            var resourceUri = new Uri(uriLocation, UriKind.RelativeOrAbsolute);
            var streamInfo = Application.GetContentStream(resourceUri);
            if (streamInfo != null)
            {
                var temp = BitmapFrame.Create(streamInfo.Stream);
                var brush = new ImageBrush(temp);
                this.SetCurrentValue(ValueProperty, brush);

                switch (this.register.RegisterNumber)
                {
                    case 2:
                        {
                            Properties.Settings.Default.FilePath_TextureMap2 = resourceUri;
                            break;
                        }

                    case 3:
                        {
                            Properties.Settings.Default.FilePath_TextureMap3 = resourceUri;
                            break;
                        }

                    default:
                        Properties.Settings.Default.FilePath_TextureMap1 = resourceUri;
                        break;
                }

                // Properties.Settings.Default.FilePath_TextureMap1 = resourceUri;
                Properties.Settings.Default.Save();
            }
        }

        private void ChooseTextureClick(object sender, RoutedEventArgs e)
        {
            this.popSelectTexture.SetCurrentValue(System.Windows.Controls.Primitives.Popup.IsOpenProperty, true);
        }

        private void LoadTextureFromSettings()
        {
            Uri tempUri;

            switch (this.register.RegisterNumber)
            {
                case 2:
                    {
                        tempUri = Properties.Settings.Default.FilePath_TextureMap2;
                        break;
                    }

                case 3:
                    {
                        tempUri = Properties.Settings.Default.FilePath_TextureMap3;
                        break;
                    }

                default:
                    tempUri = Properties.Settings.Default.FilePath_TextureMap1;
                    break;
            }

            if (tempUri != null)
            {
                if (tempUri.IsAbsoluteUri)
                {
                    if (System.IO.File.Exists(tempUri.AbsolutePath))
                    {
                        this.SetCurrentValue(ValueProperty, new ImageBrush(new BitmapImage(tempUri)));
                    }
                    else
                    {
                        this.SetCurrentValue(ValueProperty, null);
                    }
                }
                else
                {
                    var streamInfo = Application.GetContentStream(tempUri);
                    if (streamInfo != null)
                    {
                        var temp = BitmapFrame.Create(streamInfo.Stream);
                        var brush = new ImageBrush(temp);
                        this.SetCurrentValue(ValueProperty, brush);
                    }
                    else
                    {
                        this.SetCurrentValue(ValueProperty, null);
                    }
                }
            }
        }

        private void SetupTextures()
        {
            var path = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            path += "\\Images\\TextureMaps";
            foreach (var file in System.IO.Directory.GetFiles(path))
            {
                this.IncludedTexturesList.Items.Add(new TextureMapLocator
                {
                    ShortFileName = System.IO.Path.GetFileNameWithoutExtension(file),
                    LongFileName = file,
                    TrimmedFileName = AssemblyPrefix + System.IO.Path.GetFileName(file),
                });
            }
        }
    }
}
