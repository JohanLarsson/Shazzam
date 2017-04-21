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

        private void SettingsPlugin_Loaded(object sender, RoutedEventArgs e)
        {
            if (RenderCapability.IsPixelShaderVersionSupported(3, 0))
            {
                this.notAvailable.SetCurrentValue(VisibilityProperty, Visibility.Collapsed);
            }
            else
            {
                this.notAvailable.SetCurrentValue(VisibilityProperty, Visibility.Visible);
                this.notAvailable.SetCurrentValue(ToolTipProperty, "PS_3 is not supported on your video card.");
            }
        }
    }
}
