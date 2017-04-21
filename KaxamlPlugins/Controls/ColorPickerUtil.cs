namespace KaxamlPlugins.Controls
{
    using System;
    using System.Windows.Media;

    public static class ColorPickerUtil
    {

        public static string MakeValidColorString(string S)
        {
            string s = S;

            // remove invalid characters (this is a very forgiving function)
            for (int i = 0; i < s.Length; i++)
            {
                char c = s[i];

                if (!(c >= 'a' && c <= 'f') &&
                    !(c >= 'A' && c <= 'F') &&
                    !(c >= '0' && c <= '9'))
                {
                    s = s.Remove(i, 1);
                    i--;
                }
            }

            // trim if too long
            if (s.Length > 8) s = s.Substring(0, 8);

            // pad with zeroes until a valid length is found
            while (s.Length <= 8 && s.Length != 3 && s.Length != 4 && s.Length != 6 && s.Length != 8)
            {
                s = s + "0";
            }

            return s;
        }

        public static Color ColorFromString(string S)
        {
            //ColorConverter converter = new ColorConverter();
            Color c = (Color) ColorConverter.ConvertFromString(S);

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

        static char[] hexDigits = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };

        public static string StringFromColor(Color c)
        {
            byte[] bytes = new byte[4];
            bytes[0] = c.A;
            bytes[1] = c.R;
            bytes[2] = c.G;
            bytes[3] = c.B;

            char[] chars = new char[bytes.Length * 2];

            for (int i = 0; i < bytes.Length; i++)
            {
                int b = bytes[i];
                chars[i * 2] = hexDigits[b >> 4];
                chars[i * 2 + 1] = hexDigits[b & 0xF];
            }

            return new string(chars);
        }

        public static void HSBFromColor(Color C, ref double H, ref double S, ref double B)
        {
            // standard algorithm from nearly any graphics textbook

            byte _red = C.R;
            byte _green = C.G;
            byte _blue = C.B;

            int imax = _red, imin = _red;

            if (_green > imax) imax = _green; else if (_green < imin) imin = _green;
            if (_blue > imax) imax = _blue; else if (_blue < imin) imin = _blue;
            double max = imax / 255.0, min = imin / 255.0;

            double value = max;
            double saturation = (max > 0) ? (max - min) / max : 0.0;
            double hue = 0;

            if (imax > imin)
            {
                double f = 1.0 / ((max - min) * 255.0);
                hue = (imax == _red) ? 0.0 + f * (_green - _blue)
                          : (imax == _green) ? 2.0 + f * (_blue - _red)
                              : 4.0 + f * (_red - _green);
                hue = hue * 60.0;
                if (hue < 0.0)
                    hue += 360.0;
            }

            H = hue / 360;
            S = saturation;
            B = value;
        }

        public static Color ColorFromAHSB(double A, double H, double S, double B)
        {
            Color r = ColorFromHSB(H, S, B);
            r.A = (byte)Math.Round(A * 255);
            return r;
        }

        public static Color ColorFromHSB(double H, double S, double B)
        {
            // standard algorithm from nearly any graphics textbook

            double red = 0.0, green = 0.0, blue = 0.0;

            if (S == 0.0)
            {
                red = green = blue = B;
            }
            else
            {
                double h = H * 360;
                while (h >= 360.0)
                    h -= 360.0;

                h = h / 60.0;
                int i = (int)h;

                double f = h - i;
                double r = B * (1.0 - S);
                double s = B * (1.0 - S * f);
                double t = B * (1.0 - S * (1.0 - f));

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