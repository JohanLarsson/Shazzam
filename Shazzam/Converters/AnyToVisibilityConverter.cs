namespace Shazzam.Converters
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Windows;
    using System.Windows.Data;

    [ValueConversion(typeof(IEnumerable), typeof(Visibility))]
    public sealed class AnyToVisibilityConverter : IValueConverter
    {
        public static readonly AnyToVisibilityConverter VisibleWhenAny = new(Visibility.Visible, Visibility.Collapsed);
        public static readonly AnyToVisibilityConverter VisibleWhenEmpty = new(Visibility.Collapsed, Visibility.Visible);

        private readonly object whenAny;
        private readonly object whenEmpty;

        public AnyToVisibilityConverter(Visibility whenAny, Visibility whenEmpty)
        {
            this.whenAny = whenAny;
            this.whenEmpty = whenEmpty;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is IEnumerable<object> enumerable)
            {
                return enumerable.Any() ? this.whenAny : this.whenEmpty;
            }

            return value;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException($"{nameof(AnyToVisibilityConverter)} can only be used in OneWay bindings");
        }
    }
}