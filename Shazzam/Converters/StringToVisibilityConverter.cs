namespace Shazzam.Converters
{
    using System;
    using System.Windows;
    using System.Windows.Data;

    [ValueConversion(typeof(string), typeof(Visibility))]
    public sealed class StringToVisibilityConverter : IValueConverter
    {
        public static readonly StringToVisibilityConverter CollapsedWhenNullOrEmpty = new();

        public object Convert(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture)
        {
            return value switch
            {
                null => Visibility.Collapsed,
                string { Length: 0 } => Visibility.Collapsed,
                _ => Visibility.Visible,
            };
        }

        object IValueConverter.ConvertBack(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException($"{nameof(StringToVisibilityConverter)} can only be used in OneWay bindings");
        }
    }
}
