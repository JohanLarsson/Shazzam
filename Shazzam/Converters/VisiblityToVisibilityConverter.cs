using System;
using System.Windows.Data;
using System.Windows;

namespace Shazzam.Converters
{
  class VisibiltyToVisibilityConverter : IValueConverter
  {
    #region IValueConverter Members

    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      Visibility temp = (Visibility)value;
      if(temp==Visibility.Visible){
        return Visibility.Collapsed;

      }
      return Visibility.Visible;
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      throw new NotImplementedException();
    }

    #endregion
  }
}
