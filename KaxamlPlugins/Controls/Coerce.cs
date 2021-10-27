namespace KaxamlPlugins.Controls
{
    public static class Coerce
    {
        public static object? ClampDouble(object? value, double min, double max)
        {
            if (value is double d)
            {
                if (d < min)
                {
                    return min;
                }

                if (d > max)
                {
                    return max;
                }
            }

            return value;
        }

        public static object? ClampInt(object? value, int min, int max)
        {
            if (value is double d)
            {
                if (d < min)
                {
                    return min;
                }

                if (d > max)
                {
                    return max;
                }
            }

            return value;
        }
    }
}
