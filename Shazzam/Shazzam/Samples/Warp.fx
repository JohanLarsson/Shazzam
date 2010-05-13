// from http://fluidkit.codeplex.com/
// http://blog.pixelingene.com/?p=229

sampler2D Texture1 : register(s0);
/// <summary>The Size of the texture.</summary>
/// <minValue>0</minValue>
/// <maxValue>100</maxValue>
/// <defaultValue>2</defaultValue>
float LeftControlPoint0 : register(c0);

/// <summary>The Size of the texture.</summary>
/// <minValue>0</minValue>
/// <maxValue>100</maxValue>
/// <defaultValue>2</defaultValue>
float LeftControlPoint1 : register(c1);

/// <summary>The Size of the texture.</summary>
/// <minValue>0</minValue>
/// <maxValue>100</maxValue>
/// <defaultValue>2</defaultValue>
float LeftControlPoint2 : register(c2);

/// <summary>The Size of the texture.</summary>
/// <minValue>0</minValue>
/// <maxValue>100</maxValue>
/// <defaultValue>2</defaultValue>
float LeftControlPoint3 : register(c3);

/// <summary>The Size of the texture.</summary>
/// <minValue>0</minValue>
/// <maxValue>100</maxValue>
/// <defaultValue>2</defaultValue>
float RightControlPoint0 : register(c4);

/// <summary>The Size of the texture.</summary>
/// <minValue>0</minValue>
/// <maxValue>100</maxValue>
/// <defaultValue>2</defaultValue>
float RightControlPoint1 : register(c5);

/// <summary>The Size of the texture.</summary>
/// <minValue>0</minValue>
/// <maxValue>100</maxValue>
/// <defaultValue>2</defaultValue>
float RightControlPoint2 : register(c6);

/// <summary>The Size of the texture.</summary>
/// <minValue>0</minValue>
/// <maxValue>100</maxValue>
/// <defaultValue>2</defaultValue>
float RightControlPoint3 : register(c7);


float BezierInterpolate(float x0, float x1, float x2, float x3, float t)
    {float b0 = pow(1-t, 3);
    float b1 = 3*t*pow(1-t,2);
    float b2 = 3*t*t*(1-t);
    float b3 = pow(t, 3);
    
    return b0*x0 + b1*x1 + b2*x2 + b3*x3;
}

float4 main(float2 uv : TEXCOORD) : COLOR
{
    float left = BezierInterpolate(LeftControlPoint0, LeftControlPoint1, LeftControlPoint2, LeftControlPoint3, uv.y);
    float right = BezierInterpolate(RightControlPoint0, RightControlPoint1, RightControlPoint2, RightControlPoint3, uv.y);
    
    if (uv.x >= left && uv.x <= right)
    {
        float tx = lerp(0, 1, (uv.x-left)/(right-left));
        float2 pos = float2(tx, uv.y);
        
        return tex2D(implicitInput, pos);
    }
    else return float4(0,0,0,1);
}
