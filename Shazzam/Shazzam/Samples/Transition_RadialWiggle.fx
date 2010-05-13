/// <class>RadialWiggleTransitionEffect</class>

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


float4 RadialWiggle(float2 uv,float progress)
{
	float2 center = float2(0.5,0.5);
	float2 toUV = uv - center;
	float distanceFromCenter = length(toUV);
	float2 normToUV = toUV / distanceFromCenter;
	float angle = (atan2(normToUV.y, normToUV.x) + 3.141592) / (2.0 * 3.141592);
	float offset1 = tex2D(TextureMap, float2(angle, frac(progress/3 + distanceFromCenter/5 + randomSeed))).x * 2.0 - 1.0;
	float offset2 = offset1 * 2.0 * min(0.3, (1-progress)) * distanceFromCenter;
	offset1 = offset1 * 2.0 * min(0.3, progress) * distanceFromCenter;
	
	float4 c1 = tex2D(Texture2, frac(center + normToUV * (distanceFromCenter + offset1))); 
    float4 c2 = tex2D(Texture1, frac(center + normToUV * (distanceFromCenter + offset2)));

	return lerp(c1, c2, progress);
}

//--------------------------------------------------------------------------------------
// Pixel Shader
//--------------------------------------------------------------------------------------
float4 main(VS_OUTPUT input) : COlOR
{
	return RadialWiggle(input.UV,Progress/100);
}

