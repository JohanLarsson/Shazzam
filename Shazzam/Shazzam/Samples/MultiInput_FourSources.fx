sampler2D Texture1 : register(s0);

// use samples with dark edges 
// otherwise the combined colors will wash out result

/// <summary>The second input texture.</summary>
/// <defaultValue>c:\examplefolder\examplefile.jpg</defaultValue> 
sampler2D Texture2 : register(s1);
/// <summary>The third input texture.</summary>
/// <defaultValue>c:\examplefolder\examplefile.jpg</defaultValue> 
sampler2D Texture3 : register(s2);
/// <summary>The fourth input texture.</summary>
/// <defaultValue>c:\examplefolder\examplefile.jpg</defaultValue> 
sampler2D Texture4 : register(s3);

/// <summary>The horizontal offset for this sample.  </summary>
/// <minValue>1/minValue>
/// <maxValue>5</maxValue>
/// <defaultValue>1</defaultValue>
float Offset1 : register(C0);

/// <summary>The horizontal offset for this sample.  </summary>
/// <minValue>1/minValue>
/// <maxValue>5</maxValue>
/// <defaultValue>2</defaultValue>
float Offset2 : register(C1);


/// <summary>The horizontal offset for this sample.  </summary>
/// <minValue>1/minValue>
/// <maxValue>5</maxValue>
/// <defaultValue>3</defaultValue>
float Offset3 : register(C2);

/// <summary>The horizontal offset for this sample.  </summary>
/// <minValue>1/minValue>
/// <maxValue>5</maxValue>
/// <defaultValue>4</defaultValue>
float Offset4 : register(C3);
float4 main(float2 uv : TEXCOORD) : COLOR 
{ 
  float columnCount = 4;
  float repeatInterval = 4;
  float2 uv1 = float2(( Offset1-  uv.x * columnCount % repeatInterval)  , (uv.y  )) ;
  float2 uv2 = float2(( Offset2 -( uv.x * columnCount % repeatInterval) ) , (uv.y  )) ;
  float2 uv3 = float2(( Offset3 -( uv.x * columnCount % repeatInterval) ) , (uv.y  )) ;
  float2 uv4 = float2(( Offset4 -( uv.x * columnCount % repeatInterval) ) , (uv.y  )) ;
  
  float4 texA = tex2D(Texture1, uv1);
  float4 texB = tex2D(Texture2, uv2);  
  float4 texC= tex2D(Texture3, uv3);
   float4 texD = tex2D(Texture4, uv4);
// return ((inputTex*Ratio)+(otherTex*(1 - Ratio))); // mix two images

//float2 newUv = float2((uv.x * 2 % 1)  , (uv.y * 2 % 1)) ;
	return  texA*.6  + texB*.6 + texC *.6 +texD *.6;
}
