
sampler2D ourImage : register(s0);

float4 main(float2 locationInSource : TEXCOORD) : COLOR
{

		// The simplest variable declaration includes a type and a variable name, 
		// example: a floating-point declaration
		float x;
		
    // init the variable on declaration.
    float y = 5.6f;
		

    // declaring an array
		bool myBools[6];
		
    // init the array on declaration.
		bool moreBools[4] = {true, true, false, false};

		

   float4 color ={5,5,5,5};
		return color;

}