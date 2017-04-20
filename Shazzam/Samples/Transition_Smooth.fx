/// <class>SmoothTransitionEffect</class>

/// <description>A transition effect </description>
/// <summary>The amount(%) of the transition from first texture to the second texture. </summary>

/// <minValue>0</minValue>
/// <maxValue>100</maxValue>
/// <defaultValue>30</defaultValue>
float progress : register(C0);

/// <minValue>-1</minValue>
/// <maxValue>1</maxValue>
/// <defaultValue>.4</defaultValue>
float twistAmount : register(C1);
sampler2D Texture1 : register(s0);
sampler2D Texture2 : register(s1);


float4 main(float2 uv : TEXCOORD) : COlOR
{
  float cellcount = 10;
  float cellsize = 1.0 / cellcount;
  
  float2 cell = floor(uv * cellcount);
  float2 oddeven = fmod(cell, 2.0);
  float cellTwistAmount = twistAmount;
  if (oddeven.x < 1.0)
  {
    cellTwistAmount *= -1;
  }
  if (oddeven.y < 1.0)
  {
    cellTwistAmount *= -1;
  }
  
  float2 newUV = frac(uv * cellcount);
  
  float2 center = float2(0.5,0.5);
  float2 toUV = newUV - center;
  float distanceFromCenter = length(toUV);
  float2 normToUV = toUV / distanceFromCenter;
  float angle = atan2(normToUV.y, normToUV.x);
  
  angle += max(0, 0.5-distanceFromCenter) * cellTwistAmount * progress;
  float2 newUV2;
  sincos(angle, newUV2.y, newUV2.x);
  newUV2 *= distanceFromCenter;
  newUV2 += center;
  
  newUV2 *= cellsize;
  newUV2 += cell * cellsize;
  
  float4 c1 = tex2D(Texture2, newUV2);
    float4 c2 = tex2D(Texture1, uv);

    return lerp(c1,c2, progress);
}

