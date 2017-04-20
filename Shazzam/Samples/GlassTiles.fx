/// <class>GlassTileEffect</class>
/// <description>An effect mimics the look of glass tiles.</description>
//  ------------------------------------------------------------------------------------

// contributed by Fakhruddin Faizal
// http://hdprogramming.blogspot.com/ 
// modifications by Walt Ritscher
// -----------------------------------------------------------------------------------------
// Shader constant register mappings (scalars - float, double, Point, Color, Point3D, etc.)
// -----------------------------------------------------------------------------------------


/// <summary>The approximate number of tiles per row/column.</summary>
/// <minValue>0</minValue>
/// <maxValue>20</maxValue>
///<defaultValue>5</defaultValue>
float Tiles : register(C0);

/// <summary>The gap width between the tiles.</summary>
/// <minValue>0/minValue>
/// <maxValue>10</maxValue>
///<defaultValue>1</defaultValue>
float BevelWidth : register(C1);

/// <summary>The offset for the upper left corner of the tiles.</summary>
/// <minValue>0/minValue>
/// <maxValue>3</maxValue>
///<defaultValue>1</defaultValue>
float Offset: register(C3);

/// <summary>The color for the gap between the tiles.</summary>
/// <minValue>0/minValue>
/// <maxValue>10</maxValue>
///<defaultValue>1</defaultValue>
float4 GroutColor : register(C2);

sampler2D input : register(s0);

float4 main(float2 uv : TEXCOORD) : COLOR
{
	float2 newUV1;
	newUV1.xy = uv.xy + tan((Tiles*2.5)*uv.xy + Offset)*(BevelWidth/100);
	
	float4 c1 = tex2D(input, newUV1); 
	if(newUV1.x<0 || newUV1.x>1 || newUV1.y<0 || newUV1.y>1)
	{
	
	c1 = GroutColor;
	}
	c1.a=1;
	return c1;
}

