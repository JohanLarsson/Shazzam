/// <class>DropTransitionEffect</class>

/// <description>An transition effect </description>
/// <summary>The amount(%) of the transition from first texture to the second texture. </summary>
/// <minValue>0</minValue>
/// <maxValue>100</maxValue>
/// <defaultValue>30</defaultValue>
float Progress : register(C0);

float randomSeed : register(C1);
sampler2D Texture1 : register(s0);
sampler2D Texture2 : register(s1);
sampler2D TextureMap : register(s2);

struct VS_OUTPUT
{
    float4 Position  : POSITION;
    float4 Color     : COlOR;
    float2 UV        : TEXCOORD;
};

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

float4 DropFade(float2 uv, float progress)
{
	float offset = -tex2D(TextureMap, float2(uv.x / 5, randomSeed)).x;
	float4 c1 = SampleWithBorder(float4(0,0,0,0), Texture2, float2(uv.x, uv.y + offset * progress));
    float4 c2 = tex2D(Texture1, uv);

	if (c1.a <= 0.0)
		return c2;
	else
		return lerp(c1, c2, progress);
}

//--------------------------------------------------------------------------------------
// Pixel Shader
//--------------------------------------------------------------------------------------
float4 main(VS_OUTPUT input) : COlOR
{
	return DropFade(input.UV, Progress/100);
}

