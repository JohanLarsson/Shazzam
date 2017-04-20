/// <class>GrowablePoissonDiskEffect</class>

/// <description>An effect that blurs the input using Poisson disk sampling.</description>

//-----------------------------------------------------------------------------------------
// Shader constant register mappings (scalars - float, double, Point, Color, Point3D, etc.)
//-----------------------------------------------------------------------------------------

/// <summary>The radius of the Poisson disk (in pixels).</summary>
/// <minValue>1</minValue>
/// <maxValue>10</maxValue>
/// <defaultValue>5</defaultValue>
float DiskRadius : register(C0);

/// <summary>The size of the input (in pixels).</summary>
/// <type>Size</type>
/// <minValue>1,1</minValue>
/// <maxValue>1000,1000</maxValue>
/// <defaultValue>600,400</defaultValue>
float2 InputSize : register(C1);

static const float2 poisson[12] = 
{
        float2(-0.326212f, -0.40581f),
        float2(-0.840144f, -0.07358f),
        float2(-0.695914f, 0.457137f),
        float2(-0.203345f, 0.620716f),
        float2(0.96234f, -0.194983f),
        float2(0.473434f, -0.480026f),
        float2(0.519456f, 0.767022f),
        float2(0.185461f, -0.893124f),
        float2(0.507431f, 0.064425f),
        float2(0.89642f, 0.412458f),
        float2(-0.32194f, -0.932615f),
        float2(-0.791559f, -0.59771f)
};

//--------------------------------------------------------------------------------------
// Sampler Inputs (Brushes, including Texture1)
//--------------------------------------------------------------------------------------

sampler2D Texture1Sampler : register(S0);

//--------------------------------------------------------------------------------------
// Pixel Shader
//--------------------------------------------------------------------------------------

float4 main(float2 uv : TEXCOORD) : COLOR
{
    float4 cOut;

    // Center tap
    cOut = tex2D(Texture1Sampler, uv);
    for(int tap = 0; tap < 12; tap++)
    {
        float2 coord= uv.xy + (poisson[tap] / InputSize * DiskRadius);
        // Sample pixel
        cOut += tex2D(Texture1Sampler, coord);
    }

    return(cOut / 13.0f);
}


