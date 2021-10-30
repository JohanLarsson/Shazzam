namespace Shazzam
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    [ValueConversion(typeof(bool), typeof(bool))]
    public sealed class InvertBooleanConverter : IValueConverter
    {
        public static readonly InvertBooleanConverter Default = new();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(bool)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(bool)value;
        }
    }
}
