namespace Shazzam
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Media;

    public static class Constants
    {
        public static readonly IReadOnlyList<Color> Colors = CreateColors(360).ToArray();

        private static IEnumerable<Color> CreateColors(int n)
        {
            var step = 360.0 / n;
            var hue = 0.0;
            while (hue < 360.0)
            {
                yield return Hsv.ColorFromHsv(hue, 1, 1);
                hue += step;
            }
        }

        public static class Paths
        {
            public static readonly string Application = "\\Shazzam\\";
            public static readonly string GeneratedShaders = "\\Shazzam\\GeneratedShaders\\";
            public static readonly string TextureMaps = "\\Images\\TextureMaps\\";
        }

        public static class FileNames
        {
            public static readonly string TempShaderFx = "temp.fx";
            public static readonly string TempShaderPs = "temp.ps";
        }
    }
}
