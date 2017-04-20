namespace Shazzam.Converters
{
    using System;
    using System.Windows;
    using System.Windows.Data;

    internal class VisibiltyToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var temp = (Visibility)value;
            if (temp == Visibility.Visible)
            {
                return Visibility.Collapsed;
            }

            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
