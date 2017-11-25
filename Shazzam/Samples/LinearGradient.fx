sampler2D inputSampler : register(S0);

/// <summary>The primary color of the gradient. </summary>
/// <defaultValue>Blue</defaultValue>
float4 StartColor : register(C1);

/// <summary>The secondary color of the gradient. </summary>
/// <defaultValue>Red</defaultValue>
float4 EndColor : register(C2);

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

    return interpolate(max, min, value);
}

float4 main(float2 uv : TEXCOORD) : COLOR
{
    float f = interpolate(-1, 1, uv.x);
    float a = lerp(StartColor.a, EndColor.a, f);
    float3 rgb = lerp(StartColor.rgb, EndColor.rgb, f) * a;
    return float4(rgb.r, rgb.g, rgb.b, a);
}