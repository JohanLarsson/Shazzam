sampler2D inputSampler : register(S0);

/// <summary>The primary color of the gradient. </summary>
/// <defaultValue>Blue</defaultValue>
float4 StartColor : register(C1);

/// <summary>The secondary color of the gradient. </summary>
/// <defaultValue>Red</defaultValue>
float4 EndColor : register(C2);

float4 lerp_rgba(float4 x, float4 y, float s)
{
    float a = lerp(x.a, y.a, s);
    float3 rgb = lerp(x.rgb, y.rgb, s) * a;
    return float4(rgb.r, rgb.g, rgb.b, a);
}

float4 main(float2 uv : TEXCOORD) : COLOR
{
    return lerp_rgba(StartColor, EndColor, uv.x);
}