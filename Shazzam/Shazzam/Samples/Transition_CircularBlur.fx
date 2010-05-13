/// <class>CircularBlurTransitionEffect</class>

/// <description>An transition effect </description>
/// <summary>The amount(%) of the transition from first texture to the second texture. </summary>
/// <minValue>0</minValue>
/// <maxValue>100</maxValue>
/// <defaultValue>30</defaultValue>
float Progress : register(C0);
sampler2D Texture1 : register(s0);
sampler2D Texture2 : register(s1);
sampler2D trigInput : register(s2);

struct VS_OUTPUT
{
    float4 Position  : POSITION;
    float4 Color     : COlOR;
    float2 UV        : TEXCOORD;
};

float4 CircularBlur(float2 uv,float progress)
{
	float2 center = float2(0.5,0.5);
	float2 toUV = uv - center;
	float distanceFromCenter = length(toUV);
	float2 normToUV = toUV / distanceFromCenter;
	float angle = tex2D(trigInput, (normToUV + 1) * 0.5).z;
	
	float4 c1 = float4(0,0,0,0);
	float s = progress * 0.005;
    int count = 7;
	
	for(int i=0; i<count; i++)
	{
		float newAngle = angle - i*s;
		float2 newUV = (tex2D(trigInput, frac(newAngle - 0.5)).xy * 2.0 - 1.0) * distanceFromCenter + center;
		c1 += tex2D(Texture2, newUV);
	}
	
	c1 /= count;
    float4 c2 = tex2D(Texture1, uv);

	return lerp(c1, c2, progress);
}

//--------------------------------------------------------------------------------------
// Pixel Shader
//--------------------------------------------------------------------------------------
float4 main(VS_OUTPUT input) : COlOR
{
	return CircularBlur(input.UV, Progress/100);
}

