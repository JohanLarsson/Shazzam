/// <class>RippleEffect</class>

/// <description>An effect that superimposes rippling waves upon the input.</description>

//-----------------------------------------------------------------------------------------
// Shader constant register mappings (scalars - float, double, Point, Color, Point3D, etc.)
//-----------------------------------------------------------------------------------------

/// <summary>The center point of the ripples.</summary>
/// <minValue>0,0</minValue>
/// <maxValue>1,1</maxValue>
/// <defaultValue>0.5,0.5</defaultValue>
float2 Center : register(C0);

/// <summary>The amplitude of the ripples.</summary>
/// <minValue>0</minValue>
/// <maxValue>1</maxValue>
/// <defaultValue>0.1</defaultValue>
float Amplitude : register(C1);

/// <summary>The frequency of the ripples.</summary>
/// <minValue>0</minValue>
/// <maxValue>100</maxValue>
/// <defaultValue>70</defaultValue>
float Frequency: register(C2);

/// <summary>The phase of the ripples.</summary>
/// <minValue>-20</minValue>
/// <maxValue>20</maxValue>
/// <defaultValue>0</defaultValue>
float Phase: register(C3);

/// <summary>The aspect ratio (width / height) of the input.</summary>
/// <minValue>0.5</minValue>
/// <maxValue>2</maxValue>
/// <defaultValue>1.5</defaultValue>
float AspectRatio : register(C4);

//--------------------------------------------------------------------------------------
// Sampler Inputs (Brushes, including ImplicitInput)
//--------------------------------------------------------------------------------------

sampler2D implicitInputSampler : register(S0);

//--------------------------------------------------------------------------------------
// Pixel Shader
//--------------------------------------------------------------------------------------

float4 main(float2 uv : TEXCOORD) : COLOR
{
 
	float2 dir = uv - Center; // vector from center to pixel
	dir.y /= AspectRatio;
	float dist = length(dir);
	dir /= dist;
	dir.y *= AspectRatio;

	float2 wave;
	sincos(Frequency * dist + Phase, wave.x, wave.y);
		
	float falloff = saturate(1 - dist);
	falloff *= falloff;
		
	dist += Amplitude * wave.x * falloff;
	float2 samplePoint = Center + dist * dir;
	float4 color = tex2D(implicitInputSampler, samplePoint);

	float lighting = 1 - Amplitude * 0.2 * (1 - saturate(wave.y * falloff));
	color.rgb *= lighting;
	return color;
}
