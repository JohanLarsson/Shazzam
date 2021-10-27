// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeThisQualifier
// ReSharper disable ArrangeTypeMemberModifiers
#pragma warning disable SA1101 // Prefix local calls with this
#pragma warning disable SA1300 // Element must begin with upper-case letter
#pragma warning disable IDE1006 // Naming Styles
#pragma warning disable SA1400 // Access modifier must be declared
namespace ShazzamUnitTests.Sandbox
{
    using NUnit.Framework;

    public class InterpolateTests : Hlsl
    {
        [TestCase(0, 1, -1, 0)]
        [TestCase(0, 1, 0, 0)]
        [TestCase(0, 1, 0.5f, 0.5f)]
        [TestCase(0, 1, 1, 1f)]
        [TestCase(0, 1, 2, 1f)]
        [TestCase(1, 0, -1, 0)]
        [TestCase(1, 0, 0, 0)]
        [TestCase(1, 0, 0.5f, 0.5f)]
        [TestCase(1, 0, 1, 1f)]
        [TestCase(1, 0, 2, 1f)]
        public void Test(float min, float max, float value, float expected)
        {
            Assert.AreEqual(expected, interpolate(min, max, value), 1E-3);
        }

        float interpolate(float min, float max, float value)
        {
            if (abs(min - max) < 0.001)
            {
                return 0.5f;
            }

            if (min < max)
            {
                return clamp((value - min) / (max - min), 0, 1);
            }

            return clamp((value - max) / (min - max), 0, 1);
        }
    }
}
