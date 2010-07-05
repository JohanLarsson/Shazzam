/// <class>SlideInTransitionEffect</class>

/// <description>A transition effect </description>
/// <summary>The amount(%) of the transition from first texture to the second texture. </summary>
/// 
/// <minValue>0</minValue>
/// <maxValue>100</maxValue>
/// <defaultValue>30</defaultValue>
float Progress : register(C0);

/// <minValue>0,0</minValue>
/// <maxValue>1,1</maxValue>
/// <defaultValue>1,0</defaultValue>
float2 slideAmount : register(C1);
sampler2D Texture1 : register(s0);
sampler2D Texture2 : register(s1);

struct VS_OUTPUT
{
    float4 Position  : POSITION;
    float4 Color     : COlOR;
    float2 UV        : TEXCOORD;
};

float4 SlideLeft(float2 uv,float progress)
{
  uv += slideAmount * progress;
  if(any(saturate(uv)-uv))
  {	
    uv = frac(uv);
    return tex2D(Texture1, uv);
  }
  else
  {
    return tex2D(Texture2, uv);
  }
}

//--------------------------------------------------------------------------------------
// Pixel Shader
//--------------------------------------------------------------------------------------
float4 main(VS_OUTPUT input) : COlOR
{
  return SlideLeft(input.UV, Progress/100);
}

