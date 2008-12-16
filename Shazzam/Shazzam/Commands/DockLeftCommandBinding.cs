using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace Shazzam.Commands
{
  public sealed class DockLeftCommandBinding : CommandBinding
  {

    public DockLeftCommandBinding()

      : base(AppCommands.DockLeft, OnExecute, OnCanExecute)
    {

    }

    static void OnExecute(object sender, ExecutedRoutedEventArgs e)
    {

    }

    static void OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
    {

      e.CanExecute = true;

    }

  }

}
