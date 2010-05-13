/// <class>BrightExtractEffect</class>

/// <description>An effect that dims all but the brightest pixels.</description>

//-----------------------------------------------------------------------------------------
// Shader constant register mappings (scalars - float, double, Point, Color, Point3D, etc.)
//-----------------------------------------------------------------------------------------

/// <summary>Threshold below which values are discarded.</summary>
/// <minValue>0</minValue>
/// <maxValue>1</maxValue>
/// <defaultValue>0.5</defaultValue>
float Threshold : register(C0);

//--------------------------------------------------------------------------------------
// Sampler Inputs (Brushes, including Texture1)
//--------------------------------------------------------------------------------------

sampler2D Texture1Sampler : register(S0);


//--------------------------------------------------------------------------------------
// Pixel Shader
//--------------------------------------------------------------------------------------

float4 main(float2 uv : TEXCOORD) : COLOR
{
    // Look up the original image color.
    float4 originalColor = tex2D(Texture1Sampler, uv);
    
    // Undo pre-multiplied alpha.
	float3 rgb = originalColor.rgb / originalColor.a;

    // Adjust RGB to keep only values brighter than the specified threshold.
    rgb = saturate((rgb - Threshold) / (1 - Threshold));
    
    // Re-apply alpha.
    return float4(rgb * originalColor.a, originalColor.a);
}


