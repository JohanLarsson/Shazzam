//--------------------------------------------------------------------------------------
// 
// WPF ShaderEffect HLSL -- RippleEffect
//
//--------------------------------------------------------------------------------------

//-----------------------------------------------------------------------------------------
// Shader constant register mappings (scalars - float, double, Point, Color, Point3D, etc.)
//-----------------------------------------------------------------------------------------

float2 center : register(C0);
float amplitude : register(C1);
float frequency: register(C2);
float phase: register(C3);

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
   
   float2 toPixel = uv - center; // vector from center to pixel
	float distance = length(toPixel);
	float2 direction = toPixel/distance;
	float angle = atan2(direction.y, direction.x);
	float2 wave;
	sincos(frequency * distance + phase, wave.x, wave.y);
		
	float falloff = saturate(1-distance);
	falloff *= falloff;
		
	distance += amplitude * wave.x * falloff;
   sincos(angle, direction.y, direction.x);
   float2 uv2 = center + distance * direction;
   
   float lighting = saturate(wave.y * falloff) * 0.2 + 0.8;
   
   float4 color = tex2D( implicitInputSampler, uv2 );
   color.rgb *= lighting;
   
   return color;
}
