float4 lerp_rgba(float4 x, float4 y, float s)
{
    float a = lerp(x.a, y.a, s);
    float3 rgb = lerp(x.rgb, y.rgb, s) * a;
    return float4(rgb.r, rgb.g, rgb.b, a);
}

float interpolate(float min, float max, float value)
{
    if (min == max)
    {
        return 0.5;
    }

    if (min < max)
    {
        return clamp((value - min) / (max - min), 0, 1);
    }

    return clamp((value - max) / (min - max), 0, 1);
}
