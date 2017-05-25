/// <class>MaskEffect</class>
/// <description>An effect that makes black pixels transparent and all other pixels the provided color.</description>

//-----------------------------------------------------------------------------------------
// Shader constant register mappings (scalars - float, double, Point, Color, Point3D, etc.)
//-----------------------------------------------------------------------------------------

/// <summary>The color to use for nonblack pixels.</summary>
/// <defaultValue>#66FF0000</defaultValue>
float4 Color : register(C0);

//--------------------------------------------------------------------------------------
// Sampler Inputs (Brushes, including Texture1)
//--------------------------------------------------------------------------------------
sampler2D Texture1Sampler : register(S0);

float4 main(float2 uv : TEXCOORD) : COLOR
{
    float4 color = tex2D(Texture1Sampler, uv);
    if (all(color.rgb < 0.1))
    {
        return Color;
    }
   
    color.rgba = 0;
    return color;
}