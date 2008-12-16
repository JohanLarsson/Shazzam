using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.ComponentModel;

namespace Shazzam.CodeGen {
	class ShaderCompiler : INotifyPropertyChanged{
		public void Compile(string codeText) {
			IsCompiled = false;
			// verify that the DirectX composer exe (fxc.exe) path is stored in settings
			if (Path.GetFileName(Properties.Settings.Default.DirectX_FxcPath).ToLower() != "fxc.exe")
			{
				throw new CompilerException("Ensure that the DirectX SDK is installed and that the correct path is configure in Settings pane.  \r\n\r\nCurrent setting for path is " + Properties.Settings.Default.DirectX_FxcPath);
			
			}

			// verify that the DirectX composer (fxc.exe) is available 
			if (File.Exists(Properties.Settings.Default.DirectX_FxcPath) == false)
			{
				throw new FileNotFoundException("Cannot find the DirectX FXC.exe file.  Ensure that the DirectX SDK is installed and that the correct path is configure in Settings pane. \r\n\r\nCurrent setting for path is " + Properties.Settings.Default.DirectX_FxcPath);
			
			}
			// create app folder
			string path = string.Format("{0}{1}", Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),Constants.Paths.GeneratedShaders);
			if (Directory.Exists(path) == false)
			{
				Directory.CreateDirectory(path);
			}
			// set path to temporary folder
		//	string path = string.Format("{0}\\tmp", Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));
			//string errorText;
			
			// create new file with fx extension
			using (FileStream fs = new FileStream(path + Constants.FileNames.TempShaderFx, FileMode.Create))
			{
				byte[] data = Encoding.ASCII.GetBytes(codeText);
				fs.Write(data, 0, data.Length);
			}

			ProcessStartInfo psi = new ProcessStartInfo(Properties.Settings.Default.DirectX_FxcPath);
			psi.CreateNoWindow = true;
			psi.UseShellExecute = false;

			psi.RedirectStandardError = true;
			// call fxc with these argurments
			// this will create the *.ps file 
			// the ps file is use  by the WPF framework when working with shaders
			psi.Arguments = string.Format("/T ps_2_0 /E main /Fo\"{0}\" \"{1}\"", path + Constants.FileNames.TempShaderPs, path+ Constants.FileNames.TempShaderFx);

			// launch the process
			ErrorText = string.Empty;
			// delete any existing ps files
			if (File.Exists(path + Constants.FileNames.TempShaderPs))
			{
				File.Delete(path + Constants.FileNames.TempShaderPs);
			}
			using (Process p = Process.Start(psi))
			{
				// monitor the error stream
				StreamReader sr = p.StandardError;
				ErrorText = sr.ReadToEnd().Replace(path + Constants.FileNames.TempShaderFx, "Source Line - ");
				if (ErrorText == string.Empty)
				{
					IsCompiled = true;
				}
				p.WaitForExit();
			}
			CreateFileCopies(path);
		}
		private void CreateFileCopies(string path) {
			if (String.IsNullOrEmpty(Properties.Settings.Default.LastFxFile))
			{
				return;
			}
			string currentFileName = System.IO.Path.GetFileNameWithoutExtension(Properties.Settings.Default.LastFxFile);
			if (File.Exists(path + Constants.FileNames.TempShaderPs))
			{
				
				File.Copy(path + Constants.FileNames.TempShaderPs, path + currentFileName +".ps",true);
			}
		}
		public void Reset() {
			ErrorText = "not compiled"; 
		}
		private string _errorText;
		public String ErrorText {
			get {
				return _errorText;
			}
			set {
			_errorText = value;
			RaiseNotifyChanged("ErrorText");
			}
		}

		private bool _isCompiled;
		public bool IsCompiled {
			get {
				return _isCompiled;
			}
			set {
				_isCompiled = value;
				RaiseNotifyChanged("IsCompiled");
			}
		}

		private void RaiseNotifyChanged(string propName) {
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(propName));
		}
	
		public event PropertyChangedEventHandler PropertyChanged;

		
	}
}
