using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Shazzam.ViewModels
{

  public class ViewModelBase : INotifyPropertyChanged
  {

    public event PropertyChangedEventHandler PropertyChanged;

    internal protected void NotifyPropertyChanged<T>(Expression<Func<T>> propertyAccessor)
    {
      if (this.PropertyChanged == null)
      {
        return;
      }
      MemberExpression mExpress = (MemberExpression)propertyAccessor.Body;
      string name = mExpress.Member.Name;
      this.PropertyChanged(this, new PropertyChangedEventArgs(name));
    }

  }
}
