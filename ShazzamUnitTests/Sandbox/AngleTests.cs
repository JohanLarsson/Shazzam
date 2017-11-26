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

    internal class AngleTests : Hlsl
    {
        static float PI2 = 6.28318548f;

        [TestCase("1 0", 0)]
        [TestCase("0 1", 270)]
        [TestCase("-1 0", 180)]
        [TestCase("0 -1", 90)]
        public void ClockwiseAngleTest(string vs, float expected)
        {
            var u = Parse(vs);
            var angle = clockwise_angle(u);
            Assert.AreEqual(expected, degrees(angle), 0.001);
        }

        [TestCase(0, true, 0)]
        [TestCase(0, false, 0)]
        [TestCase(90, true, 90)]
        [TestCase(90, false, -270)]
        [TestCase(-90, true, 270)]
        [TestCase(-90, false, -90)]
        public void ClampAngleTest(float angle, bool clockwise, float expected)
        {
            var signed = clamp_angle(radians(angle), clockwise);
            Assert.AreEqual(expected, degrees(signed), 0.001);
        }

        [TestCase(0, 0, true, 0)]
        [TestCase(0, 0, false, 0)]
        [TestCase(90, 90, true, 0)]
        [TestCase(90, 90, false, 0)]
        [TestCase(90, 0, true, 90)]
        [TestCase(90, 0,false, -270)]
        [TestCase(180, 90, true, 90)]
        [TestCase(180, 90, false, -270)]
        [TestCase(-90, 0, true, 270)]
        [TestCase(-90, 0, false, -90)]
        public void AngleFromStartTests(float cw_angle, float start_angle, bool clockwise, float expected)
        {
            var angle = angle_from_start(radians(cw_angle), radians(start_angle), clockwise);
            Assert.AreEqual(expected, degrees(angle), 0.001);
        }

        [TestCase("1 0.5", "0.5 0.5", 0f, 360f, 0f)]
        [TestCase("1 0.5", "0.5 0.5", 0f, -360f, 0f)]
        [TestCase("0.5 1", "0.5 0.5", 0f, 360f, 270f)]
        [TestCase("0.5 1", "0.5 0.5", 0f, -360f, 90f)]
        [TestCase("0.5 1", "0.5 0.5", 90f, 360f, 0f)]
        [TestCase("0.5 1", "0.5 0.5", 90f, -360f, 0f)]
        [TestCase("0.5 -1", "0.5 0.5", -90f, 360f, 0f)]
        [TestCase("0.5 -1", "0.5 0.5", -90f, -360f, 0f)]
        public void AngleFromStartTest(string uvs, string cps, float start, float centralAngle, float expected)
        {
            var uv = Parse(uvs);
            var cp = Parse(cps);
            var angleFromStart = angle_from_start(uv, cp, radians(start), radians(centralAngle));
            Assert.AreEqual(expected, degrees(angleFromStart), 0.001);
        }

        private static float2 Parse(string text) => Types.float2.Parse(text);

        float clamp_angle(float angle, bool clockwise)
        {
            angle %= PI2;
            if (clockwise && angle < 0)
            {
                return angle + PI2;
            }

            if (!clockwise && angle > 0)
            {
                return angle - PI2;
            }

            return angle;
        }

        float clockwise_angle(float2 v)
        {
            float a = atan2(v.y, v.x);
            if (a < 0)
            {
                return a + PI2;
            }

            return a;
        }

        float angle_from_start(float clockwise_angle, float start_angle, bool clockwise)
        {
            return clamp_angle(clockwise_angle - start_angle, clockwise);
        }

        float angle_from_start(float2 uv, float2 center_point, float start_angle, float central_angle)
        {
            float2 v = uv - center_point;
            bool clockwise = central_angle > 0;
            return angle_from_start(
                clockwise_angle(v),
                start_angle,
                clockwise);
        }
    }
}
