using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows.Forms;

namespace Shazzam.Commands
{
  class CompileShaderCommandBinding : CommandBinding
  {

		public CompileShaderCommandBinding()

      : base(AppCommands.CompileShader, OnExecute, OnCanExecute)
    {

    }

    static void OnExecute(object sender, ExecutedRoutedEventArgs e)
    {
    
    }

    static void OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
    {

      e.CanExecute = false;

    }

  }

}
