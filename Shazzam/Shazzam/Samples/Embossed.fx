//--------------------------------------------------------------------------------------
// 
// WPF ShaderEffect HLSL -- EmbossedEffect
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
    float4 outC = {0.5, 0.5, 0.5, 1.0};

    outC -= tex2D(implicitInputSampler, uv - Width) * Amount;
    outC += tex2D(implicitInputSampler, uv + Width) * Amount;
    outC.rgb = (outC.r + outC.g + outC.b) / 3.0f;

    return outC;
}


