/// <class>PixelateTransitionEffect</class>

/// <description>An transition effect </description>
/// <summary>The amount(%) of the transition from first texture to the second texture. </summary>
/// <minValue>0</minValue>
/// <maxValue>100</maxValue>
/// <defaultValue>30</defaultValue>
float Progress : register(C0);

sampler2D Texture1 : register(s0);
sampler2D Texture2 : register(s1);

float4 Pixelate(float2 uv ,float progress)
{
	float pixels;
	float segment_progress;
	if (progress < 0.5)
	{
		segment_progress = 1 - progress * 2;
	}
	else
	{		
		segment_progress = (progress - 0.5) * 2;

	}
    
    pixels = 5 + 1000 * segment_progress * segment_progress;
	float2 newUV = round(uv * pixels) / pixels;
	
    float4 c1 = tex2D(Texture2, newUV);
    float4 c2 = tex2D(Texture1, newUV);

	float lerp_progress = saturate((progress - 0.4) / 0.2);
	return lerp(c1,c2, lerp_progress);	
}

//--------------------------------------------------------------------------------------
// Pixel Shader
//--------------------------------------------------------------------------------------
float4 main(float2 uv : TEXCOORD) : COlOR
{
	return Pixelate(uv, Progress/100);
}

