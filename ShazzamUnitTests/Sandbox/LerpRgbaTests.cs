// ReSharper disable All
#pragma warning disable SA1101, SA1300, SA1400
#pragma warning disable IDE0007, IDE0009, IDE0040, IDE1006
namespace ShazzamUnitTests.Sandbox
{
    using NUnit.Framework;
    using static Hlsl.Types;

    public class LerpRgbaTests : Hlsl
    {
        [TestCase("#FFFF0000", "#00FFFFFF", 0, "#FFFF0000")]
        [TestCase("#FFFF0000", "#00FFFFFF", 0.5f, "#7F7F3F3F")]
        [TestCase("#FFFF0000", "#00FF0000", 0.5f, "#7F7F0000")]
        [TestCase("#FFFF0000", "#00FFFFFF", 1, "#00000000")]
        [TestCase("#FFFF0000", "#FFFFFFFF", 0, "#FFFF0000")]
        [TestCase("#FFFF0000", "#FFFFFFFF", 1, "#FFFFFFFF")]
        public void LerpRgba(string x, string y, float s, string expected)
        {
            Assert.AreEqual(expected, lerp_rgba(Parse(x), Parse(y), s));
        }

        private static float4 Parse(string text) => Types.float4.Parse(text);

        float4 lerp_rgba(float4 x, float4 y, float s)
        {
            float a = lerp(x.a, y.a, s);
            float3 rgb = lerp(x.rgb, y.rgb, s) * a;
            return float4(rgb.r, rgb.g, rgb.b, a);
        }
    }
}
