/// <class>MonochromeEffect</class>

/// <description>An effect that turns the input into shades of a single color.</description>

//-----------------------------------------------------------------------------------------
// Shader constant register mappings (scalars - float, double, Point, Color, Point3D, etc.)
//-----------------------------------------------------------------------------------------

/// <summary>The color used to tint the input.</summary>
/// <defaultValue>Yellow</defaultValue>
float4 FilterColor : register(C0);

//--------------------------------------------------------------------------------------
// Sampler Inputs (Brushes, including Texture1)
//--------------------------------------------------------------------------------------

sampler2D  Texture1Sampler : register(S0);

//--------------------------------------------------------------------------------------
// Pixel Shader
//--------------------------------------------------------------------------------------

float4 main(float2 uv : TEXCOORD) : COLOR
{
   float4 srcColor = tex2D(Texture1Sampler, uv);
   float3 rgb = srcColor.rgb;
   float3 luminance = dot(rgb, float3(0.30, 0.59, 0.11));
   return float4(luminance * FilterColor.rgb, srcColor.a);
}


