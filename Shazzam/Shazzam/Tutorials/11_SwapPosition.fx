/// These are Shazzam elements, they are not part of the HLSL specification
/// <description>Swap X,Y coordinates</description>
/// <class>SwapPosition</class>
/// =============================================================================
sampler2D input : register(s0);

// new HLSL shader

/// <summary>Explain the purpose of this variable.</summary>
/// <minValue>05/minValue>
/// <maxValue>10</maxValue>
/// <defaultValue>3.5</defaultValue>
float SampleI : register(C0);

float4 main(float2 uv : TEXCOORD) : COLOR 
{ 
	
	float temp = uv.x;
	uv.x= uv.y;
	uv.y= temp;
	float4 Color = tex2D( input , uv.xy);

	
	//Color= tex2D( input , uv.xy); 
	//Color.r=uv.x;

	return Color; 
}
