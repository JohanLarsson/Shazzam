//--------------------------------------------------------------------------------------
// 
// WPF ShaderEffect HLSL -- MagnifyEffect
//
//--------------------------------------------------------------------------------------

//-----------------------------------------------------------------------------------------
// Shader constant register mappings (scalars - float, double, Point, Color, Point3D, etc.)
//-----------------------------------------------------------------------------------------

float2 radii : register(C0);
float2 center : register(C1);
float  amount : register(C2);

//--------------------------------------------------------------------------------------
// Sampler Inputs (Brushes, including ImplicitInput)
//--------------------------------------------------------------------------------------

sampler2D  implicitInputSampler : register(S0);

//--------------------------------------------------------------------------------------
// Pixel Shader
//--------------------------------------------------------------------------------------

float4 main(float2 uv : TEXCOORD) : COLOR
{
   float2 origUv = uv;
   float2 ray = origUv - center;
   float2 rt = ray / radii;

   // Outside of radii, we jus show the regular image.  Radii is ellipse radii, so width x height radius 
   float lengthRt = length(rt);
   
   float2 texuv;
   if (lengthRt > 1)
   {
       texuv = origUv;
   }
   else
   {
       texuv = center + amount * ray;
   }
   
   return tex2D(implicitInputSampler, texuv);
}

