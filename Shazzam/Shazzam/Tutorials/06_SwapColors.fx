
sampler2D ourImage : register(s0);

float4 main(float2 locationInSource : TEXCOORD) : COLOR
{
  // create a variable to hold our color
  float4 color;
  // get the color of the current pixel

  color = tex2D( ourImage , locationInSource.xy);

  
  float temp;
  
  temp = color.b;
  color.r= color.b; // (assigns the value in the blue channel to the red channel)
  

  // What to try in this tutorial 

  // 01: swap colors
  // float blue = color.b;
  // float red = color.r;
  // float green = color.g;
  // color.b= green; // (assigns the value in the blue channel to the red channel)
  // color.r =blue;
  // color.g = red;
    
 
    return color;

}
