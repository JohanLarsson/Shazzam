/// <class>EmbossedEffect</class>

/// <description>An effect that embosses the input.</description>

//-----------------------------------------------------------------------------------------
// Shader constant register mappings (scalars - float, double, Point, Color, Point3D, etc.)
//-----------------------------------------------------------------------------------------

/// <summary>The amplitude of the embossing.</summary>
/// <minValue>0</minValue>
/// <maxValue>1</maxValue>
/// <defaultValue>0.5</defaultValue>
float Amount : register(C0);

/// <summary>The separation between samples (as a fraction of input size).</summary>
/// <minValue>0</minValue>
/// <maxValue>0.01</maxValue>
/// <defaultValue>0.003</defaultValue>
float Width : register(C1);

//--------------------------------------------------------------------------------------
// Sampler Inputs (Brushes, including Texture1)
//--------------------------------------------------------------------------------------

sampler2D Texture1Sampler : register(S0);


//--------------------------------------------------------------------------------------
// Pixel Shader
//--------------------------------------------------------------------------------------

float4 main(float2 uv : TEXCOORD) : COLOR
{
    float4 outC = {0.5, 0.5, 0.5, 1.0};

    outC -= tex2D(Texture1Sampler, uv - Width) * Amount;
    outC += tex2D(Texture1Sampler, uv + Width) * Amount;
    outC.rgb = (outC.r + outC.g + outC.b) / 3.0f;

    return outC;
}


