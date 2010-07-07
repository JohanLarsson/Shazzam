using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
