/// <class>FadeToBlackEffect</class>
/// <description>Fade to black by animating the strength</description>

sampler2D  inputSampler : register(S0);

//-----------------------------------------------------------------------------------------
// Shader constant register mappings (scalars - float, double, Point, Color, Point3D, etc.)
//-----------------------------------------------------------------------------------------

/// <summary>The color used to tint the input.</summary>
/// <defaultValue>Yellow</defaultValue>
float Strength : register(C0);

float4 main(float2 uv : TEXCOORD) : COLOR
{
   float4 srcColor = tex2D(inputSampler, uv);
   float3 color =(1-Strength)*srcColor.rgb;
   return float4(color, srcColor.a);
}
