/// <class>BloomEffect</class>

/// <description>An effect that intensifies bright regions.</description>

//-----------------------------------------------------------------------------------------
// Shader constant register mappings (scalars - float, double, Point, Color, Point3D, etc.)
//-----------------------------------------------------------------------------------------

/// <summary>Intensity of the bloom image.</summary>
/// <minValue>0</minValue>
/// <maxValue>1</maxValue>
/// <defaultValue>1</defaultValue>
float BloomIntensity : register(C0);

/// <summary>Intensity of the base image.</summary>
/// <minValue>0</minValue>
/// <maxValue>1</maxValue>
/// <defaultValue>0.5</defaultValue>
float BaseIntensity : register(C1);

/// <summary>Saturation of the bloom image.</summary>
/// <minValue>0</minValue>
/// <maxValue>1</maxValue>
/// <defaultValue>1</defaultValue>
float BloomSaturation : register(C2);

/// <summary>Saturation of the base image.</summary>
/// <minValue>0</minValue>
/// <maxValue>1</maxValue>
/// <defaultValue>0.5</defaultValue>
float BaseSaturation : register(C3);

//--------------------------------------------------------------------------------------
// Sampler Inputs (Brushes, including Texture1)
//--------------------------------------------------------------------------------------

sampler2D Texture1Sampler : register(S0);


//--------------------------------------------------------------------------------------
// Pixel Shader
//--------------------------------------------------------------------------------------
float3 AdjustSaturation(float3 color, float saturation)
{
    float grey = dot(color, float3(0.3, 0.59, 0.11));
    return lerp(grey, color.rgb, saturation);
}

float4 main(float2 uv : TEXCOORD) : COLOR
{
    float BloomThreshold = 0.25f;

    float4 color = tex2D(Texture1Sampler, uv);
    float3 base = color.rgb / color.a;
    float3 bloom = saturate((base - BloomThreshold) / (1 - BloomThreshold));
    
    // Adjust color saturation and intensity.
    bloom = AdjustSaturation(bloom, BloomSaturation) * BloomIntensity;
    base = AdjustSaturation(base, BaseSaturation) * BaseIntensity;
    
    // Darken down the base image in areas where there is a lot of bloom,
    // to prevent things looking excessively burned-out.
    base *= (1 - saturate(bloom));
    
    // Combine the two images.
    return float4((base + bloom) * color.a, color.a);
}


