/// <class>CirclesEffect</class>

/// <description>An effect that generates concentric circles.</description>

//-----------------------------------------------------------------------------------------
// Shader constant register mappings (scalars - float, double, Point, Color, Point3D, etc.)
//-----------------------------------------------------------------------------------------

/// <summary>The center of the swirl. (100,100) is lower right corner </summary>
/// <minValue>0,0</minValue>
/// <maxValue>100,100</maxValue>
/// <defaultValue>50,50</defaultValue>
float2 Center : register(C0);

/// <summary>The number of bands in the swirl.</summary>
/// <minValue>0</minValue>
/// <maxValue>20</maxValue>
/// <defaultValue>10</defaultValue>
float Bands : register(C1);

/// <summary>The strength of the effect.</summary>
/// <minValue>0</minValue>
/// <maxValue>4</maxValue>
/// <defaultValue>0.5</defaultValue>
float Size : register(C2);



//--------------------------------------------------------------------------------------
// Sampler Inputs (Brushes, including Texture1)
//--------------------------------------------------------------------------------------

sampler2D Texture1Sampler : register(S0);

//--------------------------------------------------------------------------------------
// Pixel Shader
//--------------------------------------------------------------------------------------

float4 main(float2 uv : TEXCOORD) : COLOR
{
   // normalize
   // ================================
   float2 centerNormalized  ={ Center.x/100,Center.y/100};
 //  centerNormalized.x = Center.x/100;
 //  centerNormalized.y = Center.y/100;
  // float4 color ={5,5,5,5};
   // ================================
  float2 dir = uv - centerNormalized;
  //dir.y /= AspectRatio;
  float dist = length(dir) * Size;
  float angle = atan2(dir.y, dir.x);

  float remainder = frac(dist * Bands);
  float fac;   
  if (remainder < 0.25)
  {
    fac = 1.0;
  }
  else if (remainder < 0.5)
  {
    // transition zone - go smoothly from previous zone to next.
    fac = 1 - 8 * (remainder - 0.25);
  }
  else if (remainder < 0.75)
  {
    fac = -1.0;
  }
  else
  {
    // transition zone - go smoothly from previous zone to next.
    fac = -(1 - 8 * (remainder - 0.75));
  }

  //float newAngle = angle + fac * Strength * dist;
  float2 newDir;
//	sincos(newAngle, newDir.y, newDir.x);
//	newDir.y *= AspectRatio;

  
  return tex2D(Texture1Sampler,  centerNormalized + dist + fac );
}
