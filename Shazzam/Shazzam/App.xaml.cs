using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Diagnostics;
using Shazzam.Properties;
using System.IO;

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

			try
			{
				Settings.Default.Reload();
				// attempt to load from config
				string result = Shazzam.Properties.Settings.Default.FolderFX;

				if (string.IsNullOrEmpty(Settings.Default.FolderOutput))
				{
					var dirPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + Constants.Paths.GeneratedShaders;
					Settings.Default.FolderOutput = dirPath;
					Settings.Default.Save();

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
			var win = new MainWindow();
			win.Show();
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

			MessageBox.Show(e.Exception.Message);

		}
	}
}
