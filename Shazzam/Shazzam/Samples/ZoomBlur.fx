/// <class>ZoomBlurEffect</class>

/// <description>An effect that applies a radial blur to the input.</description>

//-----------------------------------------------------------------------------------------
// Shader constant register mappings (scalars - float, double, Point, Color, Point3D, etc.)
//-----------------------------------------------------------------------------------------

/// <summary>The center of the blur.</summary>
/// <minValue>0</minValue>
/// <maxValue>0.2</maxValue>
/// <defaultValue>0.9,0.6</defaultValue>
float2 Center : register(C0);

/// <summary>The amount of blur.</summary>
/// <minValue>0</minValue>
/// <maxValue>0.2</maxValue>
/// <defaultValue>0.1</defaultValue>
float BlurAmount : register(C1);

//--------------------------------------------------------------------------------------
// Sampler Inputs (Brushes, including ImplicitInput)
//--------------------------------------------------------------------------------------

sampler2D  inputSource : register(S0);

//--------------------------------------------------------------------------------------
// Pixel Shader
//--------------------------------------------------------------------------------------

float4 main(float2 uv : TEXCOORD) : COLOR
{
	float4 c = 0;    
	uv -= Center;

	for (int i = 0; i < 15; i++)
    {
		float scale = 1.0 + BlurAmount * (i / 14.0);
		c += tex2D(inputSource, uv * scale + Center);
	}
   
	c /= 15;
	return c;
}
