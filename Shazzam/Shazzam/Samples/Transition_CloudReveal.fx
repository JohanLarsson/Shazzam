/// <class>CloudRevealTransitionEffect</class>

/// <description>A transition effect </description>
/// <summary>The amount(%) of the transition from first texture to the second texture. </summary>

/// <minValue>0</minValue>
/// <maxValue>100</maxValue>
/// <defaultValue>30</defaultValue>
float Progress : register(C0);

sampler2D Texture1 : register(s0);
sampler2D Texture2 : register(s1);

// default texturemap for this effect is Clouds
sampler2D TextureMap : register(s2);

struct VS_OUTPUT
{
    float4 Position  : POSITION;
    float4 Color     : COlOR;
    float2 UV        : TEXCOORD;
};

float4 CloudReveal(float2 uv, float progress)
{
  float cloud = tex2D(TextureMap, uv).r ;
  float4 c1 = tex2D(Texture2, uv);
  float4 c2 = tex2D(Texture1, uv);

  float a;
  float divide = .3;
  if (progress < divide)
  {
    a = lerp(0.0, cloud, progress / divide);
  }
  else
  {
    a = lerp(cloud, 1.0, (progress - divide) / divide);
  }

    return (a < 0.5) ? c1 : c2;
}

//--------------------------------------------------------------------------------------
// Pixel Shader
//--------------------------------------------------------------------------------------
float4 main(VS_OUTPUT input) : COlOR
{
  return CloudReveal(input.UV, Progress/100);
}
