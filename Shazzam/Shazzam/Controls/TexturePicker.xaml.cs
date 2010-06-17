using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Linq;

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
      //this.Loaded += new RoutedEventHandler(TexturePicker_Loaded);
      ImageBrush result;
      _images.TryGetValue(_register.RegisterNumber, out result);
      Value = result;
      if (Value == null)
      {
        Value = new ImageBrush(image1.Source);
      }
      SetupTextures();
    }
    public const string AssemblyPrefix = "images/texturemaps/";
    private void SetupTextures()
    {
      string path = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
      path += "\\Images\\TextureMaps";
      foreach (var file in System.IO.Directory.GetFiles(path))
      {
        // IncludedTexturesCombo.Items.Add(System.IO.Path.GetFileName(file));
        IncludedTexturesCombo.Items.Add(new TextureMapLocator { ShortFileName = System.IO.Path.GetFileNameWithoutExtension(file), LongFileName = file, TrimmedFileName = AssemblyPrefix + System.IO.Path.GetFileName(file) });
      }
    }
    private void SetupTextures2()
    {

      var rm = new ResourceManager(Assembly.GetExecutingAssembly().GetName().Name + ".g", Assembly.GetExecutingAssembly());

      ResourceSet rs = rm.GetResourceSet(CultureInfo.CurrentCulture, true, true);
      var maps = new List<string>();
      foreach (System.Collections.DictionaryEntry r in rs)
      {
        var name = r.Key.ToString();
        if (name.StartsWith(AssemblyPrefix))
        {
          maps.Add(name.Remove(0, AssemblyPrefix.Length));
        }
      }

      maps.Sort();
      foreach (var entry in maps)
      {
        //	var name = entry.Key.ToString();
        //if (name.StartsWith(AssemblyPrefix))
        //{
        //  cboIncludedTextures.Items.Add();
        //}
        IncludedTexturesCombo.Items.Add(entry);

      }

    }
    void TexturePicker_Loaded(object sender, RoutedEventArgs e)
    {
      //ImageBrush result;
      //_images.TryGetValue(_register.RegisterNumber, out result);
      //Value = result;
      //if (Value == null)
      //{
      //  Value = new ImageBrush(image1.Source);
      //}
    }

    //public TexturePicker()
    //{
    //  InitializeComponent();

    //  ImageBrush brush = new ImageBrush(image1.Source);

    //  Value = brush;
    //}

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
      dialog.Filter = "JPEG Images (.jpg); PNG files(.png)|*.jpg;*.png"; // Filter files by extension
      dialog.CheckFileExists = true;
      dialog.CheckPathExists = true;
      if (dialog.ShowDialog() == true)
      {
        string filename = dialog.FileName;
        ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(filename)));
        Value = brush;
      }

    }

    private void cboIncludedTextures_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      //switch (cboIncludedTextures.SelectedItem.ToString())
      //{
      //case "Clouds":
      //  bi = new BitmapImage (new Uri("/Shazzam;component/Images/Textures/clouds.png",UriKind.RelativeOrAbsolute));
      //  image1.Source = bi;
      //  break;
      //case "Cactus":
      //  bi = new BitmapImage(new Uri("/Shazzam;component/Images/cactus.jpg", UriKind.RelativeOrAbsolute));
      //  image1.Source = bi;
      //  break;
      //case "Sweep":
      //  bi = new BitmapImage(new Uri("/Shazzam;component/Images/Textures/colorrange.png", UriKind.RelativeOrAbsolute));
      //  image1.Source = bi;
      //  break;
      //case "Blocks":
      //  bi = new BitmapImage(new Uri("/Shazzam;component/Images/Textures/blocks.jpg", UriKind.RelativeOrAbsolute));
      //  image1.Source = bi;
      //  break;
      //case "GradientBlocks":
      //  bi = new BitmapImage(new Uri("/Shazzam;component/Images/Textures/gradientblocks.png", UriKind.RelativeOrAbsolute));
      //  image1.Source = bi;
      //  break;
      //case "Snowflake":
      //  bi = new BitmapImage(new Uri("/Shazzam;component/Images/Textures/koch_snowflake.jpg", UriKind.RelativeOrAbsolute));
      //  image1.Source = bi;
      //  break;
      //  case "RippledGlass":
      //  bi = new BitmapImage(new Uri("/Shazzam;component/Images/Textures/rippled_glass.png", UriKind.RelativeOrAbsolute));
      //  image1.Source = bi;
      //  break;
      //default:
      //  break;
      //}
      BitmapImage bi = new BitmapImage(new Uri("/Shazzam;component/Images/cactus.jpg", UriKind.RelativeOrAbsolute)); ;

      // string uriLocation = string.Format("images/texturemaps/{0}", IncludedTexturesCombo.SelectedItem.ToString());
      string uriLocation = ((TextureMapLocator)IncludedTexturesCombo.SelectedItem).TrimmedFileName;
      //bi = new BitmapImage(new Uri(uriLocation,UriKind.RelativeOrAbsolute));
      //ImageBrush brush = new ImageBrush(bi);
      //Value = brush;
      //Shazzam.Images.Textures.clouds.png

      Uri resourceUri = new Uri(uriLocation, UriKind.RelativeOrAbsolute);

      StreamResourceInfo streamInfo = Application.GetContentStream(resourceUri);

      BitmapFrame temp = BitmapFrame.Create(streamInfo.Stream);
      ImageBrush brush = new ImageBrush(temp);
      Value = brush;

    }
  }
  internal struct TextureMapLocator
  {
    public string ShortFileName { get; set; }
    public string LongFileName { get; set; }
    public string TrimmedFileName { get; set; }
  }
}
