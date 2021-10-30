namespace Shazzam
{
    using System;
    using System.Windows;
    using System.Windows.Data;

    [ValueConversion(typeof(object), typeof(Visibility))]
    public sealed class VisibilityToVisibilityConverter : IValueConverter
    {
        public static readonly VisibilityToVisibilityConverter CollapsedWhenVisible = new();

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value switch
            {
                Visibility.Visible => Visibility.Collapsed,
                _ => Visibility.Visible,
            };
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException($"{nameof(VisibilityToVisibilityConverter)} can only be used in OneWay bindings");
        }
    }
}
