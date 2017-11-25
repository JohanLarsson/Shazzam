// ReSharper disable InconsistentNaming
#pragma warning disable SA1101 // Prefix local calls with this
#pragma warning disable SA1300 // Element must begin with upper-case letter
#pragma warning disable IDE1006 // Naming Styles
#pragma warning disable SA1400 // Access modifier must be declared
namespace ShazzamUnitTests.Sandbox
{
    using System;
    using System.Windows.Media;
    using static Hlsl.Types;

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

        protected float lerp(float x, float y, float s) => x + (s * (y - x));

        protected float3 lerp(float3 x, float3 y, float s) => x + (s * (y - x));

        protected float3 float3(float r, float g, float b) => new float3(r, g, b);

        protected float4 float4(float r, float g, float b, float a) => new float4(r, g, b, a);

        internal class Types
        {
            public struct float3
            {
                public float3(float r, float g, float b)
                {
                    this.r = r;
                    this.g = g;
                    this.b = b;
                }

                public float r { get; }

                public float g { get; }

                public float b { get; }

                public static float3 operator +(float3 left, float3 right) => new float3(
                    left.r + right.r,
                    left.g + right.g,
                    left.b + right.b);

                public static float3 operator -(float3 left, float3 right) => new float3(
                    left.r - right.r,
                    left.g - right.g,
                    left.b - right.b);

                public static float3 operator *(float left, float3 right) => new float3(
                    left * right.r,
                    left * right.g,
                    left * right.b);

                public static float3 operator *(float3 left, float right) => right * left;
            }

            public struct float4 : IEquatable<string>
            {
                private static readonly ColorConverter ColorConverter = new ColorConverter();

                public float4(float r, float g, float b, float a)
                {
                    this.r = r;
                    this.g = g;
                    this.b = b;
                    this.a = a;
                }

                public float r { get; }

                public float g { get; }

                public float b { get; }

                public float a { get; }

                public float3 rgb => new float3(r, g, b);

                public static float4 operator +(float4 left, float4 right) => new float4(
                    left.r + right.r,
                    left.g + right.g,
                    left.b + right.b,
                    left.a + right.a);

                public static float4 operator -(float4 left, float4 right) => new float4(
                    left.r - right.r,
                    left.g - right.g,
                    left.b - right.b,
                    left.a - right.a);

                public static float4 operator *(float left, float4 right) => new float4(
                    left * right.r,
                    left * right.g,
                    left * right.b,
                    left * right.a);

                public static float4 operator *(float4 left, float right) => right * left;

                public static float4 Parse(string name)
                {
                    var color = (Color)ColorConverter.ConvertFrom(name);
                    return new float4(
                        (float)color.R / byte.MaxValue,
                        (float)color.G / byte.MaxValue,
                        (float)color.B / byte.MaxValue,
                        (float)color.A / byte.MaxValue);
                }

                public bool Equals(string other)
                {
                    var x = Parse(other);
                    return Math.Abs(this.r - x.r) < 0.001 &&
                           Math.Abs(this.g - x.g) < 0.001 &&
                           Math.Abs(this.b - x.b) < 0.001 &&
                           Math.Abs(this.a - x.a) < 0.001;

                }
            }
        }
    }
}