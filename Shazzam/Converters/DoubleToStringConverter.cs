namespace Shazzam.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    public class DoubleToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double number = (double)value;
            return number.ToString("F2");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string number = value.ToString();
            double cleanedNumber;
            if (double.TryParse(number, NumberStyles.Any, null, out cleanedNumber))
            {
                return cleanedNumber;
            }

            return number;
        }
    }
}
