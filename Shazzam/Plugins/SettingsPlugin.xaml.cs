namespace Shazzam.Plugins
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    public partial class SettingsPlugin : UserControl
    {
        public SettingsPlugin()
        {
            this.InitializeComponent();
            this.Loaded += this.SettingsPlugin_Loaded;
        }

        void SettingsPlugin_Loaded(object sender, RoutedEventArgs e)
        {
            if (RenderCapability.IsPixelShaderVersionSupported(3, 0))
            {
                this.notAvailable.Visibility = Visibility.Collapsed;
            }
            else
            {
                this.notAvailable.Visibility = Visibility.Visible;
                this.notAvailable.ToolTip = "PS_3 is not supported on your video card.";
            }
        }
    }
}
