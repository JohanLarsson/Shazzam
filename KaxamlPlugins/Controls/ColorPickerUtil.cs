namespace KaxamlPlugins.Controls
{
    using System;
    using System.Windows.Media;

    public static class ColorPickerUtil
    {
        private static readonly char[] HexDigits = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };

        public static string MakeValidColorString(string text)
        {
            // remove invalid characters (this is a very forgiving function)
            for (var i = 0; i < text.Length; i++)
            {
                var c = text[i];

                if (!(c is >= 'a' and <= 'f') &&
                    !(c is >= 'A' and <= 'F') &&
                    !(c is >= '0' and <= '9'))
                {
                    text = text.Remove(i, 1);
                    i--;
                }
            }

            // trim if too long
            if (text.Length > 8)
            {
                text = text.Substring(0, 8);
            }

            // pad with zeroes until a valid length is found
            while (text.Length <= 8 && text.Length != 3 && text.Length != 4 && text.Length != 6 && text.Length != 8)
            {
                text += "0";
            }

            return text;
        }

        public static Color ColorFromString(string text)
        {
            // ReSharper disable once PossibleNullReferenceException
            var c = (Color)ColorConverter.ConvertFromString(text);
            return c;
            /*
            string s = MakeValidColorString(S);

            byte A = 255;
            byte R = 0;
            byte G = 0;
            byte B = 0;

            // interpret 3 characters as RRGGBB (where R, G, and B are each repeated)
            if (s.Length == 3)
            {
                R = byte.Parse(s.Substring(0, 1) + s.Substring(0, 1), System.Globalization.NumberStyles.HexNumber);
                G = byte.Parse(s.Substring(1, 1) + s.Substring(1, 1), System.Globalization.NumberStyles.HexNumber);
                B = byte.Parse(s.Substring(2, 1) + s.Substring(2, 1), System.Globalization.NumberStyles.HexNumber);
            }

            // interpret 4 characters as AARRGGBB (where A, R, G, and B are each repeated)
            if (s.Length == 4)
            {
                A = byte.Parse(s.Substring(0, 1) + s.Substring(0, 1), System.Globalization.NumberStyles.HexNumber);
                R = byte.Parse(s.Substring(1, 1) + s.Substring(1, 1), System.Globalization.NumberStyles.HexNumber);
                G = byte.Parse(s.Substring(2, 1) + s.Substring(2, 1), System.Globalization.NumberStyles.HexNumber);
                B = byte.Parse(s.Substring(3, 1) + s.Substring(3, 1), System.Globalization.NumberStyles.HexNumber);
            }

            // interpret 6 characters as RRGGBB
            if (s.Length == 6)
            {
                R = byte.Parse(s.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
                G = byte.Parse(s.Substring(1, 2), System.Globalization.NumberStyles.HexNumber);
                B = byte.Parse(s.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            }

            // interpret 8 characters as AARRGGBB
            if (s.Length == 8)
            {
                A = byte.Parse(s.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
                R = byte.Parse(s.Substring(1, 2), System.Globalization.NumberStyles.HexNumber);
                G = byte.Parse(s.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
                B = byte.Parse(s.Substring(3, 2), System.Globalization.NumberStyles.HexNumber);
            }

            return Color.FromArgb(A, R, G, B);
             */
        }

        public static string StringFromColor(Color c)
        {
            var bytes = new byte[4];
            bytes[0] = c.A;
            bytes[1] = c.R;
            bytes[2] = c.G;
            bytes[3] = c.B;

            var chars = new char[bytes.Length * 2];

            for (var i = 0; i < bytes.Length; i++)
            {
                int b = bytes[i];
                chars[i * 2] = HexDigits[b >> 4];
                chars[(i * 2) + 1] = HexDigits[b & 0xF];
            }

            return new string(chars);
        }

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
