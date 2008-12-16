//--------------------------------------------------------------------------------------
// 
// WPF ShaderEffect HLSL -- ToneMappingEffect
//
//--------------------------------------------------------------------------------------

//-----------------------------------------------------------------------------------------
// Shader constant register mappings (scalars - float, double, Point, Color, Point3D, etc.)
//-----------------------------------------------------------------------------------------

float Exposure : register(C0);
float Defog : register(C1);
float Gamma : register(C2);
float4 FogColor : register(C3);
float VignetteRadius : register(C4);
float2 VignetteCenter : register(C5);
float BlueShift : register(C6);

//--------------------------------------------------------------------------------------
// Sampler Inputs (Brushes, including ImplicitInput)
//--------------------------------------------------------------------------------------

sampler2D implicitInputSampler : register(S0);


//--------------------------------------------------------------------------------------
// Pixel Shader
//--------------------------------------------------------------------------------------

float4 main(float2 uv : TEXCOORD) : COLOR
{
    float4 c = tex2D(implicitInputSampler, uv);
    c.rgb = max(0, c.rgb - Defog * FogColor.rgb);
    c.rgb *= pow(2.0f, Exposure);
    c.rgb = pow(c.rgb, Gamma);

    float2 tc = uv - VignetteCenter;
    float v = 1.0f - dot(tc, tc);
    c.rgb += pow(v, 4) * VignetteRadius;

    float3 d = c.rgb * float3(1.05f, 0.97f, 1.27f);
    c.rgb = lerp(c.rgb, d, BlueShift);
    
    return c;
}


