/// <class>InvertColorEffect</class>

/// <description>An effect that inverts all colors.</description>

//--------------------------------------------------------------------------------------
// Sampler Inputs (Brushes, including Texture1)
//--------------------------------------------------------------------------------------

sampler2D Texture1Sampler : register(S0);

//--------------------------------------------------------------------------------------
// Pixel Shader
//--------------------------------------------------------------------------------------

float4 main(float2 uv : TEXCOORD) : COLOR
{
   float4 color = tex2D( Texture1Sampler, uv );
   float4 invertedColor = float4(color.a - color.rgb, color.a);
   return invertedColor;
}
