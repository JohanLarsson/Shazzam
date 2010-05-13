/// <class>MagnifyEffect</class>

/// <description>An effect that magnifies a circular region.</description>

//-----------------------------------------------------------------------------------------
// Shader constant register mappings (scalars - float, double, Point, Color, Point3D, etc.)
//-----------------------------------------------------------------------------------------

/// <summary>The center point of the magnified region.</summary>
/// <minValue>0,0</minValue>
/// <maxValue>1,1</maxValue>
/// <defaultValue>0.5,0.5</defaultValue>
float2 Center : register(C0);

/// <summary>The radius of the magnified region.</summary>
/// <minValue>0</minValue>
/// <maxValue>1</maxValue>
/// <defaultValue>0.25</defaultValue>
float Radius : register(C1);

/// <summary>The magnification factor.</summary>
/// <minValue>1</minValue>
/// <maxValue>5</maxValue>
/// <defaultValue>2</defaultValue>
float Magnification : register(C2);

/// <summary>The aspect ratio (width / height) of the input.</summary>
/// <minValue>0.5</minValue>
/// <maxValue>2</maxValue>
/// <defaultValue>1.5</defaultValue>
float AspectRatio : register(C4);

//--------------------------------------------------------------------------------------
// Sampler Inputs (Brushes, including Texture1)
//--------------------------------------------------------------------------------------

sampler2D  Texture1Sampler : register(S0);

//--------------------------------------------------------------------------------------
// Pixel Shader
//--------------------------------------------------------------------------------------

float4 main(float2 uv : TEXCOORD) : COLOR
{
	float2 centerToPixel = uv - Center;
	float dist = length(centerToPixel / float2(1, AspectRatio));
	float2 samplePoint = uv;
	if (dist < Radius) {
		samplePoint = Center + centerToPixel / Magnification;
	}
	return tex2D(Texture1Sampler, samplePoint);
}

