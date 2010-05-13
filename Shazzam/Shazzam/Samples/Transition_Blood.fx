/// <class>BloodTransitionEffect</class>

/// <description>An transition effect </description>
/// <summary>The amount(%) of the transition from first texture to the second texture. </summary>
/// <minValue>0</minValue>
/// <maxValue>100</maxValue>
/// <defaultValue>30</defaultValue>
float Progress : register(C0);


/// <summary>The seed value that determines dripiness. </summary>
/// <minValue>0</minValue>
/// <maxValue>1/maxValue>
/// <defaultValue>.3</defaultValue>
float RandomSeed : register(C1);

sampler2D Texture1 : register(s0);
sampler2D Texture2 : register(s1);
/// <summary>Another texture passed to the shader to determine drip pattern.</summary>
sampler2D CloudInput : register(s2);

struct VS_OUTPUT
{
    float4 Position  : POSITION;
    float4 Color     : COlOR;
    float2 UV        : TEXCOORD;
};

float4 Blood(float2 uv,float progress)
{
	float offset = min(progress + progress * tex2D(CloudInput, float2(uv.x, RandomSeed)).r, 1.0);
	uv.y -= offset;
	
	if(uv.y > 0.0)
	{
		return tex2D(Texture2, uv);
	}
	else
	{
		return tex2D(Texture1, frac(uv));
	}
}

//--------------------------------------------------------------------------------------
// Pixel Shader
//--------------------------------------------------------------------------------------
float4 main(VS_OUTPUT input) : COlOR
{
	return Blood(input.UV,  Progress/100);
}

