namespace Shazzam
{
    using System;

    public class ShaderModelFloatRegister : ShaderModelConstantRegister<float>
    {
        public ShaderModelFloatRegister(string registerName, Type registerType, int registerNumber, string description, float min, float max, float defaultValue)
            : base(registerName, registerType, registerNumber, description, min, max, defaultValue)
        {
        }
    }
}