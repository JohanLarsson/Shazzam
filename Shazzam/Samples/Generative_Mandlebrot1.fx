/// <class>GenerateMandelbrot</class>

/// <description>Create a Mandelbrot.  Does now use any of original pixels</description>

//-----------------------------------------------------------------------------------------
// Shader constant register mappings (scalars - float, double, Point, Color, Point3D, etc.)
//-----------------------------------------------------------------------------------------

/// <summary>The center of the effect.  </summary>
/// <minValue>0</minValue>
/// <maxValue>1</maxValue>
/// <defaultValue>.6</defaultValue>
float Center : register(C0);


/// <summary>The red strength of the effect.</summary>
/// <minValue>-2</minValue>
/// <maxValue>2</maxValue>
/// <defaultValue>2</defaultValue>
float RedStrength : register(C2);


/// <summary>The blue strength of the effect.</summary>
/// <minValue>-2</minValue>
/// <maxValue>2</maxValue>
/// <defaultValue>,.9</defaultValue>
float BlueStrength : register(C4);

/// <summary>The green strength of the effect.</summary>
/// <minValue>-2</minValue>
/// <maxValue>2</maxValue>
/// <defaultValue>1</defaultValue>
float GreenStrength : register(C6);

/// <summary>The Width of the effect.</summary>
/// <minValue>0</minValue>
/// <maxValue>4</maxValue>
/// <defaultValue>3.7</defaultValue>
float Width : register(C5);   

/// <summary>The Width of the effect.</summary>
/// <minValue>0</minValue>
/// <maxValue>4</maxValue>
/// <defaultValue>3</defaultValue>
float Height : register(C7); 

/// <summary>The Width of the effect.</summary>
/// <minValue>0</minValue>
/// <maxValue>4</maxValue>
/// <defaultValue>2</defaultValue>
float ScatterA : register(C3);
/// <summary>The Width of the effect.</summary>
/// <minValue>0</minValue>
/// <maxValue>4</maxValue>
/// <defaultValue>2</defaultValue>
float ScatterB : register(C8);

//--------------------------------------------------------------------------------------
// Sampler Inputs (Brushes, including ImplicitInput)
//--------------------------------------------------------------------------------------

sampler2D inputSource : register(S0);

//--------------------------------------------------------------------------------------
// Pixel Shader
//--------------------------------------------------------------------------------------

float4 main(float2 uv : TEXCOORD) : COLOR
{
   float2 c = (uv.xy - Center) * float2(Width,Height); 
   float2 v = c;
  
   for (int n = 0; n < 70; n++){
   v = float2(pow(v.x,ScatterA) - pow(v.y,ScatterB), v.x * v.y * 1.4) + c;    } 
   float4 color = float4(uv.x, uv.y, uv.y, uv.x);// = tex2D(inputSource,uv);
   float4 result = float4(uv.y, uv.x, uv.y, uv.x);// = (dot(v, v) > 1) ? 1- color :color; 
   float red = (dot(v, v) > RedStrength) ? RedStrength- color.r :color.r; 
   float blue = (dot(v, v) > BlueStrength) ? BlueStrength- color.b :color.b; 
   float green = (dot(v, v) > GreenStrength) ? GreenStrength- color.g :color.g; 
   result.a = 1; 
   result.r = red;
   result.b = blue;
   result.g = green;
   return result;
}
