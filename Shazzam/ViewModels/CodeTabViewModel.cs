namespace Shazzam.ViewModels
{
  class CodeTabViewModel : ViewModelBase
  {

    public bool AreGeneratedCodeTabsEnabled
    {
      get
      {
        return Shazzam.Properties.Settings.Default.AreGeneratedCodeTabsEnabled;
      }
    }

  }
}
