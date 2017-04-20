
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

  color = tex2D( ourImage , locationInSource.xy);

    return color ;

}
