/// <class>BandedSwirlTransitionEffect</class>

/// <description>An transition effect </description>

/// <summary>The amount(%) of the transition from first texture to the second texture. </summary>
/// <minValue>0</minValue>
/// <maxValue>100</maxValue>
/// <defaultValue>30</defaultValue>
float Progress : register(C0);

/// <summary>The amount of twist for the Swirl. </summary>
/// <minValue>0</minValue>
/// <maxValue>10</maxValue>
/// <defaultValue>1</defaultValue>
float TwistAmount : register(C1);

/// <summary>The amount of twist for the Swirl. </summary>
/// <minValue>0</minValue>
/// <maxValue>100</maxValue>
/// <defaultValue>20</defaultValue>
float frequency : register(C2);
sampler2D Texture1 : register(s0);
sampler2D Texture2 : register(s1);

float4 SampleWithBorder(float4 border, sampler2D tex, float2 uv)
{
	if (any(saturate(uv) - uv))
	{
		return border;
	}
	else
	{
		return tex2D(tex, uv);
	}
}

float4 BandedSwirl(float2 uv)
{
   // normalize
   // ================================
   float2 centerNormalized ;
   centerNormalized.x = Center.x/100;
   centerNormalized.y = Center.y/100;
   // ================================

//	float2 center = float2(0.5,0.5);
	float2 toUV = uv - centerNormalized;
	float distanceFromCenter = length(toUV);
	float2 normToUV = toUV / distanceFromCenter;
	float angle = atan2(normToUV.y, normToUV.x);

	angle += sin(distanceFromCenter * frequency) * (TwistAmount /100) * Progress;
	float2 newUV;
	sincos(angle, newUV.y, newUV.x);
	newUV = newUV * distanceFromCenter + centerNormalized;

	float4 c1 = tex2D(Texture2, frac(newUV));
  float4 c2 = tex2D(Texture1, uv);

    return lerp(c1,c2, Progress/100);
}

//--------------------------------------------------------------------------------------
// Pixel Shader
//--------------------------------------------------------------------------------------
float4 main(float2 uv : TEXCOORD) : COLOR
{
	return BandedSwirl(uv);
}
