/// <class>FrostyOutlineEffect</class>
/// <description>An effect that turns the input into shades of a single color.</description>
//  ------------------------------------------------------------------------------------

// contributed by Fakhruddin Faizal
// http://hdprogramming.blogspot.com/ 

// -----------------------------------------------------------------------------------------
// Shader constant register mappings (scalars - float, double, Point, Color, Point3D, etc.)
// -----------------------------------------------------------------------------------------

/// <summary>The width of the frost.</summary>
/// <minValue>150</minValue>
/// <maxValue>650</maxValue>
/// <defaultValue>500</defaultValue>
float Width : register(C0);

/// <summary>The height of the frost.</summary>
/// <minValue>150</minValue>
/// <maxValue>400</maxValue>
/// <defaultValue>300</defaultValue>
float Height : register(C1);

//--------------------------------------------------------------------------------------
// Sampler Inputs (Brushes, including Texture1)
//--------------------------------------------------------------------------------------

sampler2D input : register(S0);


//--------------------------------------------------------------------------------------
// Pixel Shader
//--------------------------------------------------------------------------------------

float4 main(float2 middle : TEXCOORD) : COLOR

{
	float2 topLeft;
	float2 left;
	float2 bottomLeft;
	float2 top;
	float2 bottom;
	float2 topRight;
	float2 right;
	float2 bottomRight;


	topLeft.x = middle.x - 1/Width;
	topLeft.y = middle.y - 1/Height;
	top.x = middle.x;
	top.y = middle.y - 1/Height;
	topRight.x = middle.x + 1/Width;
	topRight.y = middle.y - 1/Height;
	left.x = middle.x - 1/Width;
	left.y = middle.y;
	right.x = middle.x + 1/Width;
	right.y = middle.y;
	bottomLeft.x = middle.x - 1/Width;
	bottomLeft.y = middle.y + 1/Height;
	bottom.x = middle.x;
	bottom.y = middle.y + 1/Height;
	bottomRight.x = middle.x + 1/Width;
	bottomRight.y = middle.y + 1/Height;
	
	
	float4 m = tex2D (input , middle);
	float4 tl = tex2D (input, topLeft);
	float4 l = tex2D (input, left);
	float4 bl = tex2D (input, bottomLeft);
	float4 t = tex2D (input, top);
	float4 b = tex2D (input, bottom);
	float4 tr = tex2D (input, topRight);
	float4 r = tex2D (input, right);
	float4 br = tex2D (input, bottomRight);
	
	
	
	float4 color = (-tl-t-tr) + (-l+8*m-r) + (-bl-b-br);
	float4 color2 = tex2D(input,middle);
	float avg=color.r+color.g+color.b;
	avg/=3;
	color.rgb=avg;
	color.a = 1;
	
	return color2+color;
}
