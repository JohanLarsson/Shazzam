//--------------------------------------------------------------------------------------
// 
// WPF ShaderEffect HLSL -- DirectionalBlurEffect
//
//--------------------------------------------------------------------------------------

//-----------------------------------------------------------------------------------------
// Shader constant register mappings (scalars - float, double, Point, Color, Point3D, etc.)
//-----------------------------------------------------------------------------------------

float Angle : register(C0);
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
    float rad = Angle * 0.0174533f;
    float xOffset = cos(rad);
    float yOffset = sin(rad);

    for(int i=0; i<16; i++)
    {
        uv.x = uv.x - BlurAmount * xOffset;
        uv.y = uv.y - BlurAmount * yOffset;
        c += tex2D(implicitInputSampler, uv);
    }
    c /= 16;
    
    return c;
}