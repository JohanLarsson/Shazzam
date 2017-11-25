// ReSharper disable InconsistentNaming
#pragma warning disable SA1101 // Prefix local calls with this
#pragma warning disable SA1300 // Element must begin with upper-case letter
#pragma warning disable IDE1006 // Naming Styles
#pragma warning disable SA1400 // Access modifier must be declared
namespace ShazzamUnitTests.Sandbox
{
    using System;

    internal abstract class Hlsl
    {
        protected float clamp(float value, float min, float max)
        {
            if (value <= min)
            {
                return min;
            }

            if (value >= max)
            {
                return max;
            }

            return value;
        }

        protected float abs(float f)
        {
            return Math.Abs(f);
        }

        protected double abs(double f)
        {
            return Math.Abs(f);
        }
    }
}