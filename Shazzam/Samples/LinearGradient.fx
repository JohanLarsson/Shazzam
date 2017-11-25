sampler2D inputSampler : register(S0);

/// <summary>The primary color of the gradient. </summary>
/// <defaultValue>Blue</defaultValue>
float4 StartColor : register(C1);

/// <summary>The secondary color of the gradient. </summary>
/// <defaultValue>Red</defaultValue>
float4 EndColor : register(C2);

float4 main(float2 uv : TEXCOORD) : COLOR
{
    float a = lerp(StartColor.a, EndColor.a, uv.x);
    float3 rgb = lerp(StartColor.rgb, EndColor.rgb, a) * a;
    return float4(rgb.r, rgb.g, rgb.b, a);
}