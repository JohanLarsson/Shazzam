/// <class>RandomCircleTransitionEffect</class>

/// <description>A transition effect </description>
/// <summary>The amount(%) of the transition from first texture to the second texture. </summary>

/// <minValue>0</minValue>
/// <maxValue>100</maxValue>
/// <defaultValue>30</defaultValue>
float Progress : register(C0);

/// <minValue>-1</minValue>
/// <maxValue>1</maxValue>
/// <defaultValue>0</defaultValue>
float randomSeed : register(C1);
sampler2D Texture1 : register(s0);
sampler2D Texture2 : register(s1);
sampler2D TextureMap : register(s2);

struct VS_OUTPUT
{
    float4 Position  : POSITION;
    float4 Color     : COlOR;
    float2 UV        : TEXCOORD;
};

float4 RandomCircle(float2 uv,float progress)
{
  float radius = progress * 0.70710678;
  float2 fromCenter = uv - float2(0.5,0.5);
  float len = length(fromCenter);
  
  float2 toUV = normalize(fromCenter);
  float angle = (atan2(toUV.y, toUV.x) + 3.141592) / (2.0 * 3.141592);
  radius += progress * tex2D(TextureMap, float2(angle, frac(randomSeed + progress / 5.0))).r;
  
  if(len < radius)
  {
    return tex2D(Texture1, uv);
  }
  else
  {
    return tex2D(Texture2, uv);
  }
}

float4 main(VS_OUTPUT input) : COlOR
{
  return RandomCircle(input.UV, Progress/100);
}

