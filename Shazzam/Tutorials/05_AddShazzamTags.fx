/// XML tags with the triple-slash are Shazzam specific tags,
/// they are not part of the HLSL specification and are parsed
/// by Shazzam and used to provide semantic or compiler information
///-------------------------------------------------------------
// Provide a description for this Shader.
// Shazzam will generate comments from this description.
/// <description>Provide </description>
/// 
/// Specify a default Class name for generated file. Otherwise it will use the file name
/// <class>SuggestedClassName</class>
/// =============================================================================

sampler2D ourImage : register(s0);

//  Shazzam contains a number of XML tags
//  that provide extra information for each top-level variable 
//  The also permit storing min, max and defaults for the variable
//  Everytime the HLSL code is compile the these values are assigned  to the WPF
//  defaults for the WPF dependency property

/// <summary>Modifies the Red value.</summary> // appears in the Shazzam tooltips
/// <minValue>0</minValue>
/// <maxValue>1</maxValue>
/// <defaultValue>1</defaultValue>
float RedValue : register(C0);

/// <summary>Modifies the Green value.</summary> 
/// <minValue>0</minValue>
/// <maxValue>1</maxValue>
/// <defaultValue>.5</defaultValue>
float GreenValue : register(C1);
/// <summary>Modifies the Blue value.</summary> 
/// <minValue>0</minValue>
/// <maxValue>1</maxValue>
/// <defaultValue>.5</defaultValue>
float BlueValue : register(C2);
float4 main(float2 locationInSource : TEXCOORD) : COLOR
{
  // create a variable to hold our color
  float4 color;
  // get the color of the current pixel
  color = tex2D( ourImage , locationInSource.xy);


  // assign the variables
  color.r *= RedValue; 
  color.g *= GreenValue;
  color.b *= BlueValue;


// modify these variable at runtime in Shazzam with the controls in the 'Tryout shader (adjust settings)' tab.



    return color;

}
