using System;
using System.Deployment.Application;

namespace Shazzam.Helpers
{
  static class VersionHelper
  {

    internal static string GetVersionNumber()
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
    internal static string GetShortVersionNumber()
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
