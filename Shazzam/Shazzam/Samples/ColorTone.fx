//--------------------------------------------------------------------------------------
// 
// WPF ShaderEffect HLSL -- ColorToneEffect
//
//--------------------------------------------------------------------------------------

//-----------------------------------------------------------------------------------------
// Shader constant register mappings (scalars - float, double, Point, Color, Point3D, etc.)
//-----------------------------------------------------------------------------------------

float Desaturation : register(C0);
float Toned : register(C1);
float4 LightColor : register(C2);
float4 DarkColor : register(C3);

//--------------------------------------------------------------------------------------
// Sampler Inputs (Brushes, including ImplicitInput)
//--------------------------------------------------------------------------------------

sampler2D implicitInputSampler : register(S0);


//--------------------------------------------------------------------------------------
// Pixel Shader
//--------------------------------------------------------------------------------------

float4 main(float2 uv : TEXCOORD) : COLOR
{
    float4 scnColor = LightColor * tex2D(implicitInputSampler, uv);
    float gray = dot(float3(0.3, 0.59, 0.11), scnColor.rgb);
    
    float3 muted = lerp(scnColor.rgb, gray.xxx, Desaturation);
    float3 middle = lerp(DarkColor, LightColor, gray);
    
    scnColor.rgb = lerp(muted, middle, Toned);
    return scnColor;
}


