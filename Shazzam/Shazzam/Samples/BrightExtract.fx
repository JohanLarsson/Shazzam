//--------------------------------------------------------------------------------------
// 
// WPF ShaderEffect HLSL -- BrightExtractEffect
//
//--------------------------------------------------------------------------------------

//-----------------------------------------------------------------------------------------
// Shader constant register mappings (scalars - float, double, Point, Color, Point3D, etc.)
//-----------------------------------------------------------------------------------------

float Threshold : register(C0);

//--------------------------------------------------------------------------------------
// Sampler Inputs (Brushes, including ImplicitInput)
//--------------------------------------------------------------------------------------

sampler2D implicitInputSampler : register(S0);


//--------------------------------------------------------------------------------------
// Pixel Shader
//--------------------------------------------------------------------------------------

float4 main(float2 uv : TEXCOORD) : COLOR
{
    // Look up the original image color.
    float4 c = tex2D(implicitInputSampler, uv);

    // Adjust it to keep only values brighter than the specified threshold.
    return saturate((c - Threshold) / (1 - Threshold));
}


