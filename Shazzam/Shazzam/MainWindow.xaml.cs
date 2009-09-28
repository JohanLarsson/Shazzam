using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using System.IO;
using System.Windows.Media;

namespace Shazzam
{
	//  Images
	//  creative commons license
	//  StuffEyeSee  http://www.flickr.com/photos/rcsaxon/689732379/
	//  http://www.flickr.com/photos/glockenblume/2228713567/sizes/l/
	//  http://www.flickr.com/photos/96dpi/2329024258/
	//
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			Commands.AppCommands.Initialize();
			InitializeComponent();
			ShazzamSwitchboard.MainWindow = this;
			ShazzamSwitchboard.CodeTabView = this.codeTabView;
			codeTabView.ShaderEffectChanged += new RoutedPropertyChangedEventHandler<object>(codeTabView_ShaderEffectChanged);
			imageTabControl.SelectionChanged += new SelectionChangedEventHandler(codeTabControl_SelectionChanged);

			if (Properties.Settings.Default.LastImageFile != String.Empty)
			{
				if (File.Exists(Properties.Settings.Default.LastImageFile))
				{
					LoadImage(Properties.Settings.Default.LastImageFile);
				}
				else
				{
					Uri resourceUri = new Uri("images/ColorRange.png", UriKind.Relative);
					System.Windows.Resources.StreamResourceInfo streamInfo = Application.GetResourceStream(resourceUri);

					BitmapFrame temp = BitmapFrame.Create(streamInfo.Stream);
					userImage.Source = temp;
				}


			}
			imageTabControl.SelectedIndex = Properties.Settings.Default.LastImageTabIndex;

			if (!String.IsNullOrEmpty(Properties.Settings.Default.LastFxFile))
			{
				if (File.Exists(Properties.Settings.Default.LastFxFile))
				{
					this.codeTabView.OpenFile(Properties.Settings.Default.LastFxFile);
					ApplyEffect(codeTabView.CurrentShaderEffect);
				}

			}
		}

		void codeTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			Properties.Settings.Default.LastImageTabIndex = imageTabControl.SelectedIndex;
			Properties.Settings.Default.Save();
		}

		void codeTabView_ShaderEffectChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
		{

			ApplyEffect(codeTabView.CurrentShaderEffect);
		}

		private void LoadImage(string fileName)
		{
			userImage.Source = null;
			userImage.Source = new BitmapImage(new Uri(fileName));

		}

		private void ApplyEffect(ShaderEffect se)
		{
			userImage.Effect = se;
			sampleImage1.Effect = se;
			sampleImage2.Effect = se;
			sampleImage3.Effect = se;
			sampleImage4.Effect = se;
			sampleImage5.Effect = se;
		}

		private void Open_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			var ofd = new Microsoft.Win32.OpenFileDialog();
			ofd.Filter = "Shader Files (*.fx)|*.fx|All Files|*.*";

			if (Properties.Settings.Default.FolderFX != string.Empty)
			{
				ofd.InitialDirectory = Properties.Settings.Default.FolderFX;
			}
			if (ofd.ShowDialog() == true)
			{
				codeTabView.OpenFile(ofd.FileName);
				Properties.Settings.Default.FolderFX = System.IO.Path.GetDirectoryName(ofd.FileName);
				Properties.Settings.Default.LastFxFile = ofd.FileName;
				Properties.Settings.Default.Save();

				if (ShazzamSwitchboard.FileLoaderPlugin != null)
				{
					ShazzamSwitchboard.FileLoaderPlugin.Update();
				}
			}
		}

		private void Save_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			try
			{
				codeTabView.SaveFile();
			}
			catch (UnauthorizedAccessException exception)
			{
				MessageBox.Show(this, exception.Message, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
				this.SaveAs_Executed(sender, e);
			}
		}

		private void SaveAs_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			var sfd = new Microsoft.Win32.SaveFileDialog();
			sfd.Filter = "FX files|*.fx;|All Files|*.*";
			sfd.InitialDirectory = Properties.Settings.Default.FolderFX;

			if (sfd.ShowDialog() == true)
			{
				codeTabView.SaveFile(sfd.FileName);
				Properties.Settings.Default.FolderFX = System.IO.Path.GetDirectoryName(sfd.FileName);
				Properties.Settings.Default.LastFxFile = sfd.FileName;
				Properties.Settings.Default.Save();

				if (ShazzamSwitchboard.FileLoaderPlugin != null)
				{
					ShazzamSwitchboard.FileLoaderPlugin.Update();
				}
			}
		}

		private void Exit_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			Application.Current.Shutdown();
		}

		private void ApplyShader_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			codeTabView.RenderShader();
		}

		private void CompileShader_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			codeTabView.CompileShader();
		}

		private void RemoveShader_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			userImage.Effect = null;
			sampleImage1.Effect = null;
			sampleImage2.Effect = null;
			sampleImage3.Effect = null;
			sampleImage4.Effect = null;
			sampleImage5.Effect = null;
		}

		private void ExploreCompiledShaders_Executed(object sender, System.Windows.RoutedEventArgs e)
		{
			string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + Constants.Paths.GeneratedShaders;
			System.Diagnostics.Process.Start(path);
		}

		private void FullScreenImage_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			if (codeRow.Height != new GridLength(0, GridUnitType.Pixel))
			{
				//	codeTabView.Visibility = Visibility.Collapsed;
				//imageTabControl.Visibility = Visibility.Visible;
				codeRow.Height = new GridLength(0, GridUnitType.Pixel);
				imageRow.Height = new GridLength(5, GridUnitType.Star);
			}
			else
			{
				//	codeTabView.Visibility = Visibility.Visible;
				//imageTabControl.Visibility = Visibility.Visible;
				codeRow.Height = new GridLength(5, GridUnitType.Star);
				imageRow.Height = new GridLength(5, GridUnitType.Star);
			}
		}

		private void FullScreenCode_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			//codeTabView.Visibility = Visibility;
			if (imageRow.Height != new GridLength(0, GridUnitType.Pixel))
			{
				imageRow.Height = new GridLength(0, GridUnitType.Pixel);
				codeRow.Height = new GridLength(5, GridUnitType.Star);
			}
			else
			{
				imageRow.Height = new GridLength(5, GridUnitType.Star);

			}

			//	DockPanel.SetDock(codeTabView, Dock.Bottom);
		}
		private void OpenImage_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.Filter = "Images|*.jpg;*.png;*.bmp;*.gif|All Files|*.*";

			if (Properties.Settings.Default.FolderImages != string.Empty)
			{
				ofd.InitialDirectory = Properties.Settings.Default.FolderImages;
			}
			if (ofd.ShowDialog(this) == true)
			{

				LoadImage(ofd.FileName);
				Properties.Settings.Default.LastImageFile = ofd.FileName;
				Properties.Settings.Default.FolderImages = System.IO.Path.GetDirectoryName(ofd.FileName);
				Properties.Settings.Default.Save();
			}
		}

		private void ShaderCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			string fxcPath = Environment.ExpandEnvironmentVariables(Properties.Settings.Default.DirectX_FxcPath);
			e.CanExecute = File.Exists(fxcPath);
		}
	}

}
