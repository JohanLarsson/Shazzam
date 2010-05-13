/// <class>BandsEffect</class>

/// <description>An effect that creates bands of bright regions.</description>

//-----------------------------------------------------------------------------------------
// Shader constant register mappings (scalars - float, double, Point, Color, Point3D, etc.)
//-----------------------------------------------------------------------------------------

/// <summary>The number of verical bands to add to the output. The higher the value the more bands.</summary>
/// <minValue>0</minValue>
/// <maxValue>150</maxValue>
/// <defaultValue>65</defaultValue>
float BandDensity : register(C0);

/// <summary>Intensity of each band.</summary>
/// <minValue>0</minValue>
/// <maxValue>.5</maxValue>
/// <defaultValue>0.056</defaultValue>
float BandIntensity : register(C1);

//--------------------------------------------------------------------------------------
// Sampler Inputs (Brushes, including Texture1)
//--------------------------------------------------------------------------------------

sampler2D Texture1Sampler : register(S0);

//--------------------------------------------------------------------------------------
// Pixel Shader
//--------------------------------------------------------------------------------------

float4 main(float2 uv : TEXCOORD) : COLOR
{
    float4 color;

    color = tex2D(Texture1Sampler, uv.xy);

    color.rgb+=tan(uv.x*BandDensity)*BandIntensity;
    return color;
}
