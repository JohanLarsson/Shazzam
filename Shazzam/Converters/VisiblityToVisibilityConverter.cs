﻿namespace Shazzam.Converters
{
    using System;
    using System.Windows;
    using System.Windows.Data;

    internal sealed class VisibiltyToVisibilityConverter : IValueConverter
    {
        internal static readonly VisibiltyToVisibilityConverter Default = new();

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is Visibility visibility &&
                visibility == Visibility.Visible)
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
