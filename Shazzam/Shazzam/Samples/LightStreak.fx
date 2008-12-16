//--------------------------------------------------------------------------------------
// 
// WPF ShaderEffect HLSL -- LightStreakEffect
//
//--------------------------------------------------------------------------------------

//-----------------------------------------------------------------------------------------
// Shader constant register mappings (scalars - float, double, Point, Color, Point3D, etc.)
//-----------------------------------------------------------------------------------------

float BrightThreshold : register(C0);
float Scale : register(C1);
float Attenuation : Attenuation : register(C2);

//--------------------------------------------------------------------------------------
// Sampler Inputs (Brushes, including ImplicitInput)
//--------------------------------------------------------------------------------------

sampler2D implicitInputSampler : register(S0);


//--------------------------------------------------------------------------------------
// Pixel Shader
//--------------------------------------------------------------------------------------

float4 main(float2 uv : TEXCOORD) : COLOR
{
    float2 Direction = {1.0f, 1.0f};
    float2 PixelSize = {0.0009766f, 0.0013021f};
    const static int numSamples = 2;
    int Iteration = 1.0;

    float4 pixelColor = tex2D(implicitInputSampler, uv);
    float4 bright = saturate((pixelColor - BrightThreshold) / (1 - BrightThreshold));

    pixelColor += bright;

    float weightIter = pow(numSamples, Iteration);
    
    for(int sample=0; sample<numSamples; sample++)
    {
        float weight = pow(Attenuation, weightIter * sample);
        float2 texCoord = uv + (Direction * weightIter * float2(sample, sample) * PixelSize);
        pixelColor.rgb += saturate(weight) * tex2D(implicitInputSampler,  texCoord);
    }
      
    return pixelColor * Scale;
}


