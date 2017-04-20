/// <class>WaveTransitionEffect</class>

/// <description>A transition effect </description>

/// <summary>The amount(%) of the transition from first texture to the second texture. </summary>
/// <minValue>0</minValue>
/// <maxValue>100</maxValue>
/// <defaultValue>30</defaultValue>
float Progress : register(C0);
sampler2D Texture1 : register(s0);
sampler2D Texture2 : register(s1);

float4 SampleWithBorder(float4 border, sampler2D tex, float2 uv)
{
  if (any(saturate(uv) - uv))
  {
    return border;
  }
  else
  {
    return tex2D(tex, uv);
  }
}

float4 Wave(float2 uv, float progress)
{
  float mag = 0.1;
  float phase = 14;
  float freq = 20;
  
  float2 newUV = uv + float2(mag * progress * sin(freq * uv.y + phase * progress), 0);
  
  float4 c1 = SampleWithBorder(0, Texture2, newUV);
    float4 c2 = tex2D(Texture1, uv);

    return lerp(c1,c2, progress);
}

float4 StandingWave(float2 uv,float progress)
{
  float pi = 3.141592;
  float mag = 0.01;
  float freq = 8 * pi;
  float freq2 = 6 * pi;
  
  float2 newUV = uv + mag * sin(progress*freq2) * float2(cos(freq * uv.x), sin(freq * uv.y));
  
  float4 c1 = tex2D(Texture2, frac(newUV));
    float4 c2 = tex2D(Texture1, uv);

    return lerp(c1,c2, progress);
}

float4 MotionBlur(float2 uv,float progress)
{
  float4 c1 = 0;
  int count = 26;
  float2 direction = float2(0.05, 0.05);
  float2 offset = progress * direction;
  float2 startUV = uv - offset * 0.5;
  float2 delta = offset / (count-1);
  
  for(int i=0; i<count; i++)
  {
    c1 += tex2D(Texture2, startUV + delta*i);
  }
  
  c1 /= count;
    return c1;
}

//--------------------------------------------------------------------------------------
// Pixel Shader
//--------------------------------------------------------------------------------------
float4 main(float2 uv : TEXCOORD) : COLOR
{
  // Normalize to 100%
  return Wave(uv, Progress/100);
}

