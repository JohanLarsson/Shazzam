namespace KaxamlPlugins
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    public class RoundConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                var v = double.Parse(value.ToString());
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
    }
}
