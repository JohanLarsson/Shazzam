using System;
using System.Deployment.Application;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using Shazzam.Helpers;

namespace Shazzam.Plugins
{

  public partial class About : UserControl
  {
    public About()
    {
      InitializeComponent();
      this.AddHandler(Hyperlink.RequestNavigateEvent, new RoutedEventHandler(this.HandleRequestNavigate), false);
      versionText.Text = VersionHelper.GetVersionNumber();
      versionRun.Text = String.Format("v{0}", VersionHelper.GetShortVersionNumber());
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


  }
}
