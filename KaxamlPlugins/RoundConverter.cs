namespace KaxamlPlugins
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    [ValueConversion(typeof(string), typeof(double))]
    [ValueConversion(typeof(double), typeof(double))]
    public sealed class RoundConverter : IValueConverter
    {
        public static readonly RoundConverter Default = new();

        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value switch
            {
                string s
                    when double.TryParse(s, out var d)
                    => Math.Round(d, 2),
                double d => Math.Round(d, 2),
                _ => value,
            };
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
