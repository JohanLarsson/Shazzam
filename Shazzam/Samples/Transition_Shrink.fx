/// <class>ShrinkTransitionEffect</class>

/// <description>A transition effect </description>
/// <summary>The amount(%) of the transition from first texture to the second texture. </summary>
/// <minValue>0</minValue>
/// <maxValue>100</maxValue>
/// <defaultValue>30</defaultValue>
float Progress : register(C0);

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

float4 Shrink(float2 uv,float progress)
{
	float speed = 200;
	float2 center = float2(0.5,0.5);
	float2 toUV = uv - center;
	float distanceFromCenter = length(toUV);
	float2 normToUV = toUV / distanceFromCenter;
	
	float2 newUV = center + normToUV * (distanceFromCenter * (progress * speed + 1));
	
	float4 c1 = SampleWithBorder(float4(0,0,0,0), Texture2, newUV); 

	if(c1.a <= 0)
	{
		return tex2D(Texture1, uv);
	}
	
	return c1;

}

//--------------------------------------------------------------------------------------
// Pixel Shader
//--------------------------------------------------------------------------------------
float4 main(float2 uv : TEXCOORD) : COlOR
{
	return Shrink(uv, Progress/100);
}

