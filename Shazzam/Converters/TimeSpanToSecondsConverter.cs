namespace Shazzam.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    [ValueConversion(typeof(TimeSpan), typeof(double))]
    public sealed class TimeSpanToSecondsConverter : IValueConverter
    {
        public static readonly TimeSpanToSecondsConverter Default = new TimeSpanToSecondsConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((TimeSpan)value).TotalSeconds;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double d)
            {
                return TimeSpan.FromSeconds(d);
            }

            if (value is int i)
            {
                return TimeSpan.FromSeconds(i);
            }

            if (value is string s &&
                double.TryParse(s, NumberStyles.Float, CultureInfo.InvariantCulture, out d))
            {
                return TimeSpan.FromSeconds(d);
            }

            throw new ArgumentException(nameof(value));
        }
    }
}