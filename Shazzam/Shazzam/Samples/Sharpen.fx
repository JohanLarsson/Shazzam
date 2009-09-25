/// <class>SharpenEffect</class>
/// <namespace>Shazzam.Shaders</namespace>
/// <description>An effect that sharpens the input.</description>

//-----------------------------------------------------------------------------------------
// Shader constant register mappings (scalars - float, double, Point, Color, Point3D, etc.)
//-----------------------------------------------------------------------------------------

/// <summary>The amount of sharpening.</summary>
/// <minValue>0</minValue>
/// <maxValue>2</maxValue>
/// <defaultValue>1</defaultValue>
float Amount : register(C0);

/// <summary>The size of the input (in pixels).</summary>
/// <type>Size</type>
/// <minValue>1,1</minValue>
/// <maxValue>1000,1000</maxValue>
/// <defaultValue>800,600</defaultValue>
float2 InputSize : register(C1);

//--------------------------------------------------------------------------------------
// Sampler Inputs (Brushes, including ImplicitInput)
//--------------------------------------------------------------------------------------

sampler2D implicitInputSampler : register(S0);


//--------------------------------------------------------------------------------------
// Pixel Shader
//--------------------------------------------------------------------------------------

float4 main(float2 uv : TEXCOORD) : COLOR
{
	float2 offset = 1 / InputSize;
    float4 color = tex2D(implicitInputSampler, uv);
    color.rgb += tex2D(implicitInputSampler, uv - offset) * Amount;
    color.rgb -= tex2D(implicitInputSampler, uv + offset) * Amount;
    return color;
}


