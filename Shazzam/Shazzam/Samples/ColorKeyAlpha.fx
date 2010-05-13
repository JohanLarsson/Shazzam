/// <class>ColorKeyAlphaEffect</class>

/// <description>An effect that makes pixels of a particular color transparent.</description>

//-----------------------------------------------------------------------------------------
// Shader constant register mappings (scalars - float, double, Point, Color, Point3D, etc.)
//-----------------------------------------------------------------------------------------

/// <summary>The color that becomes transparent.</summary>
/// <defaultValue>Green</defaultValue>
float4 ColorKey : register(C0);

/// <summary>The tolerance in color differences.</summary>
/// <minValue>0</minValue>
/// <maxValue>1</maxValue>
/// <defaultValue>0.3</defaultValue>
float Tolerance : register(C1);

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
   
   if (all(abs(color.rgb - ColorKey.rgb) < Tolerance)) {
      color.rgba = 0;
   }
   
   return color;
}
