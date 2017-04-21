using System;
using System.Globalization;
using System.Windows.Data;

namespace KaxamlPlugins
{
    public class RoundConverter : IValueConverter
    {
        #region IValueConverter Members

        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                double v = double.Parse(value.ToString());
                return Math.Round(v, 2);
            }
            catch
            {
                return value;
            }
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        #endregion
    }
}