using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows.Forms;

namespace Shazzam.Commands
{
  class RemoveShaderCommandBinding : CommandBinding
  {

    public RemoveShaderCommandBinding()

      : base(AppCommands.RemoveShader, OnExecute, OnCanExecute)
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
