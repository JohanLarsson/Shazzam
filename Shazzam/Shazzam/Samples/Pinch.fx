//--------------------------------------------------------------------------------------
// 
// WPF ShaderEffect HLSL -- PinchEffect
//
//--------------------------------------------------------------------------------------

//-----------------------------------------------------------------------------------------
// Shader constant register mappings (scalars - float, double, Point, Color, Point3D, etc.)
//-----------------------------------------------------------------------------------------

float CenterX : register(C0);
float CenterY : register(C1);
float Radius : register(C2);
float Amount : register(C3);

//--------------------------------------------------------------------------------------
// Sampler Inputs (Brushes, including ImplicitInput)
//--------------------------------------------------------------------------------------

sampler2D implicitInputSampler : register(S0);


//--------------------------------------------------------------------------------------
// Pixel Shader
//--------------------------------------------------------------------------------------

float4 main(float2 uv : TEXCOORD) : COLOR
{
    float2 center = { CenterX, CenterY };
    float2 displace = center - uv;
    float range = saturate(1 - (length(displace) / (abs(-sin(Radius * 8) * Radius) + 0.00000001F)));
    return tex2D(implicitInputSampler, uv + displace * range * Amount);
}


