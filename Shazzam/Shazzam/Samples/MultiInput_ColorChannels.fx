sampler2D input : register(s0);

// new HLSL shader
/// <summary>Explain the purpose of this variable.</summary>

/// <defaultValue>c:\something\something.jpg</defaultValue> e.g. "c:\something\something.jpg" or "shazzam:sample1" "shazzam:noise", etc
sampler2D Texture1 : register(s1);

/// <summary>Change the ratio between the Red channel</summary>
/// <minValue>0/minValue>
/// <maxValue>1</maxValue>
/// <defaultValue>1</defaultValue>
float RedRatio : register(C0);

/// <summary>Change the ratio between the Blue channel </summary>
/// <minValue>0/minValue>
/// <maxValue>1</maxValue>
/// <defaultValue>0.5</defaultValue>
float BlueRatio : register(C1);

/// <summary>Change the ratio between the Green channel</summary>
/// <minValue>0/minValue>
/// <maxValue>1</maxValue>
/// <defaultValue>0.5</defaultValue>
float GreenRatio : register(C2);
float4 main(float2 uv : TEXCOORD) : COLOR
{

 float4 inputTex = tex2D(input, uv);
 float4 otherTex = tex2D(Texture1, uv);  // texture
inputTex.r = inputTex.r * (1-RedRatio ) + (otherTex.r * RedRatio);
inputTex.b = inputTex.b * (1-BlueRatio ) + (otherTex.b * BlueRatio);
inputTex.g = inputTex.g * (1-GreenRatio ) + (otherTex.g * GreenRatio);
 return inputTex;
}
