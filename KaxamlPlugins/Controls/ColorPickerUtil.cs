namespace KaxamlPlugins.Controls
{
    using System;
    using System.Windows.Media;

    public static class ColorPickerUtil
    {
        private static readonly char[] HexDigits = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };

        public static Color ColorFromString(string text) => (Color)ColorConverter.ConvertFromString(text);

        public static void HsbFromColor(Color c, out double h, out double s, out double b)
        {
            // standard algorithm from nearly any graphics textbook
            var red = c.R;
            var green = c.G;
            var blue = c.B;

            int imax = red, imin = red;

            if (green > imax)
            {
                imax = green;
            }
            else if (green < imin)
            {
                imin = green;
            }

            if (blue > imax)
            {
                imax = blue;
            }
            else if (blue < imin)
            {
                imin = blue;
            }

            double max = imax / 255.0, min = imin / 255.0;

            var value = max;
            var saturation = (max > 0) ? (max - min) / max : 0.0;
            double hue = 0;

            if (imax > imin)
            {
                var f = 1.0 / ((max - min) * 255.0);
                hue = (imax == red) ? 0.0 + (f * (green - blue))
                          : (imax == green) ? 2.0 + (f * (blue - red))
                              : 4.0 + (f * (red - green));
                hue *= 60.0;
                if (hue < 0.0)
                {
                    hue += 360.0;
                }
            }

            h = hue / 360;
            s = saturation;
            b = value;
        }

        public static Color ColorFromAhsb(double a, double h, double s, double b)
        {
            var r = ColorFromHsb(h, s, b);
            r.A = (byte)Math.Round(a * 255);
            return r;
        }

#pragma warning disable SA1313 // Parameter names must begin with lower-case letter
        public static Color ColorFromHsb(double H, double S, double B)
#pragma warning restore SA1313 // Parameter names must begin with lower-case letter
        {
            // standard algorithm from nearly any graphics textbook
            double red = 0.0, green = 0.0, blue = 0.0;

            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (S == 0.0)
            {
                red = green = blue = B;
            }
            else
            {
                var h = H * 360;
                while (h >= 360.0)
                {
                    h -= 360.0;
                }

                h /= 60.0;
                var i = (int)h;

                var f = h - i;
                var r = B * (1.0 - S);
                var s = B * (1.0 - (S * f));
                var t = B * (1.0 - (S * (1.0 - f)));

                switch (i)
                {
                    case 0: red = B; green = t; blue = r; break;
                    case 1: red = s; green = B; blue = r; break;
                    case 2: red = r; green = B; blue = t; break;
                    case 3: red = r; green = s; blue = B; break;
                    case 4: red = t; green = r; blue = B; break;
                    case 5: red = B; green = r; blue = s; break;
                }
            }

            byte iRed = (byte)(red * 255.0), iGreen = (byte)(green * 255.0), iBlue = (byte)(blue * 255.0);
            return Color.FromRgb(iRed, iGreen, iBlue);
        }
    }
}
