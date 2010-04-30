/// These are Shazzam elements, they are not part of the HLSL specification
/// <description>Change Postionof Pixel</description>
/// <class>ChangePosition2</class>
/// =============================================================================

sampler2D ourImage : register(s0);

float4 main(float2 locationInSource : TEXCOORD) : COLOR
{
	// create a variable to hold our color
  float4 color;
  // get the color of the current pixel
  

  // float2 has two cooridinate values that we can change
  // locationInSource.x, locationInSource.y, 
  locationInSource.x = locationInSource.x * 1.2 ;
  locationInSource.y = locationInSource.y *.9;

  // then apply a sine value

  color = tex2D( ourImage , locationInSource.xy);
  // makes the image smaller
  

  
  // What to try in this tutorial 
  // 01:   color = tex2D( ourImage , cosh(locationInSource.xy));

 
 

		return color ;

}
