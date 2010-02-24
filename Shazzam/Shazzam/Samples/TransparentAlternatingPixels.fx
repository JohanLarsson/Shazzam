//   Contributed by Rene Schulte
//   Copyright (c) 2009 Rene Schulte
//   http://kodierer.blogspot.com/2009/10/read-between-pixels-hlsl-kill-pixel.html

/// <class>TransparentAlternatingPixels</class>

/// <description>Pixel shader that samples the color from an image and draws every odd pixel transparent.</description>
/// 

// Parameters

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
	
	// Scale to int texture size, add x and y
	float2 vpos = texCoord * TextureSize * 0.5f;	
	float vposSum = vpos.x + vpos.y;
	
	// Calc diff between rounded half and half to get 0 or 0.5
	float diff = round(vposSum) - vposSum;
	float diffSq = diff * diff;
	
	// Even or odd? Only even pixels are sampled
	if(diffSq < 0.1)
	{
		color = tex2D(TexSampler, texCoord);
	}	
	return color;
}
