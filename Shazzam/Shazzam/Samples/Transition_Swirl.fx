/// <class>SwirlTransitionEffect</class>

/// <description>An transition effect </description>
/// <summary>The amount(%) of the transition from first texture to the second texture. </summary>
/// <minValue>0</minValue>
/// <maxValue>100</maxValue>
/// <defaultValue>30</defaultValue>
float Progress : register(C0);

float2 twistAmount : register(C1);
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

float4 Swirl(float2 uv,float progress)
{
	float2 center = float2(0.5,0.5);
	float2 toUV = uv - center;
	float distanceFromCenter = length(toUV);
	float2 normToUV = toUV / distanceFromCenter;
	float angle = atan2(normToUV.y, normToUV.x);
	
	angle += distanceFromCenter * distanceFromCenter * twistAmount * progress;
	float2 newUV;
	sincos(angle, newUV.y, newUV.x);
	newUV *= distanceFromCenter;
	newUV += center;
	
	float4 c1 = SampleWithBorder(float4(0,0,0,0), Texture2, newUV);
    float4 c2 = tex2D(Texture1, uv);

    return lerp(c1,c2, progress);
}

//--------------------------------------------------------------------------------------
// Pixel Shader
//--------------------------------------------------------------------------------------
float4 main(float2 uv : TEXCOORD) : COlOR
{
	return Swirl(uv, Progress/100);
}

