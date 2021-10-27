namespace Shazzam.Converters
{
    using System;
    using System.Windows;
    using System.Windows.Data;

    public sealed class StringToVisibilityConverter : IValueConverter
    {
        public static readonly StringToVisibilityConverter Default = new();

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var temp = value.ToString();
            if (temp.Length == 0)
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
