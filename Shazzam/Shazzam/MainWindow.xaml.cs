using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using System.Diagnostics;
using System.Reflection;
using Shazzam.Properties;
using System.Configuration;

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
				else
				{
					Properties.Settings.Default.LastFxFile = string.Empty;
					Properties.Settings.Default.Save();
				}
			}
			this.Closing += new System.ComponentModel.CancelEventHandler(MainWindow_Closing);
		}

		void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			ShazzamSwitchboard.CodeTabView.SaveFileFirst();
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

		private void New_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			//codeTabView.NewShader();
			var dialog = new SaveFileDialog();
			dialog.Title = "New File Name";
			if (Properties.Settings.Default.FolderFX != string.Empty)
			{
				dialog.InitialDirectory = Properties.Settings.Default.FolderFX;
			}
			dialog.CheckPathExists = true;
			dialog.CreatePrompt = true;
			dialog.Filter = "Shader File (*.fx) |*.fx";
			if (dialog.ShowDialog() == true)
			{
				if (!IsValidFileName(dialog.SafeFileName))
				{ return; }
				FileStream temp = new FileStream(dialog.FileName, FileMode.Create, FileAccess.ReadWrite);
				StreamWriter writer = new StreamWriter(temp);
				writer.Write(Properties.Resources.NewShaderText);
				writer.Close();
				LoadShaderEditor(dialog);

			}

		}
		private static bool IsValidFileName(string filename)
		{
			if (string.Equals(filename, Constants.FileNames.TempShaderFx, StringComparison.OrdinalIgnoreCase))
			{
				MessageBox.Show(String.Format("'{0}' not allowed for file name as it is reserved for Shazzam.", Constants.FileNames.TempShaderFx));
				return false;
			}
			return true;
		}
		private void LoadShaderEditor(FileDialog ofd)
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
		private void Open_Executed(object sender, ExecutedRoutedEventArgs e)
		{

			var dialog = new Microsoft.Win32.OpenFileDialog();
			dialog.Filter = "Shader Files (*.fx)|*.fx|All Files|*.*";
			if (Properties.Settings.Default.FolderFX != string.Empty)
			{
				dialog.InitialDirectory = Properties.Settings.Default.FolderFX;
			}
			if (dialog.ShowDialog() == true)
			{
				if (!IsValidFileName(dialog.SafeFileName))
				{ return; }
				LoadShaderEditor(dialog);
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
			string path = Properties.Settings.Default.FolderOutput;
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
		private void WhatsNew_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			Process.Start("http://blog.shazzam-tool.com/");
		}
	}

}
