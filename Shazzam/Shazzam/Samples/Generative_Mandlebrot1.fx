/// <class>GenerateStar</class>

/// <description>An effect that swirls the input in alternating clockwise and counterclockwise bands.</description>

//-----------------------------------------------------------------------------------------
// Shader constant register mappings (scalars - float, double, Point, Color, Point3D, etc.)
//-----------------------------------------------------------------------------------------

/// <summary>The center of the star.  </summary>
/// <minValue>0,0</minValue>
/// <maxValue>2,2</maxValue>
/// <defaultValue>.5,.5</defaultValue>
float2 Center : register(C0);



/// <summary>The strength of the effect.</summary>
/// <minValue>-2</minValue>
/// <maxValue>2</maxValue>
/// <defaultValue>1</defaultValue>
float ColorStrength : register(C2);


/// <defaultValue>Blue</defaultValue>
float4 mainColor :  register(C4); 

/// <defaultValue>Orange</defaultValue> 
float4 secondaryColor : register(C5);   

/// <minValue>0.5</minValue>
/// <maxValue>8</maxValue>
/// <defaultValue>2</defaultValue>
float  ringMultiplier : register(C6);  
 
//--------------------------------------------------------------------------------------
// Sampler Inputs (Brushes, including ImplicitInput)
//--------------------------------------------------------------------------------------

sampler2D implicitInputSampler : register(S0);

//--------------------------------------------------------------------------------------
// Pixel Shader
//--------------------------------------------------------------------------------------

float4 main(float2 uv : TEXCOORD) : COLOR
{
   float2 c = (uv - .5) * float2(3.8,3); 
   float2 v = c;
  
   for (int n = 0; n < 70; n++){
   v = float2(pow(v.x,2) - pow(v.y,2), v.x * v.y * 1.4) + c;    } 
   float4 color =float4(uv.x, uv.y, uv.y, uv.x);// = tex2D(implicitInputSampler,uv);
   float4 result =float4(uv.y, uv.x, uv.y, uv.x);// = (dot(v, v) > 1) ? 1- color :color; 
   float red = (dot(v, v) > 1) ? 1- color.r :color.r; 
    float blue = (dot(v, v) > 1) ? 1- color.b :color.b; 
     float green = (dot(v, v) > 1) ? 1- color.g :color.g; 
   result.a = 1; 
   result.r = red;
   result.b = blue;
  // result.g = green;
   return result;
}
