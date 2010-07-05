/// <class>RadialBlurTransitionEffect</class>

/// <description>A transition effect </description>
/// <summary>The amount(%) of the transition from first texture to the second texture. </summary>
/// <minValue>0</minValue>
/// <maxValue>100</maxValue>
/// <defaultValue>30</defaultValue>
float Progress : register(C0);

sampler2D Texture1 : register(s0);
sampler2D Texture2 : register(s1);

struct VS_OUTPUT
{
    float4 Position  : POSITION;
    float4 Color     : COlOR;
    float2 UV        : TEXCOORD;
};

float4 RadialBlur(float2 uv,float progress)
{
  float2 center = float2(0.5,0.5);
  float2 toUV = uv - center;
  float2 normToUV = toUV;
  
  
  float4 c1 = float4(0,0,0,0);
  int count = 24;
  float s = progress * 0.02;
  
  for(int i=0; i<count; i++)
  {
    c1 += tex2D(Texture2, uv - normToUV * s * i); 
  }
  
  c1 /= count;
    float4 c2 = tex2D(Texture1, uv);

  return lerp(c1, c2, progress);
}

//--------------------------------------------------------------------------------------
// Pixel Shader
//--------------------------------------------------------------------------------------
float4 main(VS_OUTPUT input) : COlOR
{
  return RadialBlur(input.UV, Progress/100);
}

