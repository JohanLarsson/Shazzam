// Tutorial Title:  Add Color Parameters (1)
// Lesson Plan   :  Replace the colors in an existing input texture
sampler2D ourImage : register(s0);

// add top-level variables to the shader
// top-level vars are placed in the (Cx) registers

// the float type contains a 32-bit floating point value
float RedInput : register(C0);
float GreenInput : register(C1);
float BlueInput : register(C2);

float NotInRegister = 5 ;
float4 main(float2 locationInSource : TEXCOORD) : COLOR
{
  // create a variable to hold our color
  float4 color;
  // get the color of the current pixel
  color = tex2D( ourImage , locationInSource.xy);

  color.r= RedInput; // assign the Red variable to the red portion
  color.g= GreenInput;
  color.b= BlueInput;

  float getValue = NotInRegister;  // access the value
  // it will not show up in the parameters list however

 // the following line will fail however, because you cannot modify a globel var
 //  NotInRegister = 9 ;

  // modify these variable at runtime in Shazzam with the controls in the 'Tryout shader (adjust settings)' tab.

    return color;

}
    