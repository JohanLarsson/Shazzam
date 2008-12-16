//--------------------------------------------------------------------------------------
// 
// WPF ShaderEffect HLSL -- GloomEffect
//
//--------------------------------------------------------------------------------------

//-----------------------------------------------------------------------------------------
// Shader constant register mappings (scalars - float, double, Point, Color, Point3D, etc.)
//-----------------------------------------------------------------------------------------

float GloomIntensity : register(C0);
float BaseIntensity : register(C1);
float GloomSaturation : register(C2);
float BaseSaturation : register(C3);

//--------------------------------------------------------------------------------------
// Sampler Inputs (Brushes, including ImplicitInput)
//--------------------------------------------------------------------------------------

sampler2D implicitInputSampler : register(S0);


//--------------------------------------------------------------------------------------
// Pixel Shader
//--------------------------------------------------------------------------------------

float4 AdjustSaturation(float4 color, float saturation)
{
    float grey = dot(color, float3(0.3, 0.59, 0.11));
    return lerp(grey, color, saturation);
}

float4 main(float2 uv : TEXCOORD) : COLOR
{
    float GloomThreshold = 0.25f;

    float4 base = 1.0 - tex2D(implicitInputSampler, uv);
    float4 gloom = saturate((base - GloomThreshold) / (1 - GloomThreshold));
    
    
    // Adjust color saturation and intensity.
    gloom = AdjustSaturation(gloom, GloomSaturation) * GloomIntensity;
    base = AdjustSaturation(base, BaseSaturation) * BaseIntensity;
    
    // Darken down the base image in areas where there is a lot of bloom,
    // to prevent things looking excessively burned-out.
    base *= (1 - saturate(gloom));
    
    // Combine the two images.
    return 1.0 - (base + gloom);
}


