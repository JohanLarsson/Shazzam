/// <class>Wave Warper</class>

/// <description>An effect that applies a wave pattern to the input.</description>

// Created by Timmy Kokke 
// http://geekswithblogs.net/tkokke/Default.aspx 

sampler2D input : register(s0);
/// <summary>The wind offset to apply to affect. r.</summary>
/// <minValue>0</minValue>
/// <maxValue>5</maxValue>
/// <defaultValue>.5</defaultValue>
float WindOffset : register(C0);
/// <summary>The distance between waves. (the higher the value the closer the waves are to their neighbor).</summary>
/// <minValue>5</minValue>
/// <maxValue>150</maxValue>
/// <defaultValue>60</defaultValue>
float WaveSize: register(C1);

float dist(float a, float b, float c, float d){
	return sqrt((a - c) * (a - c) + (b - d) * (b - d));
}

float4 main(float2 uv : TEXCOORD) : COLOR 
{ 
	float4 Color = 0;
	float f = sin(dist(uv.x + WindOffset, uv.y, 0.128, 0.128)*WaveSize)
                  + sin(dist(uv.x, uv.y, 0.64, 0.64)*WaveSize)
                  + sin(dist(uv.x, uv.y + WindOffset / 7, 0.192, 0.64)*WaveSize);
	uv.xy = uv.xy+((f/WaveSize));
	Color= tex2D( input , uv.xy);
	return Color; 
}
