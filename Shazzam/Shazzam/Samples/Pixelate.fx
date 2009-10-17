/// <class>PixelateEffect</class>

/// <description>An effect that turns the input into blocky pixels.</description>

//-----------------------------------------------------------------------------------------
// Shader constant register mappings (scalars - float, double, Point, Color, Point3D, etc.)
//-----------------------------------------------------------------------------------------

/// <summary>The number of horizontal and vertical pixel blocks.</summary>
/// <type>Size</type>
/// <minValue>20,20</minValue>
/// <maxValue>100,100</maxValue>
/// <defaultValue>60,40</defaultValue>
float2 PixelCounts : register(C0);

/// <summary>The amount to shift alternate rows (use 1 to get a brick wall look).</summary>
/// <minValue>0</minValue>
/// <maxValue>1</maxValue>
/// <defaultValue>0</defaultValue>
float BrickOffset : register(C1);

//--------------------------------------------------------------------------------------
// Sampler Inputs (Brushes, including ImplicitInput)
//--------------------------------------------------------------------------------------

sampler2D implicitInputSampler : register(S0);

//--------------------------------------------------------------------------------------
// Pixel Shader
//--------------------------------------------------------------------------------------

float4 main(float2 uv : TEXCOORD) : COLOR
{
   float2 brickSize = 1.0 / PixelCounts;

   // Offset every other row of bricks
   float2 offsetuv = uv;
   bool oddRow = floor(offsetuv.y / brickSize.y) % 2.0 >= 1.0;
   if (oddRow)
   {
       offsetuv.x += BrickOffset * brickSize.x / 2.0;
   }
   
   float2 brickNum = floor(offsetuv / brickSize);
   float2 centerOfBrick = brickNum * brickSize + brickSize / 2;
   float4 color = tex2D(implicitInputSampler, centerOfBrick);

   return color;
}


