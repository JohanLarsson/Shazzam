//   Contributed by Rene Schulte
//   Copyright (c) 2009 Rene Schulte
//   http://kodierer.blogspot.com/2009/10/read-between-pixels-hlsl-kill-pixel.html

/// <class>TransparentAlternatingScanlines</class>
/// <description>Pixel shader that samples the color from an image and draws every odd row transparent.</description>
/// 


/// <summary>The Size of the texture.</summary>
/// <minValue>0,0</minValue>
/// <maxValue>2048,2048</maxValue>
/// <defaultValue>512,512</defaultValue>
float2 TextureSize : register(C0);

// Sampler
sampler2D TexSampler : register(S0);

// Shader
float4 main(float2 texCoord : TEXCOORD) : COLOR 
{ 
	// Default color is fully transparent
	float4 color = 0;
	
	// Scale to int texture size
	float row = texCoord.y * TextureSize.y * 0.5f;	
	
	// Calc diff between rounded half and half to get 0 or 0.5
	float diff = round(row) - row;
	float diffSq = diff * diff;
	
	// Even or odd? Only even lines are sampled
	if(diffSq < 0.1)
	{
		color = tex2D(TexSampler, texCoord);
	}	
	return color;
}
