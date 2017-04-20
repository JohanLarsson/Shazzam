/// <class>ContrastAdjustEffect</class>

/// <description>An effect that controls brightness and contrast.</description>

//-----------------------------------------------------------------------------------------
// Shader constant register mappings (scalars - float, double, Point, Color, Point3D, etc.)
//-----------------------------------------------------------------------------------------

/// <summary>The brightness offset.</summary>
/// <minValue>-1</minValue>
/// <maxValue>1</maxValue>
/// <defaultValue>0</defaultValue>
float Brightness : register(C0);

/// <summary>The contrast multiplier.</summary>
/// <minValue>0</minValue>
/// <maxValue>2</maxValue>
/// <defaultValue>1.5</defaultValue>
float Contrast : register(C1);

//--------------------------------------------------------------------------------------
// Sampler Inputs (Brushes, including Texture1)
//--------------------------------------------------------------------------------------

sampler2D Texture1Sampler : register(S0);

//--------------------------------------------------------------------------------------
// Pixel Shader
//--------------------------------------------------------------------------------------

float4 main(float2 uv : TEXCOORD) : COLOR
{
    float4 pixelColor = tex2D(Texture1Sampler, uv);
    pixelColor.rgb /= pixelColor.a;
    
    // Apply contrast.
    pixelColor.rgb = ((pixelColor.rgb - 0.5f) * max(Contrast, 0)) + 0.5f;
    
    // Apply brightness.
    pixelColor.rgb += Brightness;
    
    // Return final pixel color.
    pixelColor.rgb *= pixelColor.a;
    return pixelColor;
}


