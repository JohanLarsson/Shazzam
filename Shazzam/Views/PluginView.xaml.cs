namespace Shazzam.Views
{
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;

    using Shazzam.Plugins;

    public partial class PluginView : UserControl
    {
        public const string PluginSubDir = "\\plugins";

        public static readonly DependencyProperty PluginsProperty = DependencyProperty.Register(
            nameof(Plugins),
            typeof(List<Plugin>),
            typeof(PluginView),
            new UIPropertyMetadata(null));

        public PluginView()
        {
            this.InitializeComponent();
            this.Plugins = new List<Plugin>();
            this.LoadPlugins();
        }

        public List<Plugin> Plugins
        {
            get => (List<Plugin>)this.GetValue(PluginsProperty);
            set => this.SetValue(PluginsProperty, value);
        }

        public Plugin SelectedPlugin
        {
            get => (Plugin)this.PluginTabControl.SelectedItem;
            set => this.PluginTabControl.SetCurrentValue(System.Windows.Controls.Primitives.Selector.SelectedItemProperty, value);
        }

        public int SelectedIndex
        {
            get => this.PluginTabControl.SelectedIndex;
            set => this.PluginTabControl.SetCurrentValue(System.Windows.Controls.Primitives.Selector.SelectedIndexProperty, value);
        }

        private void LoadPlugins()
        {
            var fileLoader = new Plugin
            {
                Root = new Plugins.FileLoaderPlugin(),
                Name = "Shader Loader",
                Description = "Pick a shader file to open"
            };

            this.Plugins.Add(fileLoader);

            // Plugin colorLoader = new Plugin();
            // colorLoader.Root = new Kaxaml.Plugins.ColorPicker.ColorPickerPlugin();
            // colorLoader.Name = "Color Picker";
            // colorLoader.Description = "Color assistant";
            // Plugins.Add(colorLoader);
            this.AddSettingsPlugin();

            //// add the about plugin
            var about = new Plugin
            {
                Root = new About(),
                Name = "About Shazzam",
                Description = "About Shazzam"
            };
            this.Plugins.Add(about);
            //// SelectedPlugin = colorLoader;
        }

        private void AddSettingsPlugin()
        {
            var settings = new Plugin
            {
                Root = new SettingsPlugin(),
                Name = "Settings",
                Description = "Modify program settings and options"
            };
            //// settings.Key = Key.E;
            //// settings.ModifierKeys = ModifierKeys.Control;
            this.Plugins.Add(settings);
        }
    }
}
