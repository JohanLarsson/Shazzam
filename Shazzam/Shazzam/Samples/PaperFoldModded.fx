
sampler2D input : register(s0); 


// <summary>The Left </summary>
/// <minValue>0</minValue>
/// <maxValue>.5</maxValue>
/// <defaultValue>0.2</defaultValue>
 float left : register(c0);
 /// <summary>The Left </summary>
/// <minValue>0</minValue>
/// <maxValue>1</maxValue>
/// <defaultValue>0.7</defaultValue>
 float edge : register(c1);
 float4 transform(float2 uv : TEXCOORD) : COLOR 
 { 
		float right =.5 - left; 
	 // transforming the curent point (uv) according to the new boundaries. 
		float2 transformedUv = float2((uv.x - left) / (right - left), uv.y);
 
	 float tx = transformedUv.x; 
	// if (tx > edge) 
	// { 
	///	tx = 1 - tx; 
	 //}
	 float top = left * tx ; 
	 float bottom = 1 - top;         
	 if (uv.y >= top && uv.y <= bottom) 
	 { 
     //linear interpolation between 0 and 1 considering the angle of folding.  
		 float ty = lerp(0, 1, (transformedUv.y - top  ) / (bottom - top)+.1); 
		// get the pixel from the transformed x and interpolated y. 
	   return tex2D(input, float2(transformedUv.x  , ty)); 
 } 
	return 0; 
 } 
 
 float4 main(float2 uv : TEXCOORD) : COLOR  
 {          
	 float right = 1 - left; 
	 if(uv.x > left && uv.x < right) 
	 { 
		return transform(uv); 
	 } 
 
	 return 0; 
 }