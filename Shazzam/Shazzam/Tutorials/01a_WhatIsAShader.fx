// Tutorial Title:  What is a Shader
// Lesson Plan   :  Explain what a shader is.
sampler2D ourImage : register(s0);

float4 main(float2 locationInSource : TEXCOORD) : COLOR
{

  // a shader is an algorithm that is compiled
  // and loaded into the Graphics Processor Unit (GPU)
  // this algorithm is run once, for every pixel in the input image
  // GPUs are efficient parallel  processors and will run
  // your algorithm on thousands of pixels at a time
    return tex2D( ourImage , locationInSource.xy);


}
