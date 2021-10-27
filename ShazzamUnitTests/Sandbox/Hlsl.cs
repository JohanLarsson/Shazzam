// ReSharper disable InconsistentNaming
#pragma warning disable SA1300 // Element must begin with upper-case letter
#pragma warning disable IDE1006 // Naming Styles
#pragma warning disable SA1400 // Access modifier must be declared
namespace ShazzamUnitTests.Sandbox
{
    using System;
    using System.Globalization;
    using System.Windows.Media;
    using static Hlsl.Types;

    public abstract class Hlsl
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

        protected float abs(double f)
        {
            return (float)Math.Abs(f);
        }

        protected float degrees(float value) => (float)(value * 180 / Math.PI);

        protected float radians(float value) => (float)(value * Math.PI / 180);

        protected float atan2(float y, float x) => (float)Math.Atan2(y, x);

        protected float lerp(float x, float y, float s) => x + (s * (y - x));

        protected float3 lerp(float3 x, float3 y, float s) => x + (s * (y - x));

        protected float2 float2(float x, float y) => new(x, y);

        protected float3 float3(float r, float g, float b) => new(r, g, b);

        protected float4 float4(float r, float g, float b, float a) => new(r, g, b, a);

        public class Types
        {
            public struct float2
            {
                public float2(float x, float y)
                {
                    this.x = x;
                    this.y = y;
                }

                public float x { get; }

                public float y { get; }

                public static float2 operator +(float2 left, float2 right) => new(
                    left.x + right.x,
                    left.y + right.y);

                public static float2 operator -(float2 left, float2 right) => new(
                    left.x - right.x,
                    left.y - right.y);

                public static float2 operator *(float left, float2 right) => new(
                    left * right.x,
                    left * right.y);

                public static float2 operator *(float2 left, float right) => right * left;

                public static float2 Parse(string text)
                {
                    var parts = text.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length != 2)
                    {
                        throw new ArgumentException(nameof(text));
                    }

                    return new float2(
                        float.Parse(parts[0], CultureInfo.InvariantCulture),
                        float.Parse(parts[1], CultureInfo.InvariantCulture));
                }
            }

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

                public static float3 operator +(float3 left, float3 right) => new(
                    left.r + right.r,
                    left.g + right.g,
                    left.b + right.b);

                public static float3 operator -(float3 left, float3 right) => new(
                    left.r - right.r,
                    left.g - right.g,
                    left.b - right.b);

                public static float3 operator *(float left, float3 right) => new(
                    left * right.r,
                    left * right.g,
                    left * right.b);

                public static float3 operator *(float3 left, float right) => right * left;
            }

            public struct float4 : IEquatable<string>
            {
                private static readonly ColorConverter ColorConverter = new();

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

                public float3 rgb => new(this.r, this.g, this.b);

                public static float4 operator +(float4 left, float4 right) => new(
                    left.r + right.r,
                    left.g + right.g,
                    left.b + right.b,
                    left.a + right.a);

                public static float4 operator -(float4 left, float4 right) => new(
                    left.r - right.r,
                    left.g - right.g,
                    left.b - right.b,
                    left.a - right.a);

                public static float4 operator *(float left, float4 right) => new(
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

                public bool Equals(string other) => this.ToString() == other;

                public override string ToString() => Color.FromArgb((byte)(byte.MaxValue * this.a), (byte)(byte.MaxValue * this.r), (byte)(byte.MaxValue * this.g), (byte)(byte.MaxValue * this.b)).ToString();
            }
        }
    }
}
