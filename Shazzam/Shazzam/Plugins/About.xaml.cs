using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Deployment.Application;

namespace Shazzam.Plugins
{

	public partial class About : UserControl
	{
		public About()
		{
			InitializeComponent();
			this.AddHandler(Hyperlink.RequestNavigateEvent, new RoutedEventHandler(this.HandleRequestNavigate), false);
			versionText.Text = GetVersionNumber();
		}

		void HandleRequestNavigate(object sender, RoutedEventArgs e)
		{
			Hyperlink hl = (e.OriginalSource as Hyperlink);
			if (hl != null)
			{
				string navigateUri = hl.NavigateUri.ToString();
				Process.Start(new ProcessStartInfo(navigateUri));
				e.Handled = true;
			}
		}

		private string GetVersionNumber()
		{
			System.Reflection.Assembly _assemblyInfo = System.Reflection.Assembly.GetExecutingAssembly();

			string ourVersion = string.Empty;

			//if running the deployed application, you can get the version
			//  from the ApplicationDeployment information. If you try
			//  to access this when you are running in Visual Studio, it will not work.
			if
			(System.Deployment.Application.ApplicationDeployment.IsNetworkDeployed)
			{
			ourVersion =ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString();
			}
			else
			{
			if (_assemblyInfo != null)
			{
			ourVersion = _assemblyInfo.GetName().Version.ToString();
			}
			}
			return ourVersion;

			        }
		}
	}

