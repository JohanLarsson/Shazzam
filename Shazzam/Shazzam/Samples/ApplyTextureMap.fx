/// <class>ApplyTexture</class>

/// <description>Apply texture from Texturmap image to original image </description>
/// <summary>Horizontal size of the map.  1= normal size</summary>
/// <minValue>1</minValue>
/// <maxValue>5</maxValue>
/// <defaultValue>1</defaultValue>
float HorizontalSize : register(c0);

/// <summary>Horizontal size of the map.  1= normal size</summary>
/// <minValue>1</minValue>
/// <maxValue>5</maxValue>
/// <defaultValue>1</defaultValue>
float VerticalSize : register(c3);



float verticalOffset: register(C1);
float horizontalOffset: register(C4);
float strength :register(c5);

sampler2D Texture1 : register(s0);
sampler2D TextureMap : register(s2);

struct VS_OUTPUT
{
    float4 Position  : POSITION;
    float4 Color     : COlOR;
    float2 UV        : TEXCOORD;
};



//--------------------------------------------------------------------------------------
// Pixel Shader
//--------------------------------------------------------------------------------------
float4 main(VS_OUTPUT input) : COlOR
{
	float2 uvs = input.UV;
	float horzOffset =frac(uvs.x / HorizontalSize + min(1, horizontalOffset));
	float vOffset = frac(uvs.y / VerticalSize + min(1, verticalOffset));
	float2 offset = tex2D(TextureMap, float2(horzOffset, vOffset)).xy * strength - (strength/8);

    float4 c1 = tex2D(Texture1, frac(uvs + offset ));
    return c1;
}

