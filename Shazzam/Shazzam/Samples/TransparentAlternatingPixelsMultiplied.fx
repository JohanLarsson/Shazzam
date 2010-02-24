//
//   Project:           Shaders
//
//   Description:       Pixel shader that samples the color from an image and draws every odd pixel transparent.
//
//   Changed by:        $Author: Rene $
//   Changed on:        $Date: 2009-07-25 12:53:09 +0200 (Sa, 25 Jul 2009) $
//   Changed in:        $Revision: 19 $
//   Project:           $URL: file:///U:/Data/Development/SVN/SilverlightDemos/trunk/DynamicBitmapComparsion/DynamicBitmapComparsion/MainPage.xaml.cs $
//   Id:                $Id: MainPage.xaml.cs 19 2009-07-25 10:53:09Z Rene $
//
//
//   Copyright (c) 2009 Rene Schulte
//

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
	float vposSum = vpos.x * vpos.y;
	
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
