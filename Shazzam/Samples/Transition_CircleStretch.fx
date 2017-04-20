/// <class>CircleRevealTransitionEffect</class>

/// <description>A transition effect </description>
/// <summary>The amount(%) of the transition from first texture to the second texture. </summary>
/// <minValue>0</minValue>
/// <maxValue>100</maxValue>
/// <defaultValue>30</defaultValue>
float Progress : register(C0);

/// <summary>The Center point of the effect </summary>
/// <minValue>0,0</minValue>
/// <maxValue>1,1</maxValue>
/// <defaultValue>.5,.5</defaultValue>
float2 CenterPoint: register(C1);

sampler2D Texture1 : register(s0);
sampler2D Texture2 : register(s1);

struct VS_OUTPUT
{
    float4 Position  : POSITION;
    float4 Color     : COlOR;
    float2 UV        : TEXCOORD;
};

float DistanceFromCenterToSquareEdge(float2 dir)
{
  dir = abs(dir);
  float dist = dir.x > dir.y ? dir.x : dir.y;
  return dist;
}

float4 CircleStretch(float2 uv,float progress)
{
  float2 center = CenterPoint;
  float radius = progress * 0.70710678;
  float2 toUV = uv - center;
  float len = length(toUV);
  float2 normToUV = toUV / len;

  if(len < radius)
  {
    float distFromCenterToEdge = DistanceFromCenterToSquareEdge(normToUV) / 2.0;
    float2 edgePoint = center + distFromCenterToEdge * normToUV;

    float minRadius = min(radius, distFromCenterToEdge);
    float percentFromCenterToRadius = len / minRadius;

    float2 newUV = lerp(center, edgePoint, percentFromCenterToRadius);
    return tex2D(Texture1, newUV);
  }
  else
  {
    float distFromCenterToEdge = DistanceFromCenterToSquareEdge(normToUV);
    float2 edgePoint = center + distFromCenterToEdge * normToUV;
    float distFromRadiusToEdge = distFromCenterToEdge - radius;

    float2 radiusPoint = center + radius * normToUV;
    float2 radiusToUV = uv - radiusPoint;

    float percentFromRadiusToEdge = length(radiusToUV) / distFromRadiusToEdge;

    float2 newUV = lerp(center, edgePoint, percentFromRadiusToEdge);
    return tex2D(Texture2, newUV);
  }
}

//--------------------------------------------------------------------------------------
// Pixel Shader
//--------------------------------------------------------------------------------------
float4 main(VS_OUTPUT input) : COlOR
{
  return CircleStretch(input.UV, Progress/100);
}
