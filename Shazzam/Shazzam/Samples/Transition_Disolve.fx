/// <class>DisolveTransitionEffect</class>

/// <description>An transition effect </description>
/// <summary>The amount(%) of the transition from first texture to the second texture. </summary>
/// <minValue>0</minValue>
/// <maxValue>100</maxValue>
/// <defaultValue>30</defaultValue>
float Progress : register(C0);

float randomSeed : register(C1);
sampler2D Texture1 : register(s0);
sampler2D Texture2 : register(s1);
sampler2D noiseInput : register(s2);

struct VS_OUTPUT
{
    float4 Position  : POSITION;
    float4 Color     : COlOR;
    float2 UV        : TEXCOORD;
};

float4 Disolve(float2 uv, float progress)
{
	float noise = tex2D(noiseInput, frac(uv + randomSeed)).x;
	if(noise > progress)
	{
		return tex2D(Texture2, uv);
    }
    else
    {
		return tex2D(Texture1, uv);
	}
}

//--------------------------------------------------------------------------------------
// Pixel Shader
//--------------------------------------------------------------------------------------
float4 main(VS_OUTPUT input) : COlOR
{
	return Disolve(input.UV, Progress/100);
}

