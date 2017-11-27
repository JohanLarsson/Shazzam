namespace Shazzam
{
    using System;
    using System.Windows;

    public class ShaderModelVectorRegister : ShaderModelPairRegister<Vector>
    {
        public ShaderModelVectorRegister(string registerName, Type registerType, int registerNumber, string description, Vector min, Vector max, Vector defaultValue)
            : base(
                registerName,
                registerType,
                registerNumber,
                description,
                min,
                max,
                defaultValue,
                x => new RegisterProperty<Vector>("X", x, p => p.X, (p, d) => new Vector(d, p.Y)),
                x => new RegisterProperty<Vector>("Y", x, p => p.Y, (p, d) => new Vector(p.X, d)))
        {
        }
    }
}