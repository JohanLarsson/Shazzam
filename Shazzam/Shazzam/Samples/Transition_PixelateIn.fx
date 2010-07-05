/// <class>PixelateInTransitionEffect</class>

/// <description>A transition effect </description>
/// <summary>The amount(%) of the transition from first texture to the second texture. </summary>
/// <minValue>0</minValue>
/// <maxValue>100</maxValue>
/// <defaultValue>30</defaultValue>
float Progress : register(C0);

sampler2D Texture1 : register(s0);
sampler2D Texture2 : register(s1);

float4 PixelateIn(float2 uv,float progress)
{
  float pixels = 10 + 1000 * progress * progress;
  float2 newUV = round(uv * pixels) / pixels;
    float4 c2 = tex2D(Texture1, newUV);

  return c2;
}

//--------------------------------------------------------------------------------------
// Pixel Shader
//--------------------------------------------------------------------------------------
float4 main(float2 uv : TEXCOORD) : COlOR
{
  return PixelateIn(uv, Progress/100);
}
