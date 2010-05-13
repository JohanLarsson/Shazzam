/// <class>BlindsTransitionEffect</class>
/// <description>A transition effect </description>
/// 

/// <summary>The amount(%) of the transition from first texture to the second texture. </summary>
/// <minValue>0</minValue>
/// <maxValue>100</maxValue>
/// <defaultValue>30</defaultValue>
float Progress : register(C0);

/// <summary>The number of Blinds strips </summary>
/// <minValue>2</minValue>
/// <maxValue>15</maxValue>
/// <defaultValue>5</defaultValue>
float NumberOfBlinds : register(C1);
sampler2D Texture1 : register(s0);
sampler2D Texture2 : register(s1);

struct VS_OUTPUT

{
    float4 Position  : POSITION;
    float4 Color     : COlOR;
    float2 UV        : TEXCOORD;
};

float4 Blinds(float2 uv)
{		
	if(frac(uv.y * NumberOfBlinds) < Progress/100)
	{
		return tex2D(Texture1, uv);
	}
	else
	{
		return tex2D(Texture2, uv);
	}
}

//--------------------------------------------------------------------------------------
// Pixel Shader
//--------------------------------------------------------------------------------------
float4 main(VS_OUTPUT input) : COlOR
{
	return Blinds(input.UV);
}

