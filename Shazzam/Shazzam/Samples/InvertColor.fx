//--------------------------------------------------------------------------------------
// 
// WPF ShaderEffect HLSL -- InvertColorEffect
//
//--------------------------------------------------------------------------------------

//--------------------------------------------------------------------------------------
// Sampler Inputs (Brushes, including ImplicitInput)
//--------------------------------------------------------------------------------------

sampler2D implicitInputSampler : register(S0);

//--------------------------------------------------------------------------------------
// Pixel Shader
//--------------------------------------------------------------------------------------

float4 main(float2 uv : TEXCOORD) : COLOR
{
   float4 color = tex2D( implicitInputSampler, uv );
   float4 inverted_color = 1 - color;
   inverted_color.a = color.a;
   inverted_color.rgb *= inverted_color.a;
   return inverted_color;
}
