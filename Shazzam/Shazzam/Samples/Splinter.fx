/// <class>Splinter</class>
/// <description>Applies a triangular splinter pattern</description>
///


/// <summary>Change the horizontal offset of each tile.</summary>
/// <minValue>0.01</minValue>
/// <maxValue>1.5</maxValue>
/// <defaultValue>1</defaultValue>
float SplinterIntensity : register(C3);


/// <summary>Change the horizontal offset of each tile.</summary>
/// <minValue>0.01</minValue>
/// <maxValue>2/maxValue>
/// <defaultValue>1</defaultValue>
float Multiplier : register(C2);


/// <summary>Change the horizontal offset of each tile.</summary>
/// <minValue>0.1</minValue>
/// <maxValue>2</maxValue>
/// <defaultValue>.5</defaultValue>
float Push : register(C4);



sampler2D implicitInputSampler : register(S0);

float4 main(float2 uv : TEXCOORD) : COLOR
{
	//float2 locator = float2(uv.x * uv.y % SplinterIntensity , uv.y /   uv.x % Multiplier) ;
	
	float2 locator = float2(((uv.x *Push) +  SplinterIntensity % uv.y) * sin (Multiplier) , sin( uv.y  % uv.x) );
	return tex2D( implicitInputSampler, locator );

}
