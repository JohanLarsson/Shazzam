namespace Shazzam.Converters
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;

    [ValueConversion(typeof(object), typeof(string))]
    public sealed class PairToStringConverter : IValueConverter
    {
        public static readonly PairToStringConverter F1 = new PairToStringConverter("F1");
        public static readonly PairToStringConverter F2 = new PairToStringConverter("F2");

        private readonly string format;

        public PairToStringConverter(string format)
        {
            this.format = format;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Point p)
            {
                return $"{p.X.ToString(this.format, CultureInfo.InvariantCulture)}, {p.Y.ToString(this.format, CultureInfo.InvariantCulture)}";
            }

            if (value is Vector v)
            {
                return $"{v.X.ToString(this.format, CultureInfo.InvariantCulture)}, {v.Y.ToString(this.format, CultureInfo.InvariantCulture)}";
            }

            if (value is Size s)
            {
                return $"{s.Width.ToString(this.format, CultureInfo.InvariantCulture)}, {s.Height.ToString(this.format, CultureInfo.InvariantCulture)}";
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string text &&
                text.Split(',') is string[] parts &&
                parts.Length == 2 &&
                double.TryParse(parts[0], NumberStyles.Float, CultureInfo.InvariantCulture, out var x) &&
                double.TryParse(parts[1], NumberStyles.Float, CultureInfo.InvariantCulture, out var y))
            {
                if (targetType == typeof(Point))
                {
                    return new Point(x,y);
                }

                if (targetType == typeof(Vector))
                {
                    return new Vector(x, y);
                }

                if (targetType == typeof(Size))
                {
                    return new Size(x, y);
                }
            }

            return Binding.DoNothing;
        }
    }
}
