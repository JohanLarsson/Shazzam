namespace Shazzam.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    public sealed class StretchToIsCheckedConverter : IValueConverter
    {
        public static readonly StretchToIsCheckedConverter Default = new();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var mode = (System.Windows.Media.Stretch)value;
            if (mode.ToString().ToLower() == parameter.ToString().ToLower())
            {
                return true;
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(bool)value;
        }
    }
}
