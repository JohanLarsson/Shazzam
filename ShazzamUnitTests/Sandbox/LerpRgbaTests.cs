namespace ShazzamUnitTests.Sandbox
{
    using System.Windows.Media;
    using NUnit.Framework;

    internal class LerpRgbaTests : Hlsl
    {
        [Test]
        public void Test(string x, string y, float s, string expected)
        {
            Assert.Inconclusive("Finish this");
            //Assert.AreEqual(expected, lerp_rgba(Color., max, value));
        }

        //float4 lerp_rgba(float4 x, float4 y, float s)
        //{
        //    float a = lerp(x.a, y.a, s);
        //    float3 rgb = lerp(x.rgb, y.rgb, s) * a;
        //    return float4(rgb.r, rgb.g, rgb.b, a);
        //}
    }
}