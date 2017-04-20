/// <class>CircleRevealTransitionEffect</class>

/// <description>A transition effect </description>
/// <summary>The amount(%) of the transition from first texture to the second texture. </summary>
/// <minValue>0</minValue>
/// <maxValue>100</maxValue>
/// <defaultValue>30</defaultValue>
float Progress : register(C0);

/// <summary>The fuzziness factor. </summary>
/// <minValue>0</minValue>
/// <maxValue>1</maxValue>
/// <defaultValue>.01</defaultValue>
float FuzzyAmount : register(C1);

/// <summary>The size of the circle. </summary>
/// <minValue>0</minValue>
/// <maxValue>2</maxValue>
/// <defaultValue>.5</defaultValue>
float CircleSize : register(C2);

/// <summary>The Center point of the effect </summary>
/// <minValue>0,0</minValue>
/// <maxValue>1,1</maxValue>
/// <defaultValue>.5,.5</defaultValue>
float2 CenterPoint: register(C3);

sampler2D Texture1 : register(s0);
sampler2D Texture2 : register(s1);

struct VS_OUTPUT
{
    float4 Position  : POSITION;
    float4 Color     : COlOR;
    float2 UV        : TEXCOORD;
};

float4 Circle(float2 uv,float progress)
{
  float radius = -FuzzyAmount + progress * (CircleSize + 2.0 * FuzzyAmount);
  float fromCenter = length(uv - CenterPoint);
  float distFromCircle = fromCenter - radius;

  float4 c1 = tex2D(Texture2, uv);
    float4 c2 = tex2D(Texture1, uv);

  float p = saturate((distFromCircle + FuzzyAmount) / (2.0 * FuzzyAmount));
  return lerp(c2, c1, p);
}

//--------------------------------------------------------------------------------------
// Pixel Shader
//--------------------------------------------------------------------------------------
float4 main(VS_OUTPUT input) : COlOR
{
  return Circle(input.UV, Progress/100);
}
