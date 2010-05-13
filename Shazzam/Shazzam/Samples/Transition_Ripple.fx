/// <class>RippleTransitionEffect</class>

/// <description>An transition effect </description>
/// <summary>The amount(%) of the transition from first texture to the second texture. </summary>
/// <minValue>0</minValue>
/// <maxValue>100</maxValue>
/// <defaultValue>30</defaultValue>
float Progress : register(C0);

sampler2D Texture1 : register(s0);
sampler2D Texture2 : register(s1);

struct VS_OUTPUT
{
    float4 Position  : POSITION;
    float4 Color     : COlOR;
    float2 UV        : TEXCOORD;
};

float4 Ripple(float2 uv,float progress)
{
	float frequency = 20;
	float speed = 10;
	float amplitude = 0.05;
	float2 center = float2(0.5,0.5);
	float2 toUV = uv - center;
	float distanceFromCenter = length(toUV);
	float2 normToUV = toUV / distanceFromCenter;

	float wave = cos(frequency * distanceFromCenter - speed * progress);
	float offset1 = progress * wave * amplitude;
	float offset2 = (1.0 - progress) * wave * amplitude;
	
	float2 newUV1 = center + normToUV * (distanceFromCenter + offset1);
	float2 newUV2 = center + normToUV * (distanceFromCenter + offset2);
	
	float4 c1 = tex2D(Texture2, newUV1); 
    float4 c2 = tex2D(Texture1, newUV2);

	return lerp(c1, c2, progress);
}


float4 main(VS_OUTPUT input) : COlOR
{
	return Ripple(input.UV, Progress/100);
}

