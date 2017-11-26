namespace Shazzam.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    [ValueConversion(typeof(double), typeof(string))]
    public sealed class DoubleToStringConverter : IValueConverter
    {
        public static readonly DoubleToStringConverter Default = new DoubleToStringConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var number = (double)value;
            return number.ToString("F2", CultureInfo.InvariantCulture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return System.Convert.ToDouble(value, CultureInfo.InvariantCulture);
        }
    }
}
