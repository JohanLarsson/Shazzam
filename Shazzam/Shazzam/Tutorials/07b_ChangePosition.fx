
sampler2D ourImage : register(s0);

float4 main(float2 locationInSource : TEXCOORD) : COLOR
{
  // create a variable to hold our color
  float4 color;
  // get the color of the current pixel

  // float2 has two coordinate values that we can change
  // locationInSource.x, locationInSource.y,
  locationInSource.x = locationInSource.x * 1.5 ;
  locationInSource.y = locationInSource.y *.9; 

  color = tex2D( ourImage ,( locationInSource.xy));

  // What to try in this tutorial
  // 01:   color = tex2D( ourImage , sin(locationInSource.xy));
  // 02:   color = tex2D( ourImage , cos(locationInSource.xy));

    return color ;

}
