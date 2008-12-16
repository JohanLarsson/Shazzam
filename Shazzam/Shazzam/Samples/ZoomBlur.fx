//--------------------------------------------------------------------------------------
// 
// WPF ShaderEffect HLSL -- ZoomBlurEffect
//
//--------------------------------------------------------------------------------------

//-----------------------------------------------------------------------------------------
// Shader constant register mappings (scalars - float, double, Point, Color, Point3D, etc.)
//-----------------------------------------------------------------------------------------

float Center : register(C0);
float BlurAmount : register(C1);

//--------------------------------------------------------------------------------------
// Sampler Inputs (Brushes, including ImplicitInput)
//--------------------------------------------------------------------------------------

sampler2D  implicitInputSampler : register(S0);

//--------------------------------------------------------------------------------------
// Pixel Shader
//--------------------------------------------------------------------------------------

float4 main(float2 uv : TEXCOORD) : COLOR
{
    float4 c = 0;    
    uv -= Center;

	for(int i=0; i<15; i++)
    {
        float scale = 1.0 + BlurAmount * (i / 14.0);
        c += tex2D(implicitInputSampler, uv * scale + Center );
    }
   
    c /= 15;
    return c;
}