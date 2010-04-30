/// These are Shazzam elements, they are not part of the HLSL specification
/// <description>Change Postionof Pixel</description>
/// <class>ChangePosition1</class>
/// =============================================================================

sampler2D ourImage : register(s0);

float4 main(float2 locationInSource : TEXCOORD) : COLOR
{
	// create a variable to hold our color
  float4 color;
  // get the color of the current pixel
  

  // float2 has two cooridinate values that we can change
  // locationInSource.x, locationInSource.y, 
  
  color = tex2D( ourImage , locationInSource.xy *2);
  // makes the image smaller
  

  
  // What to try in this tutorial 
  // 01: locationInSource.xy *.5
  // 02: change 
// color.r = 1 to color.r = .4 (changes red value to 40%)
  // 03: change color.r= 1 to color.a= .5 (changes the alpha value to 50%)

		return color ;

}
