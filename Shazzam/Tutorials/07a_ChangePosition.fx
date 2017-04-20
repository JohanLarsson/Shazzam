
sampler2D ourImage : register(s0);

float4 main(float2 locationInSource : TEXCOORD) : COLOR
{
  // create a variable to hold our color
  float4 color;
  // get the color of the current pixel

  // float2 has two coordinate values that we can change
  // locationInSource.x, locationInSource.y,

  color = tex2D( ourImage , locationInSource.xy *2); // swizzle the x and y 
  // makes the image smaller

  // What to try in this tutorial
  // 01: locationInSource.xy *.5

    return color ;

}
