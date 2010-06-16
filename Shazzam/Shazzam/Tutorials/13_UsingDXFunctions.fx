
sampler2D ourImage : register(s0);

float4 main(float2 locationInSource : TEXCOORD) : COLOR
{
  // create a variable to hold our color
  float4 color;
  // get the color of the current pixel

  // use the DirectX sqrt function (square root)
  locationInSource.x = sqrt(sqrt(locationInSource.x))  ;
  locationInSource.y = sqrt(locationInSource.y) ;

  // then apply a sine value

  color = tex2D( ourImage , locationInSource.xy);

    return color ;

}
