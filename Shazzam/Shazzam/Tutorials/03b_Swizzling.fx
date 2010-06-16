// Tutorial Title:  Swizzling
// Lesson Plan   :  Learn about Vector types and the Swizzling syntax
sampler2D ourImage : register(s0);

void ShowVectorSyntax()
{
// vectors in HLSL are groups of the same variable type
    // a contains one float
    float a;

    // b contains two floats
    float2 b = 5.6f;

    // what about these?
    int3 c;
    int4 d;

    // this would be handy for holding a 3D point
    float3 pointDemo;

    // this could hold a color or any other  4 floats
    float4 color;

    // less common syntax
    vector<float,4> anotherColor;

    // access the value stored in the vector
    float redVal = color[0];
    float greenVal = color[1];

    // get value using one of the component namespaces
    redVal = color.r; // can also use color.b, color.g and color.a
    // the other component namespace
    redVal = color.x; // can also use color.y, color.z, color.w

    // this also works for different size vectors
    float2 currentPoint;
    float result = currentPoint.y;
    result = currentPoint.g;      // looks odd, but works

}

void SwizzlingDemo()
{
  // what is Swizzling?
  // you can combine access to the individual vector values in a single line
  //  this can result in faster code on the GPU
  float4 color={8,8,8,8};
  float2 blueAndGreen = {color[1],color[2]}; // not swizzling
  float2 redAndBlue= color.rb;  // this is swizzling

  // beware, you cannot combine component namespaces
  // float2 yuck = color.rw ;  // won't compile
}

float4 main(float2 locationInSource : TEXCOORD) : COLOR
{

    float4 color ={0,0,0,0};
    return color;

}