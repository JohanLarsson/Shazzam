using System;
using System.Globalization;
using System.Windows.Data;

namespace Shazzam.Converters
{
  public class StretchToIsCheckedConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      var mode = (System.Windows.Media.Stretch)value;
      if (mode.ToString().ToLower() == parameter.ToString().ToLower())
      {
        return true;
      }
      return false;

    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      return !(bool)value;
    }
  }
}

