using System;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.ComponentModel;
using System.Reflection;

namespace Shazzam.CodeGen
{
   [Obsolete("Replaced by ShaderCompiler")]
	class ShaderCompiler2 : INotifyPropertyChanged
	{
		public void Compile(string codeText, ShaderProfile shaderProfile)
		{

			IsCompiled = false;
			//// verify that the DirectX composer exe (fxc.exe) path is stored in settings
			//string fxcPath = Environment.ExpandEnvironmentVariables(Properties.Settings.Default.DirectX_FxcPath);
			//if (Path.GetFileName(fxcPath).ToLower() != "fxc.exe" || !File.Exists(fxcPath))
			//{
			//  throw new CompilerException("Could not find the effect compiler \"fxc.exe\". " +
			//    "Ensure that the DirectX SDK is installed and the correct path is configured in the Settings pane.\n\n" +
			//    "The current setting is \"" + Properties.Settings.Default.DirectX_FxcPath + "\".");
			//}

			// verify that we have a copy Of the Local DirectX fxc.exe File.
			string fxcLocalPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "fxc64.exe");
			if (Path.GetFileName(fxcLocalPath) != "fxc64.exe" || !File.Exists(fxcLocalPath))
			{
				throw new CompilerException("Could not find the effect compiler \"fxc64.exe\". ");
			}

			// create application folder
			string path = Properties.Settings.Default.FolderPath_Output;
			if (Directory.Exists(path) == false)
			{
				Directory.CreateDirectory(path);
			}

			// create new file with fx extension
			using (FileStream fs = new FileStream(path + Constants.FileNames.TempShaderFx, FileMode.Create))
			{
				byte[] data = Encoding.ASCII.GetBytes(codeText);
				fs.Write(data, 0, data.Length);
			}

			ProcessStartInfo psi = new ProcessStartInfo(fxcLocalPath);
			psi.CreateNoWindow = true;
			psi.UseShellExecute = false;

			psi.RedirectStandardError = true;
			// call fxc with these arguments
			// this will create the *.ps file 
			// the ps file is use  by the WPF framework when working with shaders
		//	var compiler = new dx9fxutil.PixelShaderCompiler();
		
			if (shaderProfile == ShaderProfile.PixelShader2)
			{
			//	compiler.Compile(
				psi.Arguments = string.Format("/T ps_2_0 /E main /Fo\"{0}\" \"{1}\"", path + Constants.FileNames.TempShaderPs, path + Constants.FileNames.TempShaderFx);
			}
			else
			{
				psi.Arguments = string.Format("/T ps_3_0 /E main /Fo\"{0}\" \"{1}\"", path + Constants.FileNames.TempShaderPs, path + Constants.FileNames.TempShaderFx);
			}
		

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
		private void CreateFileCopies(string path)
		{
			if (String.IsNullOrEmpty(Properties.Settings.Default.FilePath_LastFx))
			{
				return;
			}
			string currentFileName = System.IO.Path.GetFileNameWithoutExtension(Properties.Settings.Default.FilePath_LastFx);
			if (File.Exists(path + Constants.FileNames.TempShaderPs))
			{

				File.Copy(path + Constants.FileNames.TempShaderPs, path + currentFileName + ".ps", true);
			}
		}
		public void Reset()
		{
			ErrorText = "not compiled";
		}
		private string _errorText;
		public String ErrorText
		{
			get
			{
				return _errorText;
			}
			set
			{
				_errorText = value;
				RaiseNotifyChanged("ErrorText");
			}
		}

		private bool _isCompiled;
		public bool IsCompiled
		{
			get
			{
				return _isCompiled;
			}
			set
			{
				_isCompiled = value;
				RaiseNotifyChanged("IsCompiled");
			}
		}

		private void RaiseNotifyChanged(string propName)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(propName));
		}

		public event PropertyChangedEventHandler PropertyChanged;


	}
}
