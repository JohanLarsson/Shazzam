namespace Shazzam
{
    using System;

    public class ShaderModelDoubleRegister : ShaderModelConstantRegister<double>, IRegisterProperty
    {
        public ShaderModelDoubleRegister(string registerName, Type registerType, int registerNumber, string description, double min, double max, double defaultValue)
            : base(registerName, registerType, registerNumber, description, min, max, defaultValue)
        {
        }

        public string Name { get; } = string.Empty;
    }
}