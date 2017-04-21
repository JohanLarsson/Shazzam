namespace Shazzam.ViewModels
{
  class CodeTabViewModel : ViewModelBase
  {
    public bool AreGeneratedCodeTabsEnabled => Properties.Settings.Default.AreGeneratedCodeTabsEnabled;
  }
}
