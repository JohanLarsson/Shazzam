/// <class>LeastBrightTransitionEffect</class>

/// <description>A transition effect </description>
/// <summary>The amount(%) of the transition from first texture to the second texture. </summary>
/// <minValue>0</minValue>
/// <maxValue>100</maxValue>
/// <defaultValue>30</defaultValue>
float Progress : register(C0);

sampler2D Texture1 : register(s0);
sampler2D Texture2 : register(s1);

float4 LeastBright(float2 uv,float progress)
{
	int c = 4;
	int c2 = 3;
	float oc = (c -1) / 2;
	float oc2 = (c2 -1) / 2;
	float offset = 0.01 * progress;

	float leastBright = 1;
	float4 leastBrightColor;
	for(int y=0; y<c; y++)
	{
		for(int x=0; x<c2; x++)
		{
			float2 newUV = uv + (float2(x, y) - float2(oc2, oc)) * offset;
			float4 color = tex2D(Texture2, newUV);
			float brightness = dot(color.rgb, float3(1,1.1,0.9));
			if(brightness < leastBright)
			{
				leastBright = brightness;
				leastBrightColor = color;
			}
		}
	}

	float4 impl = tex2D(Texture1, uv);

	return lerp(leastBrightColor,impl, progress);
}

//--------------------------------------------------------------------------------------
// Pixel Shader
//--------------------------------------------------------------------------------------
float4 main(float2 uv : TEXCOORD) : COlOR
{
	return LeastBright(uv, Progress/100);
}
