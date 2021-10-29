namespace Shazzam
{
    using System;
    using System.Windows.Media;

    public struct Hsv
    {
        public Hsv(double hue, double saturation, double value)
        {
            if (hue is < 0 or > 360)
            {
                throw new ArgumentOutOfRangeException(nameof(hue), hue, "Expected 0..360°");
            }

            if (saturation is < 0 or > 1)
            {
                throw new ArgumentOutOfRangeException(nameof(saturation), saturation, "Expected 0..1");
            }

            if (value is < 0 or > 1)
            {
                throw new ArgumentOutOfRangeException(nameof(value), value, "Expected 0..1");
            }

            this.Hue = hue;
            this.Saturation = saturation;
            this.Value = value;
        }

        public double Hue { get; }

        public double Saturation { get; }

        public double Value { get; }

        public static Hsv From(Color color)
        {
            return From(System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B));
        }

        // http://stackoverflow.com/a/1626175/1069200
        public static Hsv From(System.Drawing.Color color)
        {
            int max = Math.Max(color.R, Math.Max(color.G, color.B));
            int min = Math.Min(color.R, Math.Min(color.G, color.B));

            var hue = color.GetHue();
            var saturation = max == 0 ? 0 : 1d - (1d * min / max);
            var value = max / 255d;
            return new Hsv(hue, saturation, value);
        }

        public static Color ColorFromHsv(double hue, double saturation, double value)
        {
            if (hue is < 0 or > 360)
            {
                throw new ArgumentOutOfRangeException(nameof(hue), hue, "Expected 0..360°");
            }

            if (saturation is < 0 or > 1)
            {
                throw new ArgumentOutOfRangeException(nameof(saturation), saturation, "Expected 0..1");
            }

            if (value is < 0 or > 1)
            {
                throw new ArgumentOutOfRangeException(nameof(value), value, "Expected 0..1");
            }

            var hi = (int)Math.Floor(hue / 60) % 6;
            var f = (hue / 60) - Math.Floor(hue / 60);

            value = value * 255;
            var v = (byte)value;
            var p = (byte)(value * (1 - saturation));
            var q = (byte)(value * (1 - (f * saturation)));
            var t = (byte)(value * (1 - ((1 - f) * saturation)));

            return hi switch
            {
                0 => System.Windows.Media.Color.FromArgb(255, v, t, p),
                1 => System.Windows.Media.Color.FromArgb(255, q, v, p),
                2 => System.Windows.Media.Color.FromArgb(255, p, v, t),
                3 => System.Windows.Media.Color.FromArgb(255, p, q, v),
                4 => System.Windows.Media.Color.FromArgb(255, t, p, v),
                _ => System.Windows.Media.Color.FromArgb(255, v, p, q),
            };
        }

        public Color Color() => ColorFromHsv(this.Hue, this.Saturation, this.Value);
    }
}
