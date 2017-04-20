/// <class>WaveWarperEffect</class>

/// <description>An effect that applies a wave pattern to the input.</description>

// Copyright (C) 2009 Timmy Kokke 
// blog: http://blog.timmykokke.com
// email: info@timmykokke.com
// twitter: @sorskoot
 
sampler2D input : register(s0);

/// <summary>The moment in time. Animate this value over a long period of time. The speed depends on the 
/// size. The larger the size, the larger the increase in time on every frame, thus from 0 to 2048 in a smaller amount of time.
/// </summary>
/// <minValue>0/minValue>
/// <maxValue>2048</maxValue>
/// <defaultValue>0</defaultValue>
float Time : register(C0);

/// <summary>The distance between waves. (the higher the value the closer the waves are to their neighbor).</summary>
/// <minValue>32</minValue>
/// <maxValue>256</maxValue>
/// <defaultValue>64</defaultValue>
float WaveSize: register(C1);

float dist(float a, float b, float c, float d){
	return sqrt((a - c) * (a - c) + (b - d) * (b - d));
}

float4 main(float2 uv : TEXCOORD) : COLOR 
{ 
	float4 Color = 0;
	float f = sin(dist(uv.x + Time, uv.y, 0.128, 0.128)*WaveSize)
                  + sin(dist(uv.x, uv.y, 0.64, 0.64)*WaveSize)
                  + sin(dist(uv.x, uv.y + Time / 7, 0.192, 0.64)*WaveSize);
	uv.xy = uv.xy+((f/WaveSize));
	Color= tex2D( input , uv.xy);
	return Color; 
}
