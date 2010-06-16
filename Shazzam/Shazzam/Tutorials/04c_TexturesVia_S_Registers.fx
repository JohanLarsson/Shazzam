// Tutorial Title:  Show All the Input passed via the register
// Lesson Plan   :  Learn about the S registers

// all shaders have at least one texture passed into the main function
sampler2D ourImage : register(s0);
// you can have three more textures
// note that each one is passed in via a different S register
sampler2D alternate1 : register(s1);
sampler2D alternate2 : register(s2);
sampler2D alternate3 : register(s3);


float4 main(float2 locationInSource : TEXCOORD) : COLOR
{
  // create a variable to hold our color
  float4 color;
  // get the color of the current pixel
  color = tex2D( ourImage , locationInSource.xy);

  
    return color;
  // TODO: look at these input parameter at runtime in Shazzam with the controls in the 'Change Shader Settings' tab.
}
    