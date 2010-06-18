using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Shazzam.Plugins
{

  public partial class SettingsPlugin : UserControl
  {
    public SettingsPlugin()
    {
      InitializeComponent();
      this.Loaded += new RoutedEventHandler(SettingsPlugin_Loaded);
    }

    void SettingsPlugin_Loaded(object sender, RoutedEventArgs e)
    {
      if (RenderCapability.IsPixelShaderVersionSupported(3, 0))
      {
        notAvailable.Visibility = Visibility.Collapsed;

      }
      else
      {
        notAvailable.Visibility = Visibility.Visible;
        notAvailable.ToolTip = "PS_3 is not supported on your video card.";
      }
    }
  }
}
