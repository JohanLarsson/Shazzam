namespace Shazzam.Helpers
{
    using System.Deployment.Application;

    internal static class VersionHelper
    {
        internal static string GetVersionNumber()
        {
            System.Reflection.Assembly assemblyInfo = System.Reflection.Assembly.GetExecutingAssembly();

            string ourVersion = string.Empty;

            // if running the deployed application, you can get the version
            //  from the ApplicationDeployment information. If you try
            //  to access this when you are running in Visual Studio, it will not work.
            if (ApplicationDeployment.IsNetworkDeployed)
            {
                ourVersion = ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString();
            }
            else
            {
                if (assemblyInfo != null)
                {
                    ourVersion = assemblyInfo.GetName().Version.ToString();
                }
            }

            return ourVersion;
        }

        internal static string GetShortVersionNumber()
        {
            System.Reflection.Assembly assemblyInfo = System.Reflection.Assembly.GetExecutingAssembly();

            string ourVersion = string.Empty;

            // if running the deployed application, you can get the version
            // from the ApplicationDeployment information. If you try
            // to access this when you are running in Visual Studio, it will not work.
            if (ApplicationDeployment.IsNetworkDeployed)
            {
                // ourVersion = ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString();
                ourVersion = string.Format(
                    "{0}.{1}",
                                     ApplicationDeployment.CurrentDeployment.CurrentVersion.Major.ToString(),
                                     ApplicationDeployment.CurrentDeployment.CurrentVersion.Minor.ToString());
            }
            else
            {
                if (assemblyInfo != null)
                {
                    ourVersion = string.Format(
                        "{0}.{1}",
                                                assemblyInfo.GetName().Version.Major.ToString(),
                                                assemblyInfo.GetName().Version.Minor.ToString());
                }
            }

            return ourVersion;
        }
    }
}
