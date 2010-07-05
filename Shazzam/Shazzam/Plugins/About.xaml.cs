using System;
using System.Deployment.Application;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace Shazzam.Plugins
{

  public partial class About : UserControl
  {
    public About()
    {
      InitializeComponent();
      this.AddHandler(Hyperlink.RequestNavigateEvent, new RoutedEventHandler(this.HandleRequestNavigate), false);
      versionText.Text = GetVersionNumber();
      versionRun.Text = String.Format("v{0}", GetShortVersionNumber());
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

      if (System.Deployment.Application.ApplicationDeployment.IsNetworkDeployed)
      {
        ourVersion = ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString();
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
    private string GetShortVersionNumber()
    {
      System.Reflection.Assembly _assemblyInfo = System.Reflection.Assembly.GetExecutingAssembly();

      string ourVersion = string.Empty;

      // if running the deployed application, you can get the version
      // from the ApplicationDeployment information. If you try
      // to access this when you are running in Visual Studio, it will not work.
      if (System.Deployment.Application.ApplicationDeployment.IsNetworkDeployed)
      {
        // ourVersion = ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString();
        ourVersion = String.Format("{0}.{1}",
                             ApplicationDeployment.CurrentDeployment.CurrentVersion.Major.ToString(),
                             ApplicationDeployment.CurrentDeployment.CurrentVersion.Minor.ToString());
      }
      else
      {
        if (_assemblyInfo != null)
        {
          ourVersion = String.Format("{0}.{1}",
                                      _assemblyInfo.GetName().Version.Major.ToString(),
                                      _assemblyInfo.GetName().Version.Minor.ToString());
        }
      }
      return ourVersion;

    }
  }
}
