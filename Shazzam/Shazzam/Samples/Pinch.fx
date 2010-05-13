/// <class>PinchEffect</class>

/// <description>An effect that pinches a circular region.</description>

//-----------------------------------------------------------------------------------------
// Shader constant register mappings (scalars - float, double, Point, Color, Point3D, etc.)
//-----------------------------------------------------------------------------------------

/// <summary>The center point of the pinched region.</summary>
/// <minValue>0,0</minValue>
/// <maxValue>1,1</maxValue>
/// <defaultValue>0.5,0.5</defaultValue>
float2 Center : register(C0);

/// <summary>The radius of the pinched region.</summary>
/// <minValue>0</minValue>
/// <maxValue>1</maxValue>
/// <defaultValue>0.25</defaultValue>
float Radius : register(C1);

/// <summary>The strength of the pinch effect.</summary>
/// <minValue>0</minValue>
/// <maxValue>2</maxValue>
/// <defaultValue>1</defaultValue>
float Strength : register(C2);

/// <summary>The aspect ratio (width / height) of the input.</summary>
/// <minValue>0.5</minValue>
/// <maxValue>2</maxValue>
/// <defaultValue>1.5</defaultValue>
float AspectRatio : register(C3);

//--------------------------------------------------------------------------------------
// Sampler Inputs (Brushes, including Texture1)
//--------------------------------------------------------------------------------------

sampler2D Texture1Sampler : register(S0);


//--------------------------------------------------------------------------------------
// Pixel Shader
//--------------------------------------------------------------------------------------

float4 main(float2 uv : TEXCOORD) : COLOR
{
    float2 dir = Center - uv;
    float2 scaledDir = dir;
    scaledDir.y /= AspectRatio;
    float dist = length(scaledDir);
    float range = saturate(1 - (dist / (abs(-sin(Radius * 8) * Radius) + 0.00000001F)));
    float2 samplePoint = uv + dir * range * Strength;
    return tex2D(Texture1Sampler, samplePoint);
}


