/// <class>LineRevealTransitionEffect</class>

/// <description>A transition effect </description>
/// <summary>The amount(%) of the transition from first texture to the second texture. </summary>
/// <minValue>0</minValue>
/// <maxValue>100</maxValue>
/// <defaultValue>30</defaultValue>
float Progress : register(C0);

/// <minValue>0,0</minValue>
/// <maxValue>1,1</maxValue>
/// <defaultValue>1,0</defaultValue>
float2 lineOrigin : register(C1);

/// <minValue>0,0</minValue>
/// <maxValue>1,1</maxValue>
/// <defaultValue>1,1</defaultValue>
float2 lineNormal : register(C2);

/// <minValue>0,0</minValue>
/// <maxValue>1,1</maxValue>
/// <defaultValue>1,1</defaultValue>
float2 lineOffset : register(C3);

/// <minValue>0</minValue>
/// <maxValue>.1</maxValue>
/// <defaultValue>.05</defaultValue>
float fuzzyAmount : register(C4);
sampler2D Texture1 : register(s0);
sampler2D Texture2 : register(s1);

struct VS_OUTPUT
{
    float4 Position  : POSITION;
    float4 Color     : COlOR;
    float2 UV        : TEXCOORD;
};

float4 LineReveal(float2 uv,float progress)
{
  float2 currentLineOrigin = lerp(lineOrigin, lineOffset, progress);
  float2 normLineNormal = normalize(lineNormal);
  float4 c1 = tex2D(Texture2, uv);
    float4 c2 = tex2D(Texture1, uv);

  float distFromLine = dot(normLineNormal, uv-currentLineOrigin);
  float p = saturate((distFromLine + fuzzyAmount) / (2.0 * fuzzyAmount));
  return lerp(c2, c1, p);
}

//--------------------------------------------------------------------------------------
// Pixel Shader
//--------------------------------------------------------------------------------------
float4 main(VS_OUTPUT input) : COlOR
{
  return LineReveal(input.UV, Progress/100);
}
