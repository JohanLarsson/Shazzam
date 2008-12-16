using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using System.IO;

namespace Shazzam {
	//  Images
	//  creative commons license
	//  StuffEyeSee  http://www.flickr.com/photos/rcsaxon/689732379/
	//  http://www.flickr.com/photos/glockenblume/2228713567/sizes/l/
	//  http://www.flickr.com/photos/96dpi/2329024258/
	//
	public partial class MainWindow : Window {
		public MainWindow() {
			InitializeComponent();
			if (ShazzamSwitchboard.IsFXCompilerAvailable()==false)
			{
				codeTabView.Visibility = Visibility.Hidden;
				menuView.Visibility = Visibility.Hidden;
				imageTabControl.Visibility = Visibility.Hidden;
				string message = "Ensure that the DirectX SDK is installed and that the correct path is configure in Settings pane.  \r\n\r\nCurrent setting for path is " + Properties.Settings.Default.DirectX_FxcPath;
				MessageBox.Show(message);
				return;
			}
			ShazzamSwitchboard.MainWindow = this;
			ShazzamSwitchboard.CodeTabView = this.codeTabView;
			codeTabView.ShaderEffectChanged += new RoutedPropertyChangedEventHandler<object>(codeTabView_ShaderEffectChanged);
			imageTabControl.SelectionChanged += new SelectionChangedEventHandler(codeTabControl_SelectionChanged);

			if (Properties.Settings.Default.LastImageFile != String.Empty)
			{
				LoadImage(Properties.Settings.Default.LastImageFile);
			}
			imageTabControl.SelectedIndex = Properties.Settings.Default.LastImageTabIndex;

		}
		void codeTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e) {
			Properties.Settings.Default.LastImageTabIndex = imageTabControl.SelectedIndex;
			Properties.Settings.Default.Save();
		}

		void codeTabView_ShaderEffectChanged(object sender, RoutedPropertyChangedEventArgs<object> e) {

			ApplyEffect(codeTabView.CurrentShaderEffect);
		}

		private void LoadImage(string fileName) {
			userImage.Source = null;
			userImage.Source = new BitmapImage(new Uri(fileName));

		}

		private void ApplyEffect(ShaderEffect se) {
			userImage.Effect = se;
			sampleImage1.Effect = se;
			sampleImage2.Effect = se;
			sampleImage3.Effect = se;
			sampleImage4.Effect = se;
			sampleImage5.Effect = se;
		}

		private void Open_Executed(object sender, ExecutedRoutedEventArgs e) {
			codeTabView.OpenCodeFile();
		}
		private void Save_Executed(object sender, ExecutedRoutedEventArgs e) {

			codeTabView.SaveFile();
			//	csTextEditer.SaveFile(csTextEditer.FileName);
		}
		private void SaveAs_Executed(object sender, ExecutedRoutedEventArgs e) {
			var sfd = new Microsoft.Win32.SaveFileDialog();
			sfd.Filter = "FX files|*.fx;|All Files|*.*";
			sfd.InitialDirectory = Properties.Settings.Default.FolderFX;

			if (sfd.ShowDialog() == true)
			{
				codeTabView.SaveFile(sfd.FileName);
				//csTextEditer.SaveFile(sfd.FileName);
				Properties.Settings.Default.FolderFX = System.IO.Path.GetDirectoryName(sfd.FileName);
				Properties.Settings.Default.LastFxFile = sfd.FileName;
				Properties.Settings.Default.Save();

			}
		}

		private void AppyShader_Executed(object sender, ExecutedRoutedEventArgs e) {
			//	codeTabView.CompileShader();

			codeTabView.RenderShader();
			//	ApplyEffect(codeTabView.CurrentShaderEffect);
		}
		private void CompileShader_Executed(object sender, ExecutedRoutedEventArgs e) {

			codeTabView.CompileShader();
			//	ApplyEffect(codeTabView.CurrentShaderEffect);
		}
		private void RemoveShader_Executed(object sender, ExecutedRoutedEventArgs e) {
			userImage.Effect = null;
			sampleImage1.Effect = null;
			sampleImage2.Effect = null;
			sampleImage3.Effect = null;
			sampleImage4.Effect = null;
			sampleImage5.Effect = null;
		}

		private void FullScreen_Executed(object sender, ExecutedRoutedEventArgs e) {
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

		//private void DockLeft_Executed(object sender, ExecutedRoutedEventArgs e) {
		//  codeTabView.Visibility = Visibility;
		//  //DockPanel.SetDock(codeTabView, Dock.Left);
		//}

		private void FullScreenCode_Executed(object sender, ExecutedRoutedEventArgs e) {
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
		private void OpenImage_Executed(object sender, ExecutedRoutedEventArgs e) {
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

		private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e) {
			e.CanExecute = true;
		}

		private void CompileShaderBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e) {
			e.CanExecute = File.Exists(Properties.Settings.Default.DirectX_FxcPath);

		}

		private void CommandBinding_CanExecute_1(object sender, CanExecuteRoutedEventArgs e) {
			e.CanExecute = File.Exists(Properties.Settings.Default.DirectX_FxcPath);

		}

	}

}
