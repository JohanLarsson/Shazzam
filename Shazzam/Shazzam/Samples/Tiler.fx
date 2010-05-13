/// <class>Tiler</class>
/// <description>Pixel shader tiles the image across multiple rows and columns</description>
///

// Created by A.Boschin: andrea@boschin.it
// site: http://www.silverlightplayground.org
// blog: http://blog.boschin.it


/// <summary>The number of verical tiles to add to the output. The higher the value the more tiles.</summary>
/// <minValue>0</minValue>
/// <maxValue>20</maxValue>
/// <defaultValue>4</defaultValue>
float VerticalTileCount : register(C1);

/// <summary>The number of horizontal tiles to add to the output. The higher the value the more tiles.</summary>
/// <minValue>0</minValue>
/// <maxValue>20</maxValue>
/// <defaultValue>3</defaultValue>
float HorizontalTileCount : register(C2);

/// <summary>Change the horizontal offset of each tile.</summary>
/// <minValue>0</minValue>
/// <maxValue>1</maxValue>
/// <defaultValue>0</defaultValue>
float HorizontalOffset : register(C3);

/// <summary>Change the vertical offset of each tile.</summary>
/// <minValue>0</minValue>
/// <maxValue>1</maxValue>
/// <defaultValue>0</defaultValue>
float VerticalOffset : register(C4);


sampler2D Texture1Sampler : register(S0);

float4 main(float2 uv : TEXCOORD) : COLOR
{
	float2 newUv = float2((uv.x * HorizontalTileCount % 1) + HorizontalOffset , (uv.y * VerticalTileCount % 1)+VerticalOffset) ;
	return tex2D( Texture1Sampler, newUv );

}
