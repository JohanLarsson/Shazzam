namespace Shazzam.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    [ValueConversion(typeof(object), typeof(bool))]
    public sealed class EqualityConverter : IValueConverter
    {
        public static readonly EqualityConverter Default = new();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null && value.ToString() == parameter.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? parameter : Binding.DoNothing;
        }
    }
}
