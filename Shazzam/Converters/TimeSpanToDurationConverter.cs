namespace Shazzam.Converters
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;

    [ValueConversion(typeof(TimeSpan), typeof(Duration))]
    public sealed class TimeSpanToDurationConverter : IValueConverter
    {
        public static readonly TimeSpanToDurationConverter Default = new TimeSpanToDurationConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new Duration((TimeSpan) value);
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException($"{nameof(TimeSpanToDurationConverter)} can only be used in OneWay bindings");
        }
    }
}