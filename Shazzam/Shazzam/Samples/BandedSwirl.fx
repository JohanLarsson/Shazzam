/// <class>BandedSwirlEffect</class>
/// <namespace>Shazzam.Shaders</namespace>
/// <description>An effect that swirls the input in alternating clockwise and counterclockwise bands.</description>

//-----------------------------------------------------------------------------------------
// Shader constant register mappings (scalars - float, double, Point, Color, Point3D, etc.)
//-----------------------------------------------------------------------------------------

/// <summary>The center of the swirl.</summary>
/// <minValue>0,0</minValue>
/// <maxValue>1,1</maxValue>
/// <defaultValue>0.5,0.5</defaultValue>
float2 Center : register(C0);

/// <summary>The number of bands.</summary>
/// <minValue>0</minValue>
/// <maxValue>20</maxValue>
/// <defaultValue>10</defaultValue>
float Bands : register(C1);

/// <summary>The strength of the effect.</summary>
/// <minValue>0</minValue>
/// <maxValue>1</maxValue>
/// <defaultValue>0.5</defaultValue>
float Strength : register(C2);

/// <summary>The aspect ratio (width / height) of the input.</summary>
/// <minValue>0.5</minValue>
/// <maxValue>2</maxValue>
/// <defaultValue>1.5</defaultValue>
float AspectRatio : register(C3);

//--------------------------------------------------------------------------------------
// Sampler Inputs (Brushes, including ImplicitInput)
//--------------------------------------------------------------------------------------

sampler2D implicitInputSampler : register(S0);

//--------------------------------------------------------------------------------------
// Pixel Shader
//--------------------------------------------------------------------------------------

float4 main(float2 uv : TEXCOORD) : COLOR
{
	float2 dir = uv - Center;
	dir.y /= AspectRatio;
	float dist = length(dir);
	float angle = atan2(dir.y, dir.x);

	float remainder = frac(dist * Bands);
	float fac;   
	if (remainder < 0.25)
	{
		fac = 1.0;
	}
	else if (remainder < 0.5)
	{
		// transition zone - go smoothly from previous zone to next.
		fac = 1 - 8 * (remainder - 0.25);
	}
	else if (remainder < 0.75)
	{
		fac = -1.0;
	}
	else
	{
		// transition zone - go smoothly from previous zone to next.
		fac = -(1 - 8 * (remainder - 0.75));
	}

	float newAngle = angle + fac * Strength * dist;
	float2 newDir;
	sincos(newAngle, newDir.y, newDir.x);
	newDir.y *= AspectRatio;

	float2 samplePoint = Center + dist * newDir;
	return tex2D(implicitInputSampler, samplePoint);
}
