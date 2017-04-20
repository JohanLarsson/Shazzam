 
sampler2D input : register(s0);
/// <summary>CenterX.</summary>
/// <minValue>2/minValue>
/// <maxValue>12</maxValue>
/// <defaultValue>1</defaultValue>
float CenterX : register(C0);


/// <minValue>-10/minValue>
/// <maxValue>10</maxValue>
/// <defaultValue>.128</defaultValue>
float CenterY : register(C1);

/// <summary>The distance between bands. (the higher the value the closer the bands are to their neighbor).</summary>
/// <minValue>0</minValue>
/// <maxValue>250</maxValue>
/// <defaultValue>48</defaultValue>
float BandSize: register(C2);


	
float distort(float x, float y, float c, float d){
	return sqrt((x - c) * (x - c) + (y - d) * (y - d));
}

float4 main(float2 uv : TEXCOORD) : COLOR 
{ 
	float4 Color = 0;

  float wobble = sin (distort(uv.x ,uv.y ,CenterX ,CenterY) * BandSize);
	uv.xy = uv.xy+(( wobble/BandSize ));
	Color= tex2D( input , uv.xy);
	return Color; 
}
