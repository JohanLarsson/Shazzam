//The following restrictions apply when using a PS 3.0 shader. 

//When a PS 3.0 shader is assigned, the number of samplers increases to 8. Assign the PS 3.0 shader before other shaders to enable registering 8 samplers. 

// The full shader constant register limit of 224 for floats is used. For more information, see ps_3_0. 

// The following data types are supported in PS 3.0 shaders only. An exception is thrown if these are used in lower shader versions. 

// int and types convertible to int: uint, byte, sbyte, long, ulong, short, ushort, char

//bool

// if a valid PS 3.0 shader is loaded on a computer that does not have hardware support for PS 3.0, the shader is ignored. If the shader is invalid, no exception is thrown. 

// If a computer has more than one video card, the behavior is defined by the least capable video card. For example, if the computer has two video cards, one of which supports PS 3.0 and one of which does not, the behavior is the same as if the computer does not support PS 3.0.

//If a computer supports rendering PS 3.0 in hardware, but an invalid PS 3.0 shader is assigned, the InvalidPixelShaderEncountered event is raised. An example of an invalid PS 3.0 shader is one compiled with the ps_3_sw flag. The ShaderEffect class accepts only PS 3.0 shaders that are compiled with the ps_3_0 flag passed to fxc.exe. For more information, see Effect-Compiler Tool. 
