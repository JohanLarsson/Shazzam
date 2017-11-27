namespace Shazzam.Helpers
{
    internal static class VersionHelper
    {
        internal static string GetVersionNumber() => System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

        internal static string GetShortVersionNumber()
        {
            var assemblyInfo = System.Reflection.Assembly.GetExecutingAssembly();
            return $"{assemblyInfo.GetName().Version.Major}.{assemblyInfo.GetName().Version.Minor}";
        }
    }
}
