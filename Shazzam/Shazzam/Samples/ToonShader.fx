/// <class>ToonShaderEffect</class>
/// <namespace>Shazzam.Shaders</namespace>
/// <description>An effect that applies cartoon-like shading (posterization).</description>

//-----------------------------------------------------------------------------------------
// Shader constant register mappings (scalars - float, double, Point, Color, Point3D, etc.)
//-----------------------------------------------------------------------------------------

/// <summary>The number of color levels to use.</summary>
/// <minValue>3</minValue>
/// <maxValue>15</maxValue>
/// <defaultValue>5</defaultValue>
float Levels : register(C0);

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
	color.rgb /= color.a;

	int levels = floor(Levels);
	color.rgb *= levels;
	color.rgb = floor(color.rgb);
	color.rgb /= levels;
	color.rgb *= color.a;
	return color;
}
