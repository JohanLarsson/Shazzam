namespace Shazzam
{
    using System.Windows;
    using Shazzam.Views;

    internal static class ShazzamSwitchboard
    {
        public static Window MainWindow { get; set; }

        public static CodeTabView CodeTabView { get; set; }

        public static FileLoaderPlugin FileLoaderPlugin { get; set; }
    }
}
