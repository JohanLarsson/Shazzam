
sampler2D input : register(s0);

/// <summary>The x Value to change.</summary>
/// <minValue>0</minValue>
/// <maxValue>1</maxValue>
/// <defaultValue>1</defaultValue>
float xValue : register(C0);

float SampleI : register(C0);

float4 main(float2 uv : TEXCOORD) : COLOR
{

	float temp = uv.x = 1-uv.x;  // flip on vertical axis
	// float temp = uv.y = 1-uv.y;  // flip on horizontal axis

	float4 Color = tex2D( input , uv.xy);

	return Color;
}
