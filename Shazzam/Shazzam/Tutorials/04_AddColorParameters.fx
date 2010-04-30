/// These are Shazzam elements, they are not part of the HLSL specification
/// <description>Add Paramters</description>
/// <class>ChangeColor1</class>
/// =============================================================================

sampler2D ourImage : register(s0);

// add top-level variables to the shader
// top-level vars are placed in the (Cx) registers

// the float type contains a 32-bit floating point value
float RedValue : register(C0);
float GreenValue : register(C1);
float BlueValue : register(C2);

float4 main(float2 locationInSource : TEXCOORD) : COLOR
{
	// create a variable to hold our color
  float4 color;
  // get the color of the current pixel
  color = tex2D( ourImage , locationInSource.xy);


	
	color.r= RedValue; // assign the Red variable to the red portion 
  color.g= GreenValue;
  color.b= BlueValue;


// modify these variable at runtime in Shazzam with the controls in the 'Change Shader Settings' tab.



		return color;

}
