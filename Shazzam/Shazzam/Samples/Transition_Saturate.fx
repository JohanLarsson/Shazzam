/// <class>SaturateTransitionEffect</class>

/// <description>A transition effect </description>
/// <summary>The amount(%) of the transition from first texture to the second texture. </summary>
/// <minValue>0</minValue>
/// <maxValue>100</maxValue>
/// <defaultValue>30</defaultValue>
float Progress : register(C0);

sampler2D Texture1 : register(s0);
sampler2D Texture2 : register(s1);



float4 Saturate(float2 uv,float progress)
{
  float4 c1 = tex2D(Texture2, uv); 
  c1 = saturate(c1 * (2 * progress + 1));
    float4 c2 = tex2D(Texture1, uv);

  if ( progress > 0.8)
  {
    float new_progress = (progress - 0.8) * 5.0;
    return lerp(c1,c2, new_progress);
  }
  else
  {
    return c1;
  }
}


//--------------------------------------------------------------------------------------
// Pixel Shader
//--------------------------------------------------------------------------------------
float4 main(float2 uv : TEXCOORD) : COlOR
{
  return Saturate(uv, Progress/100);
}

