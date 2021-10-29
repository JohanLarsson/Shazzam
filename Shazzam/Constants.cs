namespace Shazzam
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Windows.Media;

    public static class Constants
    {
        public static readonly IReadOnlyList<Color> Colors = typeof(Colors)
                                                             .GetProperties(BindingFlags.Public | BindingFlags.Static)
                                                             .Where(x => x.PropertyType == typeof(Color))
                                                             .Select(x => (Color)x.GetValue(null))
                                                             .OrderBy(x => x.R + x.G + x.B)
                                                             .ToArray();

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
