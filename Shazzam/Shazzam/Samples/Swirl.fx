/// <class>SwirlEffect</class>

/// <description>An effect that swirls the input in a spiral.</description>

//-----------------------------------------------------------------------------------------
// Shader constant register mappings (scalars - float, double, Point, Color, Point3D, etc.)
//-----------------------------------------------------------------------------------------

/// <summary>The center of the spiral.</summary>
/// <minValue>0,0</minValue>
/// <maxValue>1,1</maxValue>
/// <defaultValue>0.5,0.5</defaultValue>
float2 Center : register(C0);

/// <summary>The amount of twist to the spiral.</summary>
/// <minValue>0</minValue>
/// <maxValue>20</maxValue>
/// <defaultValue>10</defaultValue>
float SpiralStrength : register(C1);

/// <summary>The aspect ratio (width / height) of the input.</summary>
/// <minValue>0.5</minValue>
/// <maxValue>2</maxValue>
/// <defaultValue>1.5</defaultValue>
float AspectRatio : register(C2);

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

	float newAngle = angle + SpiralStrength * dist;
	float2 newDir;
	sincos(newAngle, newDir.y, newDir.x);
	newDir.y *= AspectRatio;
	
	float2 samplePoint = Center + newDir * dist;
	bool isValid = all(samplePoint >= 0 && samplePoint <= 1);
	return isValid ? tex2D(implicitInputSampler, samplePoint) : float4(0, 0, 0, 0);
}


