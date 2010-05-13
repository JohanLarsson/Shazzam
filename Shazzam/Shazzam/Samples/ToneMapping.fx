/// <class>ToneMappingEffect</class>

/// <description>An effect that applies defogging, exposure, gamma, vignette, and blue shift corrections.</description>

//-----------------------------------------------------------------------------------------
// Shader constant register mappings (scalars - float, double, Point, Color, Point3D, etc.)
//-----------------------------------------------------------------------------------------

/// <summary>The amount of fog to remove.</summary>
/// <minValue>0</minValue>
/// <maxValue>1</maxValue>
/// <defaultValue>0.01</defaultValue>
float Defog : register(C0);

/// <summary>The fog color.</summary>
/// <defaultValue>White</defaultValue>
float4 FogColor : register(C1);

/// <summary>The exposure adjustment.</summary>
/// <minValue>-1</minValue>
/// <maxValue>1</maxValue>
/// <defaultValue>0.2</defaultValue>
float Exposure : register(C2);

/// <summary>The gamma correction exponent.</summary>
/// <minValue>0.5</minValue>
/// <maxValue>2</maxValue>
/// <defaultValue>0.8</defaultValue>
float Gamma : register(C3);

/// <summary>The center of vignetting.</summary>
/// <minValue>0,0</minValue>
/// <maxValue>1,1</maxValue>
/// <defaultValue>0.5,0.5</defaultValue>
float2 VignetteCenter : register(C4);

/// <summary>The radius of vignetting.</summary>
/// <minValue>0</minValue>
/// <maxValue>1</maxValue>
/// <defaultValue>1</defaultValue>
float VignetteRadius : register(C5);

/// <summary>The amount of vignetting.</summary>
/// <minValue>-1</minValue>
/// <maxValue>1</maxValue>
/// <defaultValue>-1</defaultValue>
float VignetteAmount : register(C6);

/// <summary>The amount of blue shift.</summary>
/// <minValue>0</minValue>
/// <maxValue>1</maxValue>
/// <defaultValue>0.25</defaultValue>
float BlueShift : register(C7);

//--------------------------------------------------------------------------------------
// Sampler Inputs (Brushes, including Texture1)
//--------------------------------------------------------------------------------------

sampler2D Texture1Sampler : register(S0);


//--------------------------------------------------------------------------------------
// Pixel Shader
//--------------------------------------------------------------------------------------

float4 main(float2 uv : TEXCOORD) : COLOR
{
    float4 c = tex2D(Texture1Sampler, uv);
    c.rgb = max(0, c.rgb - Defog * FogColor.rgb);
    c.rgb *= pow(2.0f, Exposure);
    c.rgb = pow(c.rgb, Gamma);

    float2 tc = uv - VignetteCenter;
    float v = length(tc) / VignetteRadius;
    c.rgb += pow(v, 4) * VignetteAmount;

    float3 d = c.rgb * float3(1.05f, 0.97f, 1.27f);
    c.rgb = lerp(c.rgb, d, BlueShift);
    
    return c;
}


