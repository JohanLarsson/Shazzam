/// <class>Fade</class>
/// <description>Fade to a colour by animating the strength.</description>

sampler2D inputSampler : register(S0);

/// <summary>The color used to tint the input.</summary>
/// <minValue>0</minValue>
/// <maxValue>1</maxValue>
/// <defaultValue>0</defaultValue>
float Strength : register(C0);

/// <summary>The colour to fade to.</summary>
/// <defaultValue>Black</defaultValue>
float4 To : register(C2);

float4 lerp_rgba(float4 x, float4 y, float s)
{
    float a = lerp(x.a, y.a, s);
    float3 rgb = lerp(x.rgb, y.rgb, s) * a;
    return float4(rgb.r, rgb.g, rgb.b, a);
}

float4 main(float2 uv : TEXCOORD) : COLOR
{
    float4 src = tex2D(inputSampler, uv);
    return lerp_rgba(src, To, Strength);
}
