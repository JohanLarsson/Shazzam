namespace Shazzam.Plugins
{
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using Shazzam.Helpers;

  public partial class About : UserControl
  {
    public About()
    {
        this.InitializeComponent();
      this.AddHandler(Hyperlink.RequestNavigateEvent, new RoutedEventHandler(this.HandleRequestNavigate), false);
        this.versionText.Text = VersionHelper.GetVersionNumber();
        this.versionRun.Text = string.Format("v{0}", VersionHelper.GetShortVersionNumber());
    }

    void HandleRequestNavigate(object sender, RoutedEventArgs e)
    {
      var hl = e.OriginalSource as Hyperlink;
      if (hl != null)
      {
        var navigateUri = hl.NavigateUri.ToString();
        Process.Start(new ProcessStartInfo(navigateUri));
        e.Handled = true;
      }
    }
  }
}
