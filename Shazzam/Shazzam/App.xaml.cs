using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Diagnostics;
using Shazzam.Properties;
using System.IO;
using System.IO.IsolatedStorage;

namespace Shazzam
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{

		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);
			Application.Current.DispatcherUnhandledException += Current_DispatcherUnhandledException;
			AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
	

			try
			{
				Settings.Default.Reload();
				// attempt to load from config
				//string result = Shazzam.Properties.Settings.Default.FolderFX;
				if (string.IsNullOrEmpty(Settings.Default.FolderPath_Output))
				{
					var dirPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + Constants.Paths.GeneratedShaders;
					Settings.Default.FolderPath_Output = dirPath;
					Settings.Default.Save();
				}
				if (Directory.Exists(Settings.Default.FolderPath_Output) == false)
				{
					Directory.CreateDirectory(Settings.Default.FolderPath_Output);
				}
			}
			catch (ConfigurationErrorsException ex)
			{ //(requires System.Configuration)
				string filename = ((ConfigurationErrorsException)ex.InnerException).Filename;

				if (MessageBox.Show("Shazzam has detected that your" +
															" user settings file has become corrupted. " +
															"This may be due to a crash or improper exiting" +
															" of the program. Shazzam must reset your " +
															"user settings in order to continue.\n\nClick" +
															" Yes to reset your user settings.  You will need to restart.\n\n" +
															"Click No if you wish to attempt manual repair" +
															" or to rescue information before proceeding.",
															"Corrupt user settings",
															MessageBoxButton.YesNo,
															MessageBoxImage.Error) == MessageBoxResult.Yes)
				{
					File.Delete(filename);
					Settings.Default.Reload();
					Process.GetCurrentProcess().Kill();
					// you could optionally restart the app instead
				}
				else
				{
					Process.GetCurrentProcess().Kill();
					// avoid the inevitable crash
				}
				

			}
			catch (Exception ex)
			{
				throw;
			}
			try
			{
				var win = new MainWindow();
				win.Show();
			}
			catch (Exception ex)
			{
				using (var isoStore = IsolatedStorageFile.GetUserStoreForAssembly())
				{
					using (var isoStream = new IsolatedStorageFileStream(
									"starupError.txt", FileMode.Create, FileAccess.Write, isoStore))
					{
						using (var writer = new StreamWriter(isoStream))
						{
						

							writer.WriteLine(ex.Message);
							writer.WriteLine(ex.StackTrace);
						}
					}
				}
			}
		}

		void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
		
		}

		void Current_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
		{

			try
			{
				Settings.Default.Reload();


			}
			catch (ConfigurationErrorsException ex)
			{ //(requires System.Configuration)
				string filename = ((ConfigurationErrorsException)ex.InnerException).Filename;

				if (MessageBox.Show("Shazzam has detected that your" +
															" user settings file has become corrupted. " +
															"This may be due to a crash or improper exiting" +
															" of the program. Shazzam must reset your " +
															"user settings in order to continue.\n\nClick" +
															" Yes to reset your user settings and continue.\n\n" +
															"Click No if you wish to attempt manual repair" +
															" or to rescue information before proceeding.",
															"Corrupt user settings",
															MessageBoxButton.YesNo,
															MessageBoxImage.Error) == MessageBoxResult.Yes)
				{
					File.Delete(filename);
					Settings.Default.Reload();
					// you could optionally restart the app instead
				}
				else
					Process.GetCurrentProcess().Kill();
				// avoid the inevitable crash
			}
			EventLog log = new EventLog();

			MessageBox.Show(e.Exception.Message);
			using (var isoStore = IsolatedStorageFile.GetUserStoreForAssembly())
			{
				//IsolatedStorageFileStream represents the stream
				// write data to the stream
				using (var isoStream = new IsolatedStorageFileStream(
							"ErrorLog.txt", FileMode.OpenOrCreate, FileAccess.Write, isoStore))
				{
					using (var writer = new StreamWriter(isoStream))
					{
						writer.WriteLine(e.Exception.Message);
						writer.WriteLine(e.Exception.StackTrace);
					}
				}
			}
			Shazzam.Properties.Settings.Default.FilePath_LastFx = String.Empty;
			Shazzam.Properties.Settings.Default.Save();
		}
	}
}
