namespace Shazzam
{
    using System;
    using System.Windows.Media;

    public static class ColorExtensions
    {
        // https://stackoverflow.com/a/26233318/1069200
        public static double Hue(this Color c)
        {
            var min = Math.Min(Math.Min(c.ScR, c.ScG), c.ScB);
            var max = Math.Max(Math.Max(c.ScR, c.ScG), c.ScB);

            if (min == max)
            {
                return 0;
            }

            var hue = 0f;
            if (max == c.ScR)
            {
                hue = (c.ScG - c.ScB) / (max - min);
            }
            else if (max == c.ScG)
            {
                hue = 2f + ((c.ScB - c.ScR) / (max - min));
            }
            else
            {
                hue = 4f + ((c.ScR - c.ScG) / (max - min));
            }

            hue *= 60;
            if (hue < 0)
            {
                hue += 360;
            }

            return Math.Round(hue);
        }
    }
}
