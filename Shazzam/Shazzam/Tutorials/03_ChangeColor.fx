/// These are Shazzam elements, they are not part of the HLSL specification
/// <description>Change Color of Pixel</description>
/// <class>ChangeColor1</class>
/// =============================================================================

sampler2D ourImage : register(s0);

float4 main(float2 locationInSource : TEXCOORD) : COLOR
{
	// create a variable to hold our color
  float4 color;
  // get the color of the current pixel
  color = tex2D( ourImage , locationInSource.xy);

  // color has four value that we can change
  // color.r, color.g, color.b, color.a

  // values are normalized, so 0 is no color, 1 is full color
	color.r= 1; // set the red portion to 100%

  // What to try in this tutorial 
  // 01: change color.r = 1 to color.b = 1;
  // 02: change color.r = 1 to color.r = .4 (changes red value to 40%)
  // 03: change color.r= 1 to color.a= .5 (changes the alpha value to 50%)
  
		return color;

}
