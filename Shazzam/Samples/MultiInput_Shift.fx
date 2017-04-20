sampler2D input : register(s0);


/// <summary>The second input texture.</summary>
/// <defaultValue>c:\examplefolder\examplefile.jpg</defaultValue> 
sampler2D Texture1 : register(s1);

/// <summary>Change the ratio between the two Textures.  0 is 100% input source, 1 is 100</summary>
/// <minValue>0/minValue>
/// <maxValue>1</maxValue>
/// <defaultValue>.5</defaultValue>
float Ratio : register(C0);
float4 main(float2 uv : TEXCOORD) : COLOR 
{ 
 float2 rev  =uv;
float temp = rev.x;
 rev.x = rev.y;
	rev.y = temp;
 float4 inputTex = tex2D(input, uv); 
 float4 otherTex = tex2D(Texture1, rev);  
 return ((inputTex*Ratio)+(otherTex*(1 - Ratio))); // mix two images
}
