/// These are Shazzam elements, they are not part of the HLSL specification
/// <description>Swap X,Y coordinates</description>
/// <class>SwapPosition</class>
/// =============================================================================
sampler2D input : register(s0);

/// <summary>The x Value to change.</summary>
/// <minValue>0</minValue>
/// <maxValue>1</maxValue>
/// <defaultValue>1</defaultValue>
float xValue : register(C0);

float SampleI : register(C0);

float4 main(float2 uv : TEXCOORD) : COLOR 
{ 
	
	float temp = uv.x /xValue;
	float i = 0;
float ret = modf(uv.x,i);
	uv.x= ;
	//uv.y= uv.y;
	float4 Color = tex2D( input , uv.xy);


	return Color; 
}
