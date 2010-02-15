/// These are Shazzam elements, they are not part of the HLSL specification
/// <description>Introduction to the parts of a standard HLSL file</description>
/// <class>PartsOfAShader</class>
/// =============================================================================
 
 
sampler2D ourImage : register(s0);


// this is the entry point for the shader
// the return type is float4
// float4 contains four float values which we 
// can think of as color of a pixel
// (alpha, red, green, blue)
float4 main(float2 locationInSource : TEXCOORD) : COLOR 
{ 
	
	// tex2D is a HLSL function
  // 1st arg is a bitmap (called a texture in HLSL)
  // ourImage: the incoming image, passed in from the GPU register (s0) 
  // 
  // 2nd arg is a locator for the pixel,  this is normalized to range 0..1 

  // what is this doing?
  // tex2D takes our sample input, gets a pixel at the current x,y location
  // and returns the color of the existing pixel, which means that the color is not altered
	return tex2D( ourImage , locationInSource.xy); 


}
