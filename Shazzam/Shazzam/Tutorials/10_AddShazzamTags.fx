/// These are Shazzam elements, they are not part of the HLSL specification
/// <description>Adding </description>
/// <class>ChangePosition2</class>
/// =============================================================================

sampler2D ourImage : register(s0);

// add our input values here

/// <summary>The x Value to change.</summary>
/// <minValue>-5</minValue>
/// <maxValue>5</maxValue>
/// <defaultValue>1.2</defaultValue>
float xValue : register(C0);

/// <summary>The y Value to change.</summary>
/// <minValue>-5</minValue>
/// <maxValue>5</maxValue>
/// <defaultValue>.8</defaultValue>
float yValue : register(C1);

float4 main(float2 locationInSource : TEXCOORD) : COLOR
{
	// create a variable to hold our color
  float4 color;
  // get the color of the current pixel
  
	// HLSL permits other inputs to shader
	// must declare fields to do this
	// it would be simpler to test these values
	// if we could add some test controls in Shazzam

	// use the Shazzam attributes to provide initial values
	// and default ranges
  locationInSource.x = locationInSource.x * xValue;
  locationInSource.y =locationInSource.y * yValue;

  // then apply a sine value

  color = tex2D( ourImage , locationInSource.xy);
  // makes the image smaller
  

  
  // What to try in this tutorial 
  // 01: locationInSource.xy *.5
  // 02: change 
// color.r = 1 to color.r = .4 (changes red value to 40%)
  // 03: change color.r= 1 to color.a= .5 (changes the alpha value to 50%)
 

		return color ;

}
