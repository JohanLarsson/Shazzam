static float PI = 3.14159274f;
static float PI2 = 6.28318548f;

float4 lerp_rgba(float4 x, float4 y, float s)
{
    float a = lerp(x.a, y.a, s);
    float3 rgb = lerp(x.rgb, y.rgb, s) * a;
    return float4(rgb.r, rgb.g, rgb.b, a);
}

float interpolate(float min, float max, float value)
{
    if (min == max)
    {
        return 0.5;
    }

    if (min < max)
    {
        return clamp((value - min) / (max - min), 0, 1);
    }

    return clamp((value - max) / (min - max), 0, 1);
}


float clamp_angle(float angle, bool clockwise)
{
    angle %= PI2;
    if (clockwise && angle < 0)
    {
        return angle + PI2;
    }

    if (!clockwise && angle > 0)
    {
        return angle - PI2;
    }

    return angle;
}

float clockwise_angle(float2 v)
{
    float a = atan2(v.y, v.x);
    if (a < 0)
    {
        return a + PI2;
    }

    return a;
}

float angle_from_start(float clockwise_angle, float start_angle, bool clockwise)
{
    return clamp_angle(clockwise_angle - start_angle, clockwise);
}

float angle_from_start(float2 uv, float2 center_point, float start_angle, float central_angle)
{
    float2 v = uv - center_point;
    bool clockwise = central_angle > 0;
    return angle_from_start(
                clockwise_angle(v),
                start_angle,
                clockwise);
}


