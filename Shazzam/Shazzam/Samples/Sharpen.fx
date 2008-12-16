//--------------------------------------------------------------------------------------
// 
// WPF ShaderEffect HLSL -- SharpenEffect
//
//--------------------------------------------------------------------------------------

//-----------------------------------------------------------------------------------------
// Shader constant register mappings (scalars - float, double, Point, Color, Point3D, etc.)
//-----------------------------------------------------------------------------------------

float Amount : register(C0);
float Width : register(C1);

//--------------------------------------------------------------------------------------
// Sampler Inputs (Brushes, including ImplicitInput)
//--------------------------------------------------------------------------------------

sampler2D implicitInputSampler : register(S0);


//--------------------------------------------------------------------------------------
// Pixel Shader
//--------------------------------------------------------------------------------------

float4 main(float2 uv : TEXCOORD) : COLOR
{
    float4 color = tex2D(implicitInputSampler, uv);
    color.rgb += tex2D(implicitInputSampler, uv - Width) * Amount;
    color.rgb -= tex2D(implicitInputSampler, uv + Width) * Amount;
    return color;
}


