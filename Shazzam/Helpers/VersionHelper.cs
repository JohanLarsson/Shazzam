namespace Shazzam.Helpers
{
    using System.Deployment.Application;

    internal static class VersionHelper
    {
        internal static string GetVersionNumber()
        {
            var assemblyInfo = System.Reflection.Assembly.GetExecutingAssembly();

            // if running the deployed application, you can get the version
            //  from the ApplicationDeployment information. If you try
            //  to access this when you are running in Visual Studio, it will not work.
            var ourVersion = ApplicationDeployment.IsNetworkDeployed
                ? ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString()
                : assemblyInfo.GetName().Version.ToString();

            return ourVersion;
        }

        internal static string GetShortVersionNumber()
        {
            var assemblyInfo = System.Reflection.Assembly.GetExecutingAssembly();

            // if running the deployed application, you can get the version
            // from the ApplicationDeployment information. If you try
            // to access this when you are running in Visual Studio, it will not work.
            var ourVersion = ApplicationDeployment.IsNetworkDeployed
                ? $"{ApplicationDeployment.CurrentDeployment.CurrentVersion.Major}.{ApplicationDeployment.CurrentDeployment.CurrentVersion.Minor}"
                : $"{assemblyInfo.GetName().Version.Major}.{assemblyInfo.GetName().Version.Minor}";

            return ourVersion;
        }
    }
}
