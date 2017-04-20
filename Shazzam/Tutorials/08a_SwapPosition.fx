
sampler2D input : register(s0);


float SampleI : register(C0);

float4 main(float2 uv : TEXCOORD) : COLOR 
{ 
	
	float temp = uv.x;
	uv.x= uv.y;
	uv.y= temp;
	float4 Color = tex2D( input , uv.xy);


	return Color; 
}
