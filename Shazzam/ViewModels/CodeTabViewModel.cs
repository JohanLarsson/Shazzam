namespace Shazzam.ViewModels
{
  class CodeTabViewModel : ViewModelBase
  {
    public bool AreGeneratedCodeTabsEnabled
    {
      get
      {
        return Properties.Settings.Default.AreGeneratedCodeTabsEnabled;
      }
    }
  }
}
