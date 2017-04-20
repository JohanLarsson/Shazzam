// Tutorial Title:  Show All the Input passed via the register
// Lesson Plan   :  Learn about the C registers and how each is represnted in the Settings window

//--------------------------------------------------------------------------------------
// float
//--------------------------------------------------------------------------------------

float SampleFloat : register(c0);

//--------------------------------------------------------------------------------------
// float2
//--------------------------------------------------------------------------------------

// float2 could be used for Point, Size or Vector.

float2 SamplePoint: register(C1);
float2 SampleVector : register(C2);

//--------------------------------------------------------------------------------------
// float3
//--------------------------------------------------------------------------------------

// float3 maps to Point3D.
// Not available in Silverlight.


float3 SamplePoint3D : register(C3);

//--------------------------------------------------------------------------------------
// float4
//--------------------------------------------------------------------------------------

// float4 could represent a Color or Point4D


float4 SampleColor: register(C4);

sampler2D inputTexture : register(S0);


float4 main(float2 uv : TEXCOORD) : COLOR
{
		float4 color;
	  return float4(0,2,3,4);;
}
