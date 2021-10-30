namespace Shazzam
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    [ValueConversion(typeof(double), typeof(string))]
    public sealed class DoubleToStringConverter : IValueConverter
    {
        public static readonly DoubleToStringConverter F1 = new("F1");
        public static readonly DoubleToStringConverter F2 = new("F2");

        private readonly string format;

        public DoubleToStringConverter(string format)
        {
            this.format = format;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var number = (double)value;
            return number.ToString(this.format, CultureInfo.InvariantCulture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return System.Convert.ToDouble(value, CultureInfo.InvariantCulture);
        }
    }
}
