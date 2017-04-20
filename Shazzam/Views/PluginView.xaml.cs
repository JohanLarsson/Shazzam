namespace Shazzam.Views
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using Kaxaml;
    using Shazzam.Plugins;

    public partial class PluginView : UserControl
    {
        public const string PluginSubDir = "\\plugins";

        public PluginView()
        {
            this.InitializeComponent();
            this.LoadPlugins();
        }

        private void LoadPlugins()
        {
            var fileLoader = new Plugin();
            fileLoader.Root = new FileLoaderPlugin();
            fileLoader.Name = "Shader Loader";
            fileLoader.Description = "Pick a shader file to open";

            this.Plugins.Add(fileLoader);

      // Plugin colorLoader = new Plugin();
      // colorLoader.Root = new Kaxaml.Plugins.ColorPicker.ColorPickerPlugin();
      // colorLoader.Name = "Color Picker";
      // colorLoader.Description = "Color assistant";
      // Plugins.Add(colorLoader);
            this.AddSettingsPlugin();

            //// add the about plugin
            var about = new Plugin();
            about.Root = new About();
            about.Name = "About Shazzam";
            about.Description = "About Shazzam";
            this.Plugins.Add(about);
        // SelectedPlugin = colorLoader;
        }

        private void AddSettingsPlugin()
        {
            var settings = new Plugin();
            settings.Root = new SettingsPlugin();
            settings.Name = "Settings";
            settings.Description = "Modify program settings and options";
            // settings.Key = Key.E;
            // settings.ModifierKeys = ModifierKeys.Control;
            this.Plugins.Add(settings);
        }

        private ImageSource LoadIcon(string imagePath)
        {
            // ImageSourceConverter conv = new ImageSourceConverter();
            // return conv.ConvertFromString(imagePath);
            var resourceUri = new Uri(imagePath, UriKind.Relative);
            var streamInfo = Application.GetResourceStream(resourceUri);

            var temp = BitmapFrame.Create(streamInfo.Stream);
            return temp;
            // return bitmapSource;
        }

        private ImageSource LoadIcon(Type typ, string icon)
        {
            var asm = Assembly.GetAssembly(typ);
            //// string iconString = typ.Namespace + '.' + icon.Replace('\\', '.');
            using (var myStream = asm.GetManifestResourceStream(icon))
            {
                if (myStream != null)
                {
                    var bitmapDecoder = new PngBitmapDecoder(myStream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                    return bitmapDecoder.Frames[0];
                }
            }

            return null;
        }

        public List<Plugin> Plugins
        {
            get { return (List<Plugin>)this.GetValue(PluginsProperty); }
            set { this.SetValue(PluginsProperty, value); }
        }

        public static readonly DependencyProperty PluginsProperty =
                DependencyProperty.Register("Plugins", typeof(List<Plugin>), typeof(PluginView), new UIPropertyMetadata(new List<Plugin>()));

        public void OpenPlugin(Key key, ModifierKeys modifierkeys)
        {
            foreach (var p in this.Plugins)
            {
                if (modifierkeys == p.ModifierKeys && key == p.Key)
                {
                    try
                    {
                        var t = (TabItem)((FrameworkElement)p.Root).Parent;
                        t.IsSelected = true;
                        t.Focus();

                        this.UpdateLayout();

                        if (t.Content is FrameworkElement)
                        {
                            (t.Content as FrameworkElement).MoveFocus(new TraversalRequest(FocusNavigationDirection.First));
                        }
                    }
                    catch
                    {
                    }
                }
            }
        }

        Plugin findPlugin = null;

        internal Plugin GetFindPlugin()
        {
            return this.findPlugin;
        }

        public int SelectedIndex
        {
            get
            {
                return (int)this.PluginTabControl.SelectedIndex;
            }

            set
            {
                this.PluginTabControl.SelectedIndex = (int)value;
            }
        }

        public Plugin SelectedPlugin
        {
            get
            {
                return (Plugin)this.PluginTabControl.SelectedItem;
            }

            set
            {
                this.PluginTabControl.SelectedItem = (Plugin)value;
            }
        }
    }
}