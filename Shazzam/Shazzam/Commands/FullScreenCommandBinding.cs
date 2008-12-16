using System.Windows.Input;

namespace Shazzam.Commands
{
  public sealed class FullScreenCommandBinding : CommandBinding
  {

    public FullScreenCommandBinding()

      : base(AppCommands.FullScreen, OnExecute, OnCanExecute)
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
