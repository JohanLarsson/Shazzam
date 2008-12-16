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

namespace Shazzam.Views {

	public partial class PluginView : System.Windows.Controls.UserControl {

		public const string PluginSubDir = "\\plugins";

		public PluginView() {
			InitializeComponent();
			LoadPlugins();
		}

		private void LoadPlugins() {
			if (ShazzamSwitchboard.IsFXCompilerAvailable()==false)
			{
				AddSettingsPlugin();
				return;
			}

			AddSettingsPlugin();
			//// add the settings plugin (we always want this to be at the end)
			Plugin colorLoader = new Plugin();
			colorLoader.Root = new Kaxaml.Plugins.ColorPicker.ColorPickerPlugin();
			colorLoader.Name = "Color Picker";
			colorLoader.Description = "Color assistant.";
			Plugins.Add(colorLoader);

			Plugin fileLoader = new Plugin();
			fileLoader.Root = new FileLoaderPlugin();
			fileLoader.Name = "Shader Loader";
			fileLoader.Description = "Pick a Shader fx file to open.";
		
			Plugins.Add(fileLoader);

			//// add the about plugin
			Plugin about = new Plugin();
			about.Root = new About();
			about.Name = "About";
			about.Description = "About Shazzam";
			Plugins.Add(about);

		}

		private void AddSettingsPlugin() {
			Plugin settings = new Plugin();
			settings.Root = new SettingsPlugin();
			settings.Name = "Settings";
			settings.Description = "Modify program settings and options (Ctrl+E)";
			//settings.Key = Key.E;
			//settings.ModifierKeys = ModifierKeys.Control;
			Plugins.Add(settings);
		}
		private ImageSource LoadIcon(string imagePath) {
			//ImageSourceConverter conv = new ImageSourceConverter();
			//return conv.ConvertFromString(imagePath);
			Uri resourceUri = new Uri(imagePath, UriKind.Relative);
			System.Windows.Resources.StreamResourceInfo streamInfo = Application.GetResourceStream(resourceUri);

			BitmapFrame temp = BitmapFrame.Create(streamInfo.Stream);
			return temp;
			//	return bitmapSource;
		}
		private ImageSource LoadIcon(Type typ, string icon) {
			Assembly asm = Assembly.GetAssembly(typ);
			//	string iconString = typ.Namespace + '.' + icon.Replace('\\', '.');
			Stream myStream = asm.GetManifestResourceStream(icon);

			if (myStream == null)
			{
				//	iconString = typ.Name + '.' + icon.Replace('\\', '.');
				myStream = asm.GetManifestResourceStream(icon);
			}

			if (myStream == null)
			{
				//	iconString = "Kaxaml.Images.package.png";
				myStream = asm.GetManifestResourceStream(icon);
			}

			if (myStream != null)
			{
				PngBitmapDecoder bitmapDecoder = new PngBitmapDecoder(myStream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
				if (bitmapDecoder.Frames[0] != null && bitmapDecoder.Frames[0] is ImageSource)
				{
					return bitmapDecoder.Frames[0];
				}
				else
				{
					return null;
				}
			}
			return null;
		}

		public List<Plugin> Plugins {
			get { return (List<Plugin>)GetValue(PluginsProperty); }
			set { SetValue(PluginsProperty, value); }
		}
		public static readonly DependencyProperty PluginsProperty =
				DependencyProperty.Register("Plugins", typeof(List<Plugin>), typeof(PluginView), new UIPropertyMetadata(new List<Plugin>()));

		public void OpenPlugin(Key key, ModifierKeys modifierkeys) {
			foreach (Plugin p in Plugins)
			{
				if (modifierkeys == p.ModifierKeys && key == p.Key)
				{
					try
					{
						TabItem t = (TabItem)((FrameworkElement)p.Root).Parent;
						t.IsSelected = true;
						t.Focus();

						UpdateLayout();

						if (t.Content is FrameworkElement)
						{
							(t.Content as FrameworkElement).MoveFocus(new TraversalRequest(FocusNavigationDirection.First));
						}
					}
					catch { }
				}
			}
		}

		Plugin _findPlugin = null;
		internal Plugin GetFindPlugin() {
			return _findPlugin;
		}

		public Plugin SelectedPlugin {
			get {
				return (Plugin)PluginTabControl.SelectedItem;
			}
			set {
				PluginTabControl.SelectedItem = (Plugin)value;
			}
		}

	}
}