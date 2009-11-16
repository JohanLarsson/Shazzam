/// <class>Granite Sketch</class>

/// <description>An paper sketch effect.</description>

// Created by  Ali Daneshmandi 
// http://daneshmandi.spaces.live.com/
// http://vimeo.com/user2530151


/// <summary>The brush size of the sketch effect.</summary>
/// <minValue>0.0006</minValue>
/// <maxValue>0.01</maxValue>
/// <defaultValue>0.003</defaultValue>
float brushSize : register(C0);

sampler Image : register(s0);
float4 main(float2 texCoord: TEXCOORD0,uniform float scale,uniform float pixelSize) : COLOR
{
    float4 color = tex2D( Image, texCoord );  
	float2 samples[4] = {0, -1,	-1, 0, 1, 0, 0, 1 };
	float4 laplace = -4 * color;

	for (int i = 0; i < 4; i++)
	{
 		laplace += tex2D(Image, texCoord + brushSize * samples[i]);
 		laplace.r=laplace.rgb;
		laplace.g=laplace.rgb;
		laplace.b=laplace.rgb;
	}
	
	laplace =(1/ laplace);
	float4 complement;
    complement.rgb=1-laplace.rgb;
    complement.a = color.a;
  	if(complement.r>1)
	{
		float gray = complement.r * 0.3 + complement.g * 0.59 + complement.b *0.11;     
 		complement.r = gray;
 		complement.g = gray;  
 		complement.b = gray;
		return complement;
	}
	else
	{
		float gray = color.r * 0.3 + color.g * 0.59 + color.b *0.11;     
 		color.r =  gray;
 		color.g =  gray;  
 		color.b = gray;
		return color;
	}
}




