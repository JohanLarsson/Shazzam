using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Resources;

namespace Shazzam.Controls
{

  public partial class TexturePicker : UserControl
  {
    static Dictionary<int, ImageBrush> _images = new Dictionary<int, ImageBrush>();
    ShaderModelConstantRegister _register = null;
    public TexturePicker(ShaderModelConstantRegister register)
    {
      InitializeComponent();
      _register = register;
      ImageBrush result;
      // attempt to get the already loaded value
      _images.TryGetValue(_register.RegisterNumber, out result);
      Value = result;
      if (Value == null)
      {

        LoadTextureFromSettings();

      }

      SetupTextures();
    }

    void TexturePicker_Loaded(object sender, RoutedEventArgs e)
    {


    }

    private void LoadTextureFromSettings()
    {
      Uri tempUri;

      switch (_register.RegisterNumber)
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
          Value = new ImageBrush(new BitmapImage(tempUri));
        }
        else
        {
          StreamResourceInfo streamInfo = Application.GetContentStream(tempUri);

          BitmapFrame temp = BitmapFrame.Create(streamInfo.Stream);
          ImageBrush brush = new ImageBrush(temp);
          Value = brush;
          if (brush == null)
          {
            Value = new ImageBrush(image1.Source);
          }

        }
      }
    }
    public const string AssemblyPrefix = "images/texturemaps/";

    private void SetupTextures()
    {
      string path = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
      path += "\\Images\\TextureMaps";
      foreach (var file in System.IO.Directory.GetFiles(path))
      {
        IncludedTexturesCombo.Items.Add(new TextureMapLocator
        {
          ShortFileName = System.IO.Path.GetFileNameWithoutExtension(file),
          LongFileName = file,
          TrimmedFileName = AssemblyPrefix + System.IO.Path.GetFileName(file)
        });
      }
    }

    #region Value

    /// <summary>
    /// Value Dependency Property
    /// </summary>
    public static readonly DependencyProperty ValueProperty =
        DependencyProperty.Register("Value", typeof(ImageBrush), typeof(TexturePicker),
            new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnValueChanged)));

    /// <summary>
    /// Gets or sets the Value property.  This dependency property
    /// indicates the current value of the AdjustableSliderPair.
    /// </summary>
    public ImageBrush Value
    {
      get { return (ImageBrush)GetValue(ValueProperty); }
      set { SetValue(ValueProperty, value); }
    }

    /// <summary>
    /// Handles changes to the Value property.
    /// </summary>
    private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      ((TexturePicker)d).OnValueChanged(e);
    }

    /// <summary>
    /// Provides derived classes an opportunity to handle changes to the Value property.
    /// </summary>
    protected virtual void OnValueChanged(DependencyPropertyChangedEventArgs e)
    {
      ImageBrush brush = (ImageBrush)e.NewValue;
      _images[_register.RegisterNumber] = brush;
      image1.Source = image2.Source = brush.ImageSource;
    }

    #endregion

    private void btnOpenImage_Click(object sender, RoutedEventArgs e)
    {
      var dialog = new Microsoft.Win32.OpenFileDialog();
      dialog.DefaultExt = ".jpg"; // Default file extension
      dialog.Filter = "JPEG Images (.jpg); PNG files(.png)|*.jpg;*.png|All Files(*.*)|*.*"; // Filter files by extension
      dialog.CheckFileExists = true;
      dialog.CheckPathExists = true;
      if (dialog.ShowDialog() == true)
      {
        string filename = dialog.FileName;
        Uri uriLocation = new Uri(filename, UriKind.RelativeOrAbsolute);
        ImageBrush brush = new ImageBrush(new BitmapImage(uriLocation));
        Value = brush;

        switch (_register.RegisterNumber)
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

    private void IncludedTextures_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {

      BitmapImage bi = new BitmapImage(new Uri("/Shazzam;component/Images/cactus.jpg", UriKind.RelativeOrAbsolute)); ;
      string uriLocation = ((TextureMapLocator)IncludedTexturesCombo.SelectedItem).TrimmedFileName;

      Uri resourceUri = new Uri(uriLocation, UriKind.RelativeOrAbsolute);
      StreamResourceInfo streamInfo = Application.GetContentStream(resourceUri);

      BitmapFrame temp = BitmapFrame.Create(streamInfo.Stream);
      ImageBrush brush = new ImageBrush(temp);
      Value = brush;

      switch (_register.RegisterNumber)
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
  internal struct TextureMapLocator
  {
    public string ShortFileName { get; set; }
    public string LongFileName { get; set; }
    public string TrimmedFileName { get; set; }
  }
}
