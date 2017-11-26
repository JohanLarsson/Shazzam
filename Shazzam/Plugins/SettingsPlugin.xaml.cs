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
            this.Loaded += this.SettingsPluginLoaded;
        }

        private void SettingsPluginLoaded(object sender, RoutedEventArgs e)
        {
            if (RenderCapability.IsPixelShaderVersionSupported(3, 0))
            {
                this.NotAvailable.SetCurrentValue(VisibilityProperty, Visibility.Collapsed);
            }
            else
            {
                this.NotAvailable.SetCurrentValue(VisibilityProperty, Visibility.Visible);
                this.NotAvailable.SetCurrentValue(ToolTipProperty, "PS_3 is not supported on your video card.");
            }
        }
    }
}
