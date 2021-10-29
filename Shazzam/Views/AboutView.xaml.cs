namespace Shazzam.Views
{
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;

    public partial class AboutView : UserControl
    {
        public AboutView()
        {
            this.InitializeComponent();
            this.AddHandler(Hyperlink.RequestNavigateEvent, new RoutedEventHandler(OnRequestNavigate), handledEventsToo: false);
            this.versionText.Text = VersionHelper.GetVersionNumber();
            this.versionRun.Text = $"v{VersionHelper.GetShortVersionNumber()}";

            static void OnRequestNavigate(object sender, RoutedEventArgs e)
            {
                if (e.OriginalSource is Hyperlink hl)
                {
                    var navigateUri = hl.NavigateUri.ToString();
                    Process.Start(new ProcessStartInfo(navigateUri));
                    e.Handled = true;
                }
            }
        }
    }
}
