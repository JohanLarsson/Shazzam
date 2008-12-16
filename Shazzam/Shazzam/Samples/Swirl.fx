//--------------------------------------------------------------------------------------
// 
// WPF ShaderEffect HLSL -- SwirlEffect
//
//--------------------------------------------------------------------------------------

//-----------------------------------------------------------------------------------------
// Shader constant register mappings (scalars - float, double, Point, Color, Point3D, etc.)
//-----------------------------------------------------------------------------------------

float2 center : register(C0);
float spiralStrength : register(C1);
float2 angleFrequency : register(C2);

//--------------------------------------------------------------------------------------
// Sampler Inputs (Brushes, including ImplicitInput)
//--------------------------------------------------------------------------------------

sampler2D implicitInputSampler : register(S0);

//--------------------------------------------------------------------------------------
// Pixel Shader
//--------------------------------------------------------------------------------------

float4 main(float2 uv : TEXCOORD) : COLOR
{
   float2 dir = uv - center;
   float l = length(dir);
   float angle = atan2(dir.y, dir.x);
   
   float newAng = angle + spiralStrength * l;
   float xAmt = cos(angleFrequency.x * newAng) * l;
   float yAmt = sin(angleFrequency.y * newAng) * l;
   
   float2 newCoord = center + float2(xAmt, yAmt);
   
   return tex2D( implicitInputSampler, newCoord );
}


