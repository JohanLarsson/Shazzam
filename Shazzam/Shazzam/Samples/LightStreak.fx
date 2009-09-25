/// <class>LightStreakEffect</class>
/// <namespace>Shazzam.Shaders</namespace>
/// <description>An effect that intensifies bright areas.</description>

//-----------------------------------------------------------------------------------------
// Shader constant register mappings (scalars - float, double, Point, Color, Point3D, etc.)
//-----------------------------------------------------------------------------------------

/// <summary>Threshold for selecting bright pixels.</summary>
/// <minValue>0</minValue>
/// <maxValue>1</maxValue>
/// <defaultValue>0.5</defaultValue>
float BrightThreshold : register(C0);

/// <summary>Contrast factor.</summary>
/// <minValue>0</minValue>
/// <maxValue>1</maxValue>
/// <defaultValue>0.5</defaultValue>
float Scale : register(C1);

/// <summary>Attenuation factor.</summary>
/// <minValue>0</minValue>
/// <maxValue>1</maxValue>
/// <defaultValue>0.25</defaultValue>
float Attenuation : register(C2);

/// <summary>Direction of light streaks.</summary>
/// <type>Vector</type>
/// <minValue>-1,-1</minValue>
/// <maxValue>1,1</maxValue>
/// <defaultValue>0.5,1</defaultValue>
float2 Direction : register(C3);

/// <summary>Size of the input (in pixels).</summary>
/// <type>Size</type>
/// <minValue>1,1</minValue>
/// <maxValue>1000,1000</maxValue>
/// <defaultValue>800,600</defaultValue>
float2 InputSize : register(C4);

//--------------------------------------------------------------------------------------
// Sampler Inputs (Brushes, including ImplicitInput)
//--------------------------------------------------------------------------------------

sampler2D implicitInputSampler : register(S0);

//--------------------------------------------------------------------------------------
// Pixel Shader
//--------------------------------------------------------------------------------------

float4 main(float2 uv : TEXCOORD) : COLOR
{
    const static int numSamples = 2;
    int Iteration = 1;

    float4 pixelColor = tex2D(implicitInputSampler, uv);
    float3 rgb = pixelColor.rgb / pixelColor.a;
    float3 bright = saturate((rgb - BrightThreshold) / (1 - BrightThreshold));

    rgb += bright;

    float weightIter = pow(numSamples, Iteration);
    
    for (int sample = 0; sample < numSamples; sample++)
    {
        float weight = pow(Attenuation, weightIter * sample);
        float2 texCoord = uv + (Direction * weightIter * float2(sample, sample) / InputSize);
        float4 sampleColor = tex2D(implicitInputSampler, texCoord);
        rgb += saturate(weight) * sampleColor.rgb / sampleColor.a;
    }
      
    return float4(rgb * Scale * pixelColor.a, pixelColor.a);
}


