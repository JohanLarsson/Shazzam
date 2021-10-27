// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeThisQualifier
// ReSharper disable ArrangeTypeMemberModifiers
// ReSharper disable SuggestVarOrType_BuiltInTypes
#pragma warning disable SA1101 // Prefix local calls with this
#pragma warning disable SA1300 // Element must begin with upper-case letter
#pragma warning disable IDE1006 // Naming Styles
#pragma warning disable SA1400 // Access modifier must be declared
#pragma warning disable SA1306 // Field names must begin with lower-case letter
namespace ShazzamUnitTests.Sandbox
{
    using NUnit.Framework;
    using static Hlsl.Types;

    public class AngleTests : Hlsl
    {
        static float PI2 = 6.28318548f;

        [TestCase("1 0.5", "0.5 0.5", 0, 360, 90)]
        [TestCase("1 0.5", "0.5 0.5", 0, -360, 270)]
        [TestCase("0.5 1", "0.5 0.5", 0, 360, 180)]
        [TestCase("0.5 1", "0.5 0.5", 0, -360, 180)]
        [TestCase("0.5 1", "0.5 0.5", 90, 360, 90)]
        [TestCase("0.5 1", "0.5 0.5", 90, -360, 270)]
        [TestCase("0.5 -1", "0.5 0.5", -90, 360, 90)]
        [TestCase("0.5 -1", "0.5 0.5", -90, -360, 270)]
        public void AngleFromStart(string uvs, string cps, float start, float centralAngle, float expected)
        {
            var uv = Parse(uvs);
            var cp = Parse(cps);
            var angleFromStart = angle_from_start(uv, cp, radians(start), radians(centralAngle));
            Assert.AreEqual(expected, degrees(angleFromStart), 0.001);
        }

        private static float2 Parse(string text) => Types.float2.Parse(text);

        float clamp_angle_positive(float a)
        {
            if (a < 0)
            {
                return a + PI2;
            }

            return a;
        }

        float clamp_angle_negative(float a)
        {
            if (a > 0)
            {
                return a - PI2;
            }

            return a;
        }

        float angle_from_start(float2 uv, float2 center_point, float start_angle, float central_angle)
        {
            float2 v = uv - center_point;
            return central_angle > 0
                ? clamp_angle_positive(clamp_angle_positive(atan2(v.x, -v.y)) - clamp_angle_positive(start_angle))
                : abs(clamp_angle_negative(clamp_angle_negative(atan2(v.x, -v.y)) - clamp_angle_negative(start_angle)));
        }
    }
}
